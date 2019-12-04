using System.Threading.Tasks;
using VivesRental.RestApi.Contracts;

namespace VivesRental.RestApi.Services.Contracts
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
