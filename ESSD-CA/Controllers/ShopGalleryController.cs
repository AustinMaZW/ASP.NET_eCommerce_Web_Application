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
            if (product != null )
            {
                Debug.WriteLine("Product Id: " + product.Id);
                Debug.WriteLine("count: " + count);
            }

            return RedirectToAction("Index");
        }
    }
}
