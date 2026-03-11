using Microsoft.AspNetCore.Mvc;
using FBZ.Web.Models;

namespace FBZ.Web.Controllers
{
    public class AccountController : Controller
    {
        static List<User> users = new List<User>();

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            users.Add(user);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            HttpContext.Session.SetString("User", username);

            if (username == "staff")
            {
                HttpContext.Session.SetString("Role", "Staff");
            }
            else
            {
                HttpContext.Session.SetString("Role", "User");
            }

            return RedirectToAction("Index", "Comic");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Comic");
        }
    }
}