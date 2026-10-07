using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PawRoleDTO role)
        {
            if (!ModelState.IsValid) return View(role);
            await _pawRoleService.CreatePawRoleAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _pawRoleService.GetPawRoleByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PawRoleDTO role)
        {
            if (!ModelState.IsValid) return View(role);
            await _pawRoleService.UpdatePawRoleAsync(id, role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var role = await _pawRoleService.GetPawRoleByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _pawRoleService.GetPawRoleByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pawRoleService.DeletePawRoleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}