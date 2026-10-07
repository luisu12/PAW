using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PawTaskDTO task)
        {
            if (!ModelState.IsValid) return View(task);
            await _pawTaskService.CreatePawTaskAsync(task);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _pawTaskService.GetPawTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PawTaskDTO task)
        {
            if (!ModelState.IsValid) return View(task);
            await _pawTaskService.UpdatePawTaskAsync(id, task);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _pawTaskService.GetPawTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _pawTaskService.GetPawTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pawTaskService.DeletePawTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}