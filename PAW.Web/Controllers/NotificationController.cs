using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Web.Models;
using PAW.Web.Services;
using PAW.Models.DTO;

namespace PAW.Web.Controllers
{

    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _notificationService.GetNotificationsAsync();
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            // populate users for dropdown and await before rendering
            await PopulateUsersAsync();
            return View();
        }

        private async Task PopulateUsersAsync()
        {
            try
            {
                var userService = HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                if (userService != null)
                {
                    var list = await userService.GetUsersAsync();
                    ViewBag.Users = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list, "UserID", "Username");
                }
                else
                {
                    ViewBag.Users = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enumerable.Empty<object>());
                }
            }
            catch
            {
                ViewBag.Users = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enumerable.Empty<object>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(NotificationDTO notification)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUsersAsync();
                return View(notification);
            }

            await _notificationService.CreateNotificationAsync(notification);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return View(notification);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NotificationDTO notification)
        {
            if (!ModelState.IsValid) return View(notification);
            await _notificationService.UpdateNotificationAsync(id, notification);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return View(notification);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return View(notification);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}