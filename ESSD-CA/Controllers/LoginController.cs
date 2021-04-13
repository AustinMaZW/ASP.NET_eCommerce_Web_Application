using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbESSDCA db;
        

        public LoginController(DbESSDCA db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            string sessionId = Request.Cookies["sessionId"];
            
            if(sessionId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Authenticate (string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username);
            string sessionId = Request.Cookies["sessionId"];
            if (sessionId != null)
                return RedirectToAction("Index", "Home");
                            
            if (user != null)
            {
                if (user.Password == password)
                {
                    user.SessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("sessionId", user.SessionId);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["username"] = username;
                    ViewData["errLogin"] = "Please enter valid username or password.";
                    return View("Index");
                }
            }

            else
            {
                ViewData["username"] = username;
                ViewData["errLogin"] = "Please enter valid username and password.";

                return View("Index");
            }
        }

    }
}
