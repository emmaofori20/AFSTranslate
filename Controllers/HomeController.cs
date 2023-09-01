using AFSTranslate.Models;
using AFSTranslate.Services;
using AFSTranslate.ViewModels;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AFSTranslate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITranslationService _translationService;

        public HomeController(ILogger<HomeController> logger, ITranslationService translationService)
        {
            _logger = logger;
            this._translationService = translationService;
        }

        public async Task<IActionResult> Index()
        {
            var userdetails = GetUserIdAndUserName();
            var homeViewModel = new HomeViewModel()
            {
                Responses = await _translationService.GetAllResponsesAsync(userdetails.userId),
                Translation = _translationService.SetupTranslation()
            };
           return View(homeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TranslateText(HomeViewModel model)
        {
           
                try
                {
                var userdetails = GetUserIdAndUserName();
                var translatedText = await _translationService.TranslateTextAsync(model.Translation.TextToTranslate, model.Translation.TranslationTYpeId, userdetails.userId);
                ViewBag.Message = "Translation successful!";

                return Json(translatedText);

                }
                catch (HttpRequestException ex)
                {
                ViewBag.Message = "Translation failed: " + ex.Message;

                ModelState.AddModelError(string.Empty, $"API request failed: {ex.Message}");
                return Json(_translationService.GetLatestTranslation());

            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        private (string userId, string userName) GetUserIdAndUserName()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return (userId, userName);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}