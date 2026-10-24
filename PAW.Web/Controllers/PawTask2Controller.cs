using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class PawTask2Controller(IPawTask2Service pawTask2Service) : Controller
{
    public async Task<IActionResult> Index(int page = 1, CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        const int pageSize = 25;
        var (items, total) = await pawTask2Service.GetPawTask2ItemsAsync(page, pageSize, ct);
        ViewBag.TotalItems = total;
        ViewBag.PageSize = pageSize;
        ViewBag.CurrentPage = page;
        return View(items);
    }

    public IActionResult Create()
    {
        return View(new PawTask2DTO());
    }

    [HttpPost]
    public IActionResult Create(PawTask2DTO item)
    {
        TempData["PawTask2ActionMessage"] = "Data coming from API, action is not possible.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await pawTask2Service.GetPawTask2ItemByIdAsync(id, ct);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    public IActionResult Edit(int id, PawTask2DTO item)
    {
        TempData["PawTask2ActionMessage"] = "Data coming from API, action is not possible.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var item = await pawTask2Service.GetPawTask2ItemByIdAsync(id, ct);
        if (item == null) return NotFound();
        return View(item);
    }

    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await pawTask2Service.GetPawTask2ItemByIdAsync(id, ct);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        TempData["PawTask2ActionMessage"] = "Data coming from API, action is not possible.";
        return RedirectToAction(nameof(Index));
    }
}
