using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Contracts.Requests
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
