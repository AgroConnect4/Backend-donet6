using agroApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(string username, string email, string password);
        Task<SignInResult> LoginAsync(string email, string password);
        Task<string> GenerateTokenAsync(User user);
        Task ForgotPasswordAsync(string email);
        bool IsValidEmail(string email);
    }
}
