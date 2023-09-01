using AFSTranslate.Models;
using AFSTranslate.Services;
using AFSTranslate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AFSTranslate.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccessService _accessService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccessController(IAccessService accessService, SignInManager<ApplicationUser> signInManager)
        {
            this._accessService = accessService;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _accessService.SignInUserAsync(model);

                if (signInResult)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(model);

                }

            }

            return View();
        }

        [HttpGet]
        public IActionResult SignUp() {
            return View();
        }  
        
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model) {

            if (ModelState.IsValid)
            {
                var registrationResult = await _accessService.RegisterUserAsync(model);

                if (registrationResult)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User registration failed.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Access");
        }

    }
}
