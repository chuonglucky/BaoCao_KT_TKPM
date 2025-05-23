using System.Diagnostics;
using ASC_Web.Configuration;
using ASC_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASC_Web.Controllers
{
    public class HomeController : AnonymousController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<ApplicationSettings> _settings; // Được highlight

        public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettings> settings) // "settings" được highlight
        {
            _logger = logger;
            _settings = settings; // Được highlight
        }

        public IActionResult Index()
        {
            ViewBag.Title = _settings.Value.ApplicationTitle; // Được highlight
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
