using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESSD_CA.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                return RedirectToAction("Index", "ShopGallery");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index (string username, string password)
        {
            string sessionId = Request.Cookies["sessionId"];
            if (sessionId != null)
                return RedirectToAction("Index", "ShopGallery");

            User user = db.Users.FirstOrDefault(x => x.Username == username /*&& x.Password == hashPassword*/);

            using (MD5 md5Hash = MD5.Create())
            {
                string hashPassword = MD5Hash.Md5hash(md5Hash, password);

                if (user == null || user.Password != hashPassword)
                {
                    ViewData["username"] = username;
                    ViewData["errLogin"] = "Please enter valid username and password.";

                    return View("Index");
                }

                else
                {
                    bool isThereGuestCart = CheckForGuestCart(user);
                    UpdateCartIcon(user);

                    user.SessionId = Guid.NewGuid().ToString();
                    db.Users.Update(user);
                    db.SaveChanges();

                    SetSessionData(user);

                    if (user.AccountType.Equals("Admin"))
                        return RedirectToAction("Index", "Product");

                    if (isThereGuestCart)
                        return RedirectToAction("Index", "ShoppingCart");

                    return RedirectToAction("Index", "ShopGallery");
                }
            }
        }

        private void SetSessionData(User user)
        {
            Response.Cookies.Append("sessionId", user.SessionId);
            HttpContext.Session.SetString("uname", user.Username);
            HttpContext.Session.SetString("AccountType", user.AccountType);     // to save account type for navigation and validation of admin
        }

        private bool CheckForGuestCart(User user)
        {
            // check if a guest shopping cart exists that matches user session
            string guestId = HttpContext.Session.GetString("guestId");
            List<ShoppingCart> guestCartObjs = db.ShoppingCarts.Where(x => x.GuestId
                == guestId).ToList();

            if (guestCartObjs.Count > 0)
            {
                foreach (ShoppingCart cartObj in guestCartObjs)
                {
                    // check if previous userid and product id exists
                    // if same one then merge guest with user and delete guest

                    ShoppingCart UserCartObj = db.ShoppingCarts.FirstOrDefault(x =>
                        x.UserId == user.UserId && x.ProductId == cartObj.ProductId);

                    if (UserCartObj != null)
                    {
                        UserCartObj.Count += cartObj.Count;
                        db.ShoppingCarts.Update(UserCartObj);
                        db.ShoppingCarts.Remove(cartObj);
                    }
                    else
                    {
                        cartObj.UserId = user.UserId;
                        cartObj.GuestId = null;
                        db.ShoppingCarts.Update(cartObj);
                    }
                    db.SaveChanges();
                }

                return true;
            }
            return false;
        }

        private void UpdateCartIcon(User user)
        {
            //below code to show user shopping cart icon count
            int count = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList().Count();
            HttpContext.Session.SetInt32("ShoppingCartIcon", count);
        }
    }
}
