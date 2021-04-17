using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class ShopGalleryController : Controller
    {
        private readonly DbESSDCA db;

        public ShopGalleryController(DbESSDCA db)
        {
            this.db = db;
        }
        public IActionResult Index(string searchString)
        {
            ViewData["Is_Gallery"] = "bold_menu";
            ViewData["Products"] = db.Products.Where(s => 
                (s.ProductName.Contains(searchString) || s.ProductDescription.Contains(searchString)) || searchString == null).OrderBy(s => s.ProductName).ToList();

            ViewData["searchString"] = searchString;

            if (HttpContext.Session.GetString("guestId") == null)       // generate guestId when visit shop gallery
            {
                string guestId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("guestId", guestId);
            }

            string sessionId = Request.Cookies["sessionId"];

            SetShopIconCount(sessionId);

            return View();
        }

        private void SetShopIconCount(string sessionId)
        {
            //below for setting up shop cart icon count
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId && x.SessionId != null);
            if (user != null)
            {
                int count = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList().Count();
                HttpContext.Session.SetInt32("ShoppingCartIcon", count);
            }
            else
            {
                int count = db.ShoppingCarts.Where(x => x.GuestId ==
                    HttpContext.Session.GetString("guestId")).ToList().Count();
                HttpContext.Session.SetInt32("ShoppingCartIcon", count);
            }
        }

        public IActionResult AddToCart([FromBody] ShoppingCart addItem)
        {
            if (addItem == null || addItem.Count <= 0) { return RedirectToAction("Index"); }

            Debug.WriteLine("Product Id: " + addItem.ProductId);               //just for debug purposes, can delete
            Debug.WriteLine("count: " + addItem.Count);

            string sessionId = Request.Cookies["sessionId"];
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId && x.SessionId != null);

            string guestId = HttpContext.Session.GetString("guestId");

            if (user != null)
            {
                UserAddToCart(addItem, guestId, user);

            }
            else
            {
                GuestAddToCart(addItem, guestId);

            }
            return Json(new { success = true });

        }

        private void GuestAddToCart(ShoppingCart addItem, string guestId)
        {
            if (addItem == null) { return; }

            // search database if existing cart with same id exists
            ShoppingCart cartFromDb = db.ShoppingCarts.FirstOrDefault(
                x => x.GuestId == guestId && x.ProductId == addItem.ProductId);

            ShoppingCart cartObj = new ShoppingCart
            {
                Id = Guid.NewGuid().ToString(),
                GuestId = guestId,
                ProductId = addItem.ProductId,
                Count = addItem.Count
            };

            if (cartFromDb == null)             // if guest never had a cart before
            {
                db.ShoppingCarts.Add(cartObj);
            }
            else                                // if guest had a cart before, then merge
            {
                cartFromDb.Count += addItem.Count;
                db.ShoppingCarts.Update(cartFromDb);
            }
            db.SaveChanges();
        }

        private void UserAddToCart(ShoppingCart addItem, string guestId, User user)
        {
            if (addItem == null || user == null) { return; }

            ShoppingCart userCartFromDb = db.ShoppingCarts.FirstOrDefault(
            x => x.UserId == user.UserId && x.ProductId == addItem.ProductId);

            ShoppingCart cartObj = new ShoppingCart
            {
                Id = Guid.NewGuid().ToString(),
                GuestId = null,
                ProductId = addItem.ProductId,
                UserId = user.UserId,
                Count = addItem.Count
            };
            if (userCartFromDb != null)             // merge if user had previous logged in and add to cart with same product
            {
                userCartFromDb.Count += addItem.Count;
                db.ShoppingCarts.Update(userCartFromDb);
            }

            else                                    // add new row 
            {
                db.ShoppingCarts.Add(cartObj);
            }
            db.SaveChanges();
        }
    }
}
