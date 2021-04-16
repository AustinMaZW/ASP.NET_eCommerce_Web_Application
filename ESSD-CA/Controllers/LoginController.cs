using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Http;
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
                return RedirectToAction("Index", "ShopGallery");
            }
            return View();
        }

        //Test out only (TO BE DELETED)
        //public IActionResult ValidateSession()
        //{
        //    string sessionId = Request.Cookies["sessionId"];
        //    if (sessionId != null)
        //    {
        //        return Redirect(Request.Headers["Referer"].ToString());
        //    }
        //    return View("Index", "Login");
        //}


        [HttpPost]
        public IActionResult Index (string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username);
            string sessionId = Request.Cookies["sessionId"];
            if (sessionId != null)
                return RedirectToAction("Index", "ShopGallery");
                            
            if (user != null)
            {
                if (user.Password == password)
                {
                    CheckForGuestCart(user);
                    UpdateCartIcon(user);

                    user.SessionId = Guid.NewGuid().ToString();
                    db.Users.Update(user);
                    db.SaveChanges();
                    Response.Cookies.Append("sessionId", user.SessionId);
                    Response.Cookies.Append("userId", user.UserId);
                    Response.Cookies.Append("username", user.Username);
                    return RedirectToAction("Index", "ShopGallery");
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

        private void CheckForGuestCart(User user)
        {
            // check if a guest shopping cart exists that matches user session
            string guestId = HttpContext.Session.GetString("guestId");
            List<ShoppingCart> guestCartObjs = db.ShoppingCarts.Where(x => x.GuestId
                == guestId).ToList();

            if (guestCartObjs != null)
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
            }
        }

        private void UpdateCartIcon(User user)
        {
            //below code to show user shopping cart icon count
            int count = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList().Count();
            HttpContext.Session.SetInt32("ShoppingCartIcon", count);
        }
    }
}
