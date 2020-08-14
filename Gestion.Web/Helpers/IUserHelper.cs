using Gestion.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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

        Task<string> GenerateEmailConfirmationTokenAsync(Usuarios user);

        Task<IdentityResult> ConfirmEmailAsync(Usuarios user, string token);

        Task<Usuarios> GetUserByIdAsync(string userId);

        Task<string> GeneratePasswordResetTokenAsync(Usuarios user);

        Task<IdentityResult> ResetPasswordAsync(Usuarios user, string token, string password);

        Task<List<Usuarios>> GetAllUsersAsync();

        Task RemoveUserFromRoleAsync(Usuarios user, string roleName);

        Task DeleteUserAsync(Usuarios user);

    }
}
