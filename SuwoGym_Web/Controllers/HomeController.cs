using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SuwoGym_Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
