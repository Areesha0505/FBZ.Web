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
        public IActionResult Login(User loginUser)
        {
            var user = users.FirstOrDefault(u =>
                u.Username == loginUser.Username &&
                u.Password == loginUser.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("role", user.Role);

                return RedirectToAction("Index", "Comic");
            }

            ViewBag.Error = "Invalid login";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Comic");
        }
    }
}