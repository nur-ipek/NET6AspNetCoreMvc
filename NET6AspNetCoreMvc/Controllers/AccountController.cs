using Microsoft.AspNetCore.Mvc;
using NET6AspNetCoreMvc.Entities;
using NET6AspNetCoreMvc.Models;
using NETCore.Encrypt.Extensions;

namespace NET6AspNetCoreMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration; 
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
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
                var md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                var saltedPassword = registerViewModel.Password + md5Salt;
                var hashedPassword = saltedPassword.MD5();

                User user = new User()
                {
                    UserName = registerViewModel.Username,
                    Password = hashedPassword
                };

                _databaseContext.Users.Add(user);
                int affectedrows = _databaseContext.SaveChanges();

                if (affectedrows == 0)
                {
                    ModelState.AddModelError("", "Kullanıcı kayıt edilemedi.");

                }
                else
                {
                    return RedirectToAction(nameof(Login));

                }

            }
            return View(registerViewModel);
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
