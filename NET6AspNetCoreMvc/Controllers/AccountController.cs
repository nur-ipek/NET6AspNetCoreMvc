using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET6AspNetCoreMvc.Entities;
using NET6AspNetCoreMvc.Models;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;

namespace NET6AspNetCoreMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            //ModelState controller seviyesinde gelmektedir.
            if (ModelState.IsValid)
            {
                var md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                var saltedPassword = loginViewModel.Password + md5Salt;
                var hashedPassword = saltedPassword.MD5();

                User user = _databaseContext.Users.FirstOrDefault(x => x.Password == hashedPassword && x.UserName.ToLower() == loginViewModel.Username.ToLower());

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(loginViewModel.Username), "Kullanıcı kilitli.");
                        return View(loginViewModel);
                    }

                    //Cookie
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.NameSurname ?? string.Empty));
                    claims.Add(new Claim("Username", user.UserName));

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");

                }

                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı ya da şifre yanlış");
                }
            }
            return View(loginViewModel);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.UserName.ToLower() == registerViewModel.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(registerViewModel.Username), "Bu kullanıcı daha önceden alınmış.");
                }

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

        public IActionResult Logout() 
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));    
        }
    }
}
