using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{

    public class PawRoleController : Controller
    {
        private readonly IPawRoleService _pawRoleService;
        private readonly ILogger<PawRoleController> _logger;

        public PawRoleController(IPawRoleService pawRoleService, ILogger<PawRoleController> logger)
        {
            _pawRoleService = pawRoleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _pawRoleService.GetPawRolesAsync();
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}