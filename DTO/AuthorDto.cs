namespace otomrelationship.DTO
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
