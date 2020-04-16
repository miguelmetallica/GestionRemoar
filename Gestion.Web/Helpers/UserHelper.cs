using Gestion.Web.Models;
using Microsoft.AspNetCore.Identity;
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


    }

}
