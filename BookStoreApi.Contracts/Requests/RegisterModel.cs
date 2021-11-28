using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Contracts.Requests
{
    public class RegisterModel
    {

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string AdminSecret { get; set; }
    }
}
