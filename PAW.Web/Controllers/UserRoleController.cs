using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{

    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(IUserRoleService userRoleService, ILogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userRoleService.GetUserRolesAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRoleDTO userRole)
        {
            if (!ModelState.IsValid) return View(userRole);
            await _userRoleService.CreateUserRoleAsync(userRole);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(decimal id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
            if (userRole == null) return NotFound();
            return View(userRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(decimal id, UserRoleDTO userRole)
        {
            if (!ModelState.IsValid) return View(userRole);
            await _userRoleService.UpdateUserRoleAsync(id, userRole);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(decimal id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
            if (userRole == null) return NotFound();
            return View(userRole);
        }

        public async Task<IActionResult> Delete(decimal id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
            if (userRole == null) return NotFound();
            return View(userRole);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            await _userRoleService.DeleteUserRoleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}