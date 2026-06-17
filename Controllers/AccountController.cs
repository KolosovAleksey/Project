using Microsoft.AspNetCore.Mvc;
using EventPortal.Models;

namespace EventPortal.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        // Для простоты POST-методы не реализованы, это только заглушки
    }
}
