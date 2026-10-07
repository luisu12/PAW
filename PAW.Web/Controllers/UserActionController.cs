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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserActionDTO action)
        {
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