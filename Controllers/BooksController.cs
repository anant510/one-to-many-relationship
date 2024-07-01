using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otomrelationship.context;
using otomrelationship.DTO;
using otomrelationship.Models;

namespace otomrelationship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly TestDbContext _context;
        private readonly IMapper _mapper; // AutoMapper for mapping entities to DTOs

        public BooksController(TestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            var bookDtos = _mapper.Map<List<BookDto>>(books);

            return bookDtos;
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;
        }
        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(BookForCreationDto bookDto)
        {
            // Map the bookDto to Book entity
            var book = _mapper.Map<Book>(bookDto);

            // Check if Author_ID is provided
            if (!string.IsNullOrEmpty(bookDto.AuthorId))
            {
                // Retrieve the author from the database
                var author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == int.Parse(bookDto.AuthorId));

                // If author not found, return BadRequest
                if (author == null)
                {
                    return BadRequest("Author not found");
                }

                // Assign the author to the book
                book.AuthorId = author.AuthorId;
                book.Author = author; // Optional: If you want to load the entire author entity
            }

            // Add the book to DbContext and save changes
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Map the inserted book back to BookDto
            var bookDtoToReturn = _mapper.Map<BookDto>(book);

            // Return the created bookDto with CreatedAtAction
            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, bookDtoToReturn);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookForCreationDto bookDto)
        {
            var bookToUpdate = await _context.Books.FindAsync(id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(bookDto, bookToUpdate);

            _context.Entry(bookToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
