using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.FrontExmple2.Controllers;

public sealed class InventoryController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}