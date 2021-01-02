using Gestion.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<Usuarios> userManager;

        public readonly SignInManager<Usuarios> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserHelper(UserManager<Usuarios> userManager, 
            SignInManager<Usuarios> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(Usuarios user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        public async Task<Usuarios> GetUserByEmailAsync(string email)
        {
            return await this.userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await this.signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(Usuarios user)
        {
            return await this.userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(Usuarios user, string oldPassword, string newPassword)
        {
            return await this.userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<SignInResult> ValidatePasswordAsync(Usuarios user, string password)
        {
            return await this.signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await this.roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task AddUserToRoleAsync(Usuarios user, string roleName)
        {
            await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(Usuarios user, string roleName)
        {
            return await this.userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Usuarios user, string token)
        {
            return await this.userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(Usuarios user)
        {
            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<Usuarios> GetUserByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(Usuarios user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(Usuarios user, string token, string password)
        {
            return await this.userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<List<Usuarios>> GetAllUsersAsync()
        {
            return await this.userManager.Users
                .Include(u => u.Sucursal)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.userManager.Users.Select(c => new SelectListItem
            {
                Text = c.UserName,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Usuario...)",
                Value = ""
            });

            return list;
        }

        public async Task RemoveUserFromRoleAsync(Usuarios user, string roleName)
        {
            await this.userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task DeleteUserAsync(Usuarios user)
        {
            await this.userManager.DeleteAsync(user);
        }


    }

}
