using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{

    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;
        private readonly IProductService _productService;
        public InventoryController(IInventoryService inventoryService, IProductService productService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _inventoryService.GetInventoriesAsync();
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            // populate products list for dropdown and await population before rendering
            await PopulateProductsAsync();
            return View();
        }

        // Populate products list for dropdown using injected IProductService
        private async Task PopulateProductsAsync()
        {
            try
            {
                var (items, _) = await _productService.GetProductsAsync();
                var list = items ?? System.Linq.Enumerable.Empty<ProductDTO>();
                ViewBag.Products = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list, "ProductId", "Name");
                return;
            }
            catch
            {
                // ignored - fallback to empty list
            }

            ViewBag.Products = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(System.Linq.Enumerable.Empty<ProductDTO>(), "ProductId", "Name");
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryDTO inventory)
        {
            if (!ModelState.IsValid) return View(inventory);
            await _inventoryService.CreateInventoryAsync(inventory);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            await PopulateProductsAsync();
            return View(inventory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, InventoryDTO inventory)
        {
            if (!ModelState.IsValid) return View(inventory);
            await _inventoryService.UpdateInventoryAsync(id, inventory);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}