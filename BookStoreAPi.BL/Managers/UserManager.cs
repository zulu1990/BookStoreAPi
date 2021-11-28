using AutoMapper;
using BookStoreApi.Contracts;
using BookStoreApi.Contracts.DTO;
using BookStoreApi.Contracts.Entity;
using BookStoreApi.Contracts.Enums;
using BookStoreApi.Contracts.Requests;
using BookStoreAPi.BL.Interfaces;
using BookStoreAPi.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreAPi.BL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<PurchaseHistory> _purchaseHistoryRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserManager(IUnitOfWork unitOfWork, IGenericRepository<User> userRepository, IConfiguration config,
            IGenericRepository<Cart> cartRepository, IGenericRepository<Book> bookRepository, IMapper mapper,
            IGenericRepository<PurchaseHistory> purchaseHistoryRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _config = config;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _purchaseHistoryRepository = purchaseHistoryRepository;
        }

        public async Task<Result<User>> Register(RegisterModel model)
        {
            bool isAdmin = string.IsNullOrEmpty(model.AdminSecret) == false;

            var isRegisterModelValid = await IsValidRegisterModel(model);

            if (isRegisterModelValid == false)
                return Result<User>.Fail("Username Exists");

            if (isAdmin && IsValidAdminModel(model) == false)
                 return Result<User>.Fail("Invalid Secret");

            CreateSaltAndHash(model.Password, out var passwordHash, out var passwordSalt);

            var user = isAdmin ? CreateNewAdmin(model, passwordHash, passwordSalt) : CreateNewCustomer(model, passwordHash, passwordSalt);

            var result = await _userRepository.Add(user);

            if (await _unitOfWork.CommitAsync())
                return result;

            return Result<User>.Fail("Couldn't Register");
        }

        public async Task<Result<List<BookDto>>> AddBooksToCart(long userId, AddToCart addToCart)
        {
            var cartResult = await GetCart(userId, trackChanges: true);

            var cart = cartResult.Data;

            var booksResult = await _bookRepository.ListAsync(x => x.BookIdentifier == addToCart.BookIdentifier && x.Locked == false && x.CartId == null,
                orderBy: q => q.OrderBy(a=> a.Id), count: addToCart.Count);

            if (booksResult.Success == false)
                return Result<List<BookDto>>.Fail("");

            var books = booksResult.Data;
            
            cart.Books.AddRange(books);

            foreach (var book in books)
            {
                book.Locked = true;
                book.Cart = cart;
            }

            if(await _unitOfWork.CommitAsync())
            {
                var result = _mapper.Map<List<BookDto>>(cart.Books);
                return Result<List<BookDto>>.Succeed(result);
            }

            return Result<List<BookDto>>.Fail("");
            
        }

        public async Task<Result<List<BookDto>>> GetUserCartInfo(long userId)
        {
            var cartResult = await GetCart(userId, trackChanges: false);

            var result = _mapper.Map<List<BookDto>>(cartResult.Data.Books);

            return Result<List<BookDto>>.Succeed(result);
        }

        private async Task<Result<Cart>> GetCart(long userId, bool trackChanges)
        {
            var cartResult = await _cartRepository.GetByExpression(x => x.UserId == userId, includes: new List<string> { "Books" }, trackChanges: trackChanges);

            if (cartResult.Success == false)
                return Result<Cart>.Fail("");

            var cart = cartResult.Data;
            return Result<Cart>.Succeed(cart);
        }

        private async Task<Result<PurchaseHistory>> GetHistory(long userId, bool trackChanges)
        {
            var purchasesResult = await _purchaseHistoryRepository.GetByExpression(x => x.UserId == userId, includes: new List<string> { "Books" }, trackChanges: trackChanges);

            if (purchasesResult.Success == false)
                return Result<PurchaseHistory>.Fail("");

            var purchases = purchasesResult.Data;

            return Result<PurchaseHistory>.Succeed(purchases);
        }


        public async Task<Result<List<BookDto>>> GetPurchasesHistory(long userId)
        {
           var purchasesResult = await GetHistory(userId, trackChanges: false);

            var result = _mapper.Map<List<BookDto>>(purchasesResult.Data.Books);
            return Result<List<BookDto>>.Succeed(result);

        }


        public async Task<Result> PurchaseBooks(long userId)
        {
            
            var cartResult = await GetCart(userId, trackChanges: true);
            var cart = cartResult.Data;

            var purchasesResult = await GetHistory(userId, trackChanges: true);

            var purchaseHistory = purchasesResult.Data;
            var purchasedBooks = purchaseHistory.Books;

            purchasedBooks.AddRange(cart.Books);

            foreach (var book in cart.Books)
            {
                book.PurchaseHistory = purchaseHistory;
                book.Cart = null;
            }

            return await _unitOfWork.CommitAsync() ? Result.Succeed() : Result.Fail("Cant Purchase");
        }






        #region Private Helpers
        private static void CreateSaltAndHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        private static User CreateNewCustomer(RegisterModel model, byte[] passwordHash, byte[] passwordSalt)
        {
            var user = new User()
            {
                Username = model.Username,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                Role = Role.Customer,
                PurchaseHistory = new PurchaseHistory(),
                Cart = new Cart()
            };

            return user;
        }
        private static User CreateNewAdmin(RegisterModel model, byte[] passwordHash, byte[] passwordSalt)
        {
            var user = new User()
            {
                Username = model.Username,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                Role = Role.Admin,
                PurchaseHistory = new PurchaseHistory(),
                Cart = new Cart()
            };

            return user;
        }
        private async Task<bool> IsValidRegisterModel(RegisterModel model)
        {
            var existingUserResult =
                 await _userRepository.GetByExpression(x => x.Username.ToLower() == model.Username);

            return existingUserResult.Success ? false : true;
        }
        private bool IsValidAdminModel(RegisterModel model)
        {
            return model.AdminSecret != _config.GetSection("Secrets:AdminRegistringKey").Value
                ? false
                : true;
        }

        #endregion



    }
}
