using AFSTranslate.Models;
using AFSTranslate.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AFSTranslate.Services
{
    public interface IAccessService
    {
        Task<bool> RegisterUserAsync(SignUpViewModel model);
        Task<bool> SignInUserAsync(LoginViewModel model);
    }
    public class AccessService : IAccessService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccessService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<bool> RegisterUserAsync(SignUpViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Surname= model.SurName,
                Othername = model.OtherName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }

            return false;
        }

        public async Task<bool> SignInUserAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }
    }
}
