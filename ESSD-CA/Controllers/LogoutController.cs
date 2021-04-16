using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ESSD_CA.Db;
using ESSD_CA.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ESSD_CA.Controllers
{
    public class LogoutController : Controller
    {
        private readonly DbESSDCA db;

        public LogoutController(DbESSDCA db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            string sessionId = Request.Cookies["sessionId"];
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);

            if (user != null)
            {
                user.SessionId = null;
                db.Users.Update(user);
                db.SaveChanges();
            }

            Response.Cookies.Delete("sessionId");
            
            ResetShopCartSettings();

            HttpContext.Session.Remove("AccountType");

            return RedirectToAction("Index", "ShopGallery");
        }

        private void ResetShopCartSettings()
        {
            //generate new guest id when log out from db
            string guestId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("guestId", guestId);

            //set shop cart to 0 again
            HttpContext.Session.SetInt32("ShoppingCartIcon", 0);
        }
    }
}
