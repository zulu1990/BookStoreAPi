using BookStoreApi.Contracts;
using BookStoreApi.Contracts.DTO;
using BookStoreApi.Contracts.Requests;
using BookStoreApi.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreAPi.BL.Interfaces
{
    public interface IBookManager
    {
        Task<Result> AddBook(AddBook book);
        Task<Result<List<BookDto>>> SearchBooks(SearchBook searchParams);
        Task<Result<Dictionary<Guid, BookStoreResponse>>> GetBookStoreInfo();
        Task<Result> EditBook(EditBook editBook);
    }
}
