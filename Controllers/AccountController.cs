using Microsoft.AspNetCore.Mvc;

namespace ProjetoCarequinha.Controllers
{
    public class AccountController : Controller
    {
        private const string ADMIN_USER = "admin";
        private const string ADMIN_PASS = "1234";

        public IActionResult Login()
        {
            // Se já for admin → pula login e vai direto ao CRUD
            if (HttpContext.Session.GetString("IsAdmin") == "true")
                return RedirectToAction("Index", "VideoAulaAdmin");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == ADMIN_USER && password == ADMIN_PASS)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "VideoAulaAdmin");
            }

            ViewBag.Error = "Usuário ou senha inválidos!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }
    }
}
