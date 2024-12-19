using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.FrontExmple2.Controllers;

[Route("reset-password")]
public class ResetPasswordController: Controller
{
    [Route("/reset-password/")]
    public ActionResult Index(string email, string resetCode)
    {
        ViewBag.Email = email;
        ViewBag.ResetCode = resetCode;
        return View();
    }
}