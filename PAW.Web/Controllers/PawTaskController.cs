using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{

    public class PawTaskController : Controller
    {
        private readonly IPawTaskService _pawTaskService;
        private readonly ILogger<PawTaskController> _logger;

        public PawTaskController(IPawTaskService pawTaskService, ILogger<PawTaskController> logger)
        {
            _pawTaskService = pawTaskService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _pawTaskService.GetPawTasksAsync();
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}