using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("home")]
public class HomeController : Controller
{
    public IActionResult Index() => View("Index");
}