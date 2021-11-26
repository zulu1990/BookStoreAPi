using BookStoreApi.Contracts;
using BookStoreApi.Contracts.Response;
using System.Threading.Tasks;

namespace BookStoreAPi.BL.Interfaces
{
    public interface IAuthManager
    {
        Task<Result<LoginResponse>> Login(string username, string password);
        Task<Result> Logout(string token);
    }
}
