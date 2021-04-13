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

            return View();
        }

        public IActionResult AddToCart(Product product, int count)
        {
            // add code to check if user is currently logged in through session ID

            if (product != null) // need to check if sessionid is null too
            {
                // debug script to check that correct items were selected
                Debug.WriteLine("Product Id: " + product.Id);
                Debug.WriteLine("count: " + count);

                string guestId = HttpContext.Session.GetString("guestId");
                // search database if existing cart with same id exists
                ShoppingCart cartFromDb = db.ShoppingCarts.FirstOrDefault(
                    x => x.Id == guestId && x.ProductId == product.Id);

                //calculate price
                double price = product.UnitPrice * count;
                bool guestUser = false;

                // add info to database
                ShoppingCart cartObj = new ShoppingCart
                {
                    // might need to change this to guest sessionId to reconcile all the items from same user
                    Id = HttpContext.Session.GetString("guestId"),
                    ProductId = product.Id,
                    GuestUser = guestUser,
                    Count = count
                };

                if (cartFromDb == null)
                {
                    db.ShoppingCarts.Add(cartObj);
                }
                else
                {
                    cartFromDb.Count += count;
                    db.ShoppingCarts.Update(cartFromDb);
                }

                // current issue is how to update a cart with same id 
                db.SaveChanges();
                

            }

            return RedirectToAction("Index");
        }
    }
}
