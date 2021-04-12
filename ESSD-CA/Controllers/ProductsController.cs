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
    public class ProductsController : Controller
    {
        private readonly DbESSDCA db;

        public ProductsController(DbESSDCA db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            
            var products = from product in db.Products select product;  //retrieving products from database and putting into a list
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => (s.ProductName.Contains(searchString) | s.ProductDescription.Contains(searchString)));
            }
            
            List<Product> productList = products.ToList();
            ViewData["productList"] = productList;
            return View(await products.ToListAsync());
        }

    }
}
