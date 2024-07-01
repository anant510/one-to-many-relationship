using AutoMapper;
using otomrelationship.DTO;
using otomrelationship.Models;

namespace otomrelationship.Automapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<Book, BookDto>();

            CreateMap<AuthorDto, Author>();
            CreateMap<BookDto, Book>();
            CreateMap<AuthorForCreationDto, Author>();
            CreateMap<BookForCreationDto, Book>();
            CreateMap<AuthorForUpdateDto, Author>();
        }
    }
}
