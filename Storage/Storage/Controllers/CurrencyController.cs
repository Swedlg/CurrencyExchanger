using Microsoft.AspNetCore.Mvc;

namespace Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
