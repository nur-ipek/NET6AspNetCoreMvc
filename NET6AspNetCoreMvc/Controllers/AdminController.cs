using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NET6AspNetCoreMvc.Controllers
{
    /*[Authorize]*/ //Login olup olmama durumu.
    //[Authorize(Roles ="admin,manager")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
