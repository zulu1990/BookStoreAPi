using BookStoreApi.Contracts.Requests;
using BookStoreAPi.BL.Interfaces;
using BookStoreAPi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStoreAPi.Controllers
{
    public class BookContoroller : ControllerBase
    {
        private readonly IBookManager _bookManager;

        public BookContoroller(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }


        [HttpPost("search-books")]
        public async Task<IActionResult> SearchBooks(SearchBook searchParams)
        {
            var result = await _bookManager.SearchBooks(searchParams);

            return result.Success? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

#region ADMINS

        [Authorize(Roles = "Admin")]
        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook(AddBook book)
        {
            var result = await _bookManager.AddBook(book);

            return result.Success ? NoContent() : BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("store-info")]
        public async Task<IActionResult> GetStorageInfo()
        {
            var result = await _bookManager.GetBookStoreInfo();

            return result.Success? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("edit-book")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditBook(EditBook editBook)
        {
            var result = await _bookManager.EditBook(editBook);

            return result.Success ? NoContent() : BadRequest(result.ErrorMessage);
        }

#endregion

    }
}
