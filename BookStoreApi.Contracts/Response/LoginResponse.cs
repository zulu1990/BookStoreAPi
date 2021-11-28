using BookStoreApi.Contracts.DTO;

namespace BookStoreApi.Contracts.Response
{
    public class LoginResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
