using AutoMapper;
using BookStoreApi.Contracts;
using BookStoreApi.Contracts.DTO;
using BookStoreApi.Contracts.Entity;
using BookStoreApi.Contracts.Requests;
using BookStoreApi.Contracts.Response;
using BookStoreAPi.BL.Interfaces;
using BookStoreAPi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreAPi.BL.Managers
{
    public class BookManager : IBookManager
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookManager(IGenericRepository<Book> bookRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> AddBook(AddBook newBook)
        {
            var res = await _bookRepository.GetByExpression(x => x.BookIdentifier == newBook.BookIdentifier);

            var newBooks = GetBooks(newBook);

            await _bookRepository.AddRange(newBooks);
            await _unitOfWork.CommitAsync();

            return Result.Succeed();
           
        }

        public async Task<Result<Dictionary<Guid, BookStoreResponse>>> GetBookStoreInfo()
        {
            var storeResult = await _bookRepository.ListAsync(x => x.PurchaseHistoryId == null);
            var result = GenerateStoreInfoResponse(storeResult.Data);

            return result;
        }

        public async Task<Result> EditBook(EditBook editBook)
        {
            var booksInStore = await _bookRepository.ListAsync(x => x.BookIdentifier == editBook.BookIdentifier, trackChanges: true);

            foreach (var book in booksInStore.Data)
            {
                book.Title = editBook.Title;
                book.Genre = editBook.Genre;
                book.Price = editBook.Price;
                book.Author = editBook.Author;
            }

            return await _unitOfWork.CommitAsync() ? Result.Succeed() : Result.Fail("Error");
        }


        public async Task<Result<List<BookDto>>> SearchBooks(SearchBook searchParams)
        {
            var bookResult = await _bookRepository.ListAsync(x =>
                x.Title.ToLower().Contains(searchParams.Title.ToLower()) &&
                x.Author.ToLower().Contains(searchParams.Author.ToLower()) &&
                x.BookIdentifier.ToString().ToLower().Contains(searchParams.BookIdentifier), orderBy: b => b.OrderBy(o => o.Price));

            if (bookResult.Success == false)
                return Result<List<BookDto>>.Fail("");

            var result = GetBooks(bookResult.Data);

            return Result<List<BookDto>>.Succeed(result);
        }


        private List<Book> GetBooks(AddBook newBook)
        {
            var result = new List<Book>();

            for(int i = 0; i< newBook.Count; i++)
            {
                var book = _mapper.Map<Book>(newBook);
                result.Add(book);
            }

            return result;
        }
        private List<BookDto> GetBooks(IList<Book> books)
        {
            var result = new List<BookDto>();

            foreach (var book in books)
            {
                if(result.FirstOrDefault(x=>x.BookIdentifier == book.BookIdentifier) == null)
                    result.Add(_mapper.Map<BookDto>(book));
            }

            return result;
        }
        private Result<Dictionary<Guid, BookStoreResponse>> GenerateStoreInfoResponse(IList<Book> books)
        {
            var result = new Dictionary<Guid, BookStoreResponse>();

            foreach (var book in books)
            {
                if (result.TryGetValue(book.BookIdentifier, out var bookInList))
                {
                    bookInList.RemainingCount++;
                }
                else
                {
                    result.Add(book.BookIdentifier, new BookStoreResponse { BookIdentifier = book.BookIdentifier, RemainingCount = 1, Title = book.Title});
                }
            }

            return Result<Dictionary<Guid, BookStoreResponse>>.Succeed(result);
        }

       
    }
}
