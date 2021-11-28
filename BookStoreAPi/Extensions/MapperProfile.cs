using AutoMapper;
using BookStoreApi.Contracts.DTO;
using BookStoreApi.Contracts.Entity;
using BookStoreApi.Contracts.Requests;

namespace BookStoreAPi.Extensions
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<AddBook, Book>();
            CreateMap<Book, BookDto>();
        }
    }

}