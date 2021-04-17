using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ESSD_CA.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbESSDCA db;
        public ProductController(DbESSDCA db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            List<Product> products = db.Products.OrderBy(s => s.ProductName).OrderBy(s => s.ProductStatus).ToList();  //retrieving products from database and putting into a list

            ViewData["Is_ProductMgmt"] = "bold_menu";
            ViewData["products"] = products;    //sending data view ViewData
            return View();
        }

        public IActionResult Create()
        {
            ViewData["Is_ProductMgmt"] = "bold_menu";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,ProductDescription,UnitPrice,DownloadLink,ImagePath")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Is_ProductMgmt"] = "bold_menu";
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ProductName,ProductDescription,UnitPrice,DownloadLink,ImagePath,ProductStatus")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        private bool ProductExists(string id)
        {
            return db.Products.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewData["Is_ProductMgmt"] = "bold_menu";
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, [Bind("Id,ProductName,ProductDescription,UnitPrice,DownloadLink,ImagePath,ProductStatus")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Product");
            }
            return View(product);
        }
    }
}
