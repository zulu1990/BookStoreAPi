using BookStoreApi.Contracts;
using BookStoreApi.Contracts.DTO;
using BookStoreApi.Contracts.Entity;
using BookStoreApi.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreAPi.BL.Interfaces
{
    public interface IUserManager
    {
        Task<Result<User>> Register(RegisterModel model);
        Task<Result<List<BookDto>>> AddBooksToCart(long userId, AddToCart addToCart);
        Task<Result<List<BookDto>>> GetUserCartInfo(long userId);
        Task<Result<List<BookDto>>> GetPurchasesHistory(long userId);
        Task<Result> PurchaseBooks(long userId);
    }
}
