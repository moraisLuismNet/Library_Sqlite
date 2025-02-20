using AutoMapper;
using Library.DTOs;
using Library.Models;

namespace Library.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Author, AuthorInsertDTO>().ReverseMap();
            CreateMap<Author, AuthorBookDTO>().ReverseMap();
            CreateMap<Author, AuthorUpdateDTO>().ReverseMap();
            CreateMap<PublishingHouse, PublishingHouseDTO>().ReverseMap();
            CreateMap<PublishingHouse, PublishingHouseInsertDTO>().ReverseMap();
            CreateMap<PublishingHouse, PublishingHouseBookAuthorDTO>().ReverseMap();
            CreateMap<PublishingHouse, PublishingHouseBookDTO>().ReverseMap();
            CreateMap<PublishingHouse, PublishingHouseUpdateDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, BookInsertDTO>().ReverseMap();
            CreateMap<Book, BookItemDTO>().ReverseMap();
            CreateMap<Book, BookUpdateDTO>().ReverseMap();
        }
    }
}
