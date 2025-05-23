using System.Security.Claims;
using ASC.Model.BaseTypes;
using ASC.Model.Models;
using ASC_Web.Configuration;
using ASC_Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASC_Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public DashboardController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Dashboard";

            return View();
        }
    }
}
