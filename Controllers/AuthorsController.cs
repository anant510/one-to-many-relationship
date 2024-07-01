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
    public class AuthorsController : ControllerBase
    {
        private readonly TestDbContext _context;
        private readonly IMapper _mapper; // AutoMapper for mapping entities to DTOs
        public AuthorsController(TestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();

            var authorDtos = _mapper.Map<List<AuthorDto>>(authors);

            return authorDtos;
        }

        // GET: api/authors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = _mapper.Map<AuthorDto>(author);

            return authorDto;
        }

        // POST: api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorForCreationDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorDtoToReturn = _mapper.Map<AuthorDto>(author);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, authorDtoToReturn);
        }

        // PUT: api/authors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorForUpdateDto authorDto)
        {
            var authorToUpdate = await _context.Authors.FindAsync(id);

            if (authorToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(authorDto, authorToUpdate);

            _context.Entry(authorToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // DELETE: api/authors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }

    }
}
