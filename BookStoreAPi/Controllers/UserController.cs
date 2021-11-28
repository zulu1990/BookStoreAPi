using BookStoreApi.Contracts.Requests;
using BookStoreAPi.BL.Interfaces;
using BookStoreAPi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookStoreAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var registrationResult = await _userManager.Register(model);

            return registrationResult.Success ? Ok(registrationResult.Data) : BadRequest(registrationResult.ErrorMessage);
        }


        //[Authorize]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddBooksToCart(AddToCart addToCart)
        {
            //var userId = HttpContext.GetUserId();

            var userId = 1;
            addToCart.BookIdentifier = Guid.Parse("850B6B8A-2AE0-4B0E-9A15-2A57119CA3EE");
            addToCart.Count = 2;

            var result = await _userManager.AddBooksToCart(userId, addToCart);

            return result.Success? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        //[Authorize]
        [HttpPost("purchase-cart-items")]
        public async Task<IActionResult> PurchaseBooks()
        {
            //var userId = HttpContext.GetUserId();

            var userId = 1;

            var result = await _userManager.PurchaseBooks(userId);

            return result.Success? NoContent() : BadRequest(result.ErrorMessage);
        }



        //[Authorize]
        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCartItems()
        {
            //var userId = HttpContext.GetUserId();

            var userId = 1;

            var result = await _userManager.GetUserCartInfo(userId);

            return result.Success ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        //[Authorize]
        [HttpGet("get-purchase-history")]
        public async Task<IActionResult> GetPurchases()
        {
            //var userId = HttpContext.GetUserId();

            var userId = 1;

            var result = await _userManager.GetPurchasesHistory(userId);

            return result.Success ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }



       // [Authorize(Roles = "Admin")]
        [HttpGet("get-customer-purchase/{customerId}")]
        public async Task<IActionResult> GetCustomerPurchases(long customerId)
        {
            var result = await _userManager.GetPurchasesHistory(customerId);

            return result.Success ? Ok(result.Data) : BadRequest();
        }

    }
}
