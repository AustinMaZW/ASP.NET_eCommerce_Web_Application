using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authenticate (string username, string password)
        {
            
            
            if (username == null)
            {
                ViewData["username"] = username;
                ViewData["errLogin"] = "Please enter valid username and password.";

                return View("Index");
            }
            else
            {

                //user???.SessionId = Guid.NewGuid().ToString();
                //Response.Cookies.Append("sessionId", user???.SessionId);

                return RedirectToAction("Index", "Home");
            }

        }
    }
}
