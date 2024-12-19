using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.FrontExmple2.Controllers;

public class SuccessController: Controller
{
    public ActionResult Index()
    {
        return View();
    }
}