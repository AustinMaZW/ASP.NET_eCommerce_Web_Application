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
        public IActionResult Index()
        {
            List<Product> products = db.Products.ToList();  //retrieving products from database and putting into a list

            ViewData["products"] = products;    //sending data view ViewData
            
            if (HttpContext.Session.GetString("guestId") == null)
            {
                string guestId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("guestId", guestId);
            }

            ViewData["sessionId"] = HttpContext.Request.Cookies["sessionId"];

            if (HttpContext.Session.GetString("guestId") == null)
            {
                string guestId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("guestId", guestId);
            }

            return View();
        }

        public IActionResult AddToCart(Product product, int count)
        {
            if (product == null) { return RedirectToAction("Index"); }

            string sessionId = Request.Cookies["sessionId"];
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId && x.SessionId != null);

            string guestId = HttpContext.Session.GetString("guestId");

            Debug.WriteLine("Product Id: " + product.Id);               //just for debug purposes, can delete
            Debug.WriteLine("count: " + count);                         //just for debug purposes, can delete

            if (user != null)
            {
                UserAddToCart(product, count, guestId, user);
            }
            else
            {
                GuestAddToCart(product, count, guestId);
            }

            return RedirectToAction("Index");
        }

        private void GuestAddToCart(Product product, int count, string guestId)
        {
            if (product == null) { return; }

            // search database if existing cart with same id exists
            ShoppingCart cartFromDb = db.ShoppingCarts.FirstOrDefault(
                x => x.GuestId == guestId && x.ProductId == product.Id);

            ShoppingCart cartObj = new ShoppingCart
            {
                Id = Guid.NewGuid().ToString(),
                GuestId = guestId,
                ProductId = product.Id,
                Count = count
            };

            if (cartFromDb == null)             // if guest never had a cart before
            {
                db.ShoppingCarts.Add(cartObj);
            }
            else                                // if guest had a cart before, then merge
            {
                cartFromDb.Count += count;
                db.ShoppingCarts.Update(cartFromDb);
            }
            db.SaveChanges();
        }

        private void UserAddToCart(Product product, int count, string guestId, User user)
        {
            if (product == null || user == null) { return; }

            ShoppingCart userCartFromDb = db.ShoppingCarts.FirstOrDefault(
            x => x.UserId == user.UserId && x.ProductId == product.Id);

            ShoppingCart cartObj = new ShoppingCart
            {
                Id = Guid.NewGuid().ToString(),
                GuestId = null,
                ProductId = product.Id,
                UserId = user.UserId,
                Count = count
            };
            if (userCartFromDb != null)             // merge if user had previous logged in and add to cart with same product
            {
                userCartFromDb.Count += count;
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
