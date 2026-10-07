using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{

    public class ComponentController : Controller
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService componentService, ILogger<ComponentController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _componentService.GetComponentsAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComponentDTO component)
        {
            if (!ModelState.IsValid) return View(component);
            await _componentService.CreateComponentAsync(component);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(decimal id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return View(component);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(decimal id, ComponentDTO component)
        {
            if (!ModelState.IsValid) return View(component);
            await _componentService.UpdateComponentAsync(id, component);
            return RedirectToAction(nameof(Index));
        }
        // Details action uses the component service to retrieve a component by its id.
        public async Task<IActionResult> Details(decimal id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return View(component);
        }

        public async Task<IActionResult> Delete(decimal id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return View(component);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            await _componentService.DeleteComponentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}