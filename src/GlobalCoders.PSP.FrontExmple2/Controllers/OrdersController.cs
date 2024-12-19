using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.FrontExmple2.Controllers;

public class OrdersController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}