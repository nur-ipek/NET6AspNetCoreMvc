using Microsoft.AspNetCore.Mvc;
using NET6AspNetCoreMvc.Models;

namespace NET6AspNetCoreMvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult  Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult  Login(LoginViewModel loginViewModel)
        {
            //ModelState controller seviyesinde gelmektedir.
            if (ModelState.IsValid)
            {

            }
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View(registerViewModel);
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
