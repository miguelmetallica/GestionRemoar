using Gestion.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Gestion.Web.Helpers
{
    public interface IUserHelper
    {
        Task<Usuarios> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(Usuarios user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(Usuarios user);

        Task<IdentityResult> ChangePasswordAsync(Usuarios user, string oldPassword, string newPassword);

        Task<SignInResult> ValidatePasswordAsync(Usuarios user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(Usuarios user, string roleName);

        Task<bool> IsUserInRoleAsync(Usuarios user, string roleName);


    }
}
