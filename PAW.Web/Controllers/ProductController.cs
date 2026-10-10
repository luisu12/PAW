using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetProductsAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            if (!ModelState.IsValid) return View(product);
            await _productService.CreateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductDTO product)
        {
            // Log raw form values to diagnose binding issues
            try
            {
                _logger.LogDebug("Request.Form contents for Product Edit:");
                foreach (var key in Request.Form.Keys)
                {
                    _logger.LogDebug($"Form[{key}] = '{Request.Form[key]}'");
                }

                // Log ModelState entries and errors
                foreach (var entry in ModelState)
                {
                    var errors = entry.Value.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var err in errors)
                        {
                            _logger.LogWarning($"ModelState error for '{entry.Key}': {err.ErrorMessage} {err.Exception}");
                        }
                    }
                    else
                    {
                        _logger.LogDebug($"ModelState OK for '{entry.Key}' Value='{entry.Value?.AttemptedValue}'");
                    }
                }

                _logger.LogDebug($"Product Edit received (model): ProductId='{product?.ProductId}', Name='{product?.Name}', Description='{product?.Description}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging form/model state for Product Edit");
            }

            if (!ModelState.IsValid) return View(product);
            await _productService.UpdateProductAsync(id, product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
