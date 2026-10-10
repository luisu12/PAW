using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{

    public class UserActionController : Controller
    {
        private readonly IUserActionService _userActionService;
        private readonly ILogger<UserActionController> _logger;

        public UserActionController(IUserActionService userActionService, ILogger<UserActionController> logger)
        {
            _userActionService = userActionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userActionService.GetUserActionsAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            // Provide an initialized DTO so the hidden Id field renders a default numeric value (0)
            return View(new UserActionDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserActionDTO action)
        {
            // Log raw form values to diagnose binding issues
            try
            {
                _logger.LogDebug("Request.Form contents for UserAction Create:");
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

                _logger.LogDebug($"UserAction Create received (model): Name='{action?.Name}', Description='{action?.Description}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging form/model state for UserAction Create");
            }

            if (!ModelState.IsValid) return View(action);

            await _userActionService.CreateUserActionAsync(action);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(decimal id)
        {
            var action = await _userActionService.GetUserActionByIdAsync(id);
            if (action == null) return NotFound();
            return View(action);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(decimal id, UserActionDTO action)
        {
            if (!ModelState.IsValid) return View(action);
            await _userActionService.UpdateUserActionAsync(id, action);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(decimal id)
        {
            var action = await _userActionService.GetUserActionByIdAsync(id);
            if (action == null) return NotFound();
            return View(action);
        }

        public async Task<IActionResult> Delete(decimal id)
        {
            var action = await _userActionService.GetUserActionByIdAsync(id);
            if (action == null) return NotFound();
            return View(action);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            await _userActionService.DeleteUserActionAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}