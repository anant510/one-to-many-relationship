namespace otomrelationship.DTO
{
    public class AuthorForCreationDto
    {
        public string Name { get; set; }
        public List<BookForCreationDto> Books { get; set; }
    }
}
