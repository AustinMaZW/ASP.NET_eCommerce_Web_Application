using ESSD_CA.Db;
using ESSD_CA.Models;
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

                //calculate price
                double price = product.UnitPrice * count;
                bool guestUser = false;

                
                // add info to database
                ShoppingCart cartObj = new ShoppingCart
                {
                    Id = Guid.NewGuid().ToString(), // might need to change this to guest sessionId to reconcile all the items from same user
                    ProductId = product.Id,
                    GuestUser = guestUser,
                    Count = count
                };

                db.ShoppingCarts.Add(cartObj);
                db.SaveChanges();
                

            }

            return RedirectToAction("Index");
        }
    }
}
