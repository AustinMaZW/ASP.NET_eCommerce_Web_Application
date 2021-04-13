using ESSD_CA.Db;
using ESSD_CA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DbESSDCA db;

        public ShoppingCartController(DbESSDCA db)
        {
            this.db = db;
        }


        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            string guestId = HttpContext.Session.GetString("guestId");
            if (sessionId != null)  //Is it a registered user? Not null means he is a registered user.
            {
                User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);     //Retrive all users and find the certain user. can be replaced by one code, will change it later.
                if (user == null)           //If this user is not exist in database, kick him out to other page
                {
                    return RedirectToAction("Index", "Logout");
                }

                List<ShoppingCart> add_list = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList();        // Retrieve carts to list.    
                ViewData["userItems"] = add_list;       //Deliver to viedata(to the cart page)

                List<Product> prods = db.Products.ToList();     //Retrieve all product details
                List<Product> _prods = new List<Product>();
                foreach (ShoppingCart it in add_list)
                {
                    _prods.Add(prods.Find(x => x.Id == it.ProductId));          //select products which are in the cart
                }

                ViewData["addItems"] = _prods;          //deliver the cart items to cart page
                ViewData["sessionId"] = sessionId;      //update the sessionId in viewData

                return View();

            }
            else if (guestId != null)
            {
                List<ShoppingCart> _guestCart = db.ShoppingCarts.Where(a => a.Id == guestId).ToList();      //GusetId & Id(in the table of ShoppingCarts)
                ViewData["userItems"] = _guestCart;
                List<Product> prods = db.Products.ToList();     //Retrieve all product details
                List<Product> _prods = new List<Product>();
                foreach (ShoppingCart it in _guestCart)
                {
                    _prods.Add(prods.Find(x => x.Id == it.ProductId));          //select products which are in the cart
                }
                ViewData["addItems"] = _prods;      // the products' detail that user added
                ViewData["guestId"] = guestId;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        public IActionResult AdditemCart([FromBody] ShoppingCart addItem)
        {
            string sessionId = Request.Cookies["sessionId"];
            string guestId = HttpContext.Session.GetString("guestId");
            if (!string.IsNullOrEmpty(sessionId))
            {
                User user = db.Users.First(a => a.SessionId == sessionId);
                if (user == null)
                {
                    return Json(new { success = false });
                }
                if (addItem.Count == 0)
                {
                    ShoppingCart item = db.ShoppingCarts.First(a => a.UserId == user.UserId && a.ProductId == addItem.ProductId);
                    db.ShoppingCarts.Remove(item);
                    db.SaveChanges();
                }
                else
                {
                    ShoppingCart item = db.ShoppingCarts.First(a => a.UserId == user.UserId && a.ProductId == addItem.ProductId);
                    item.Count = addItem.Count;
                    db.SaveChanges();
                }                
                return Json(new { success = true });
            }
            else if (guestId != null)
            {
                var _list_itemadd = from i in db.ShoppingCarts
                                    where i.Id == guestId && i.ProductId == addItem.ProductId
                                    select i;
                if (addItem.Count == 0)
                {
                    ShoppingCart item = db.ShoppingCarts.First(a => a.Id == guestId && a.ProductId == addItem.ProductId);
                    db.ShoppingCarts.Remove(item);
                    db.SaveChanges();
                }
                else
                {
                    ShoppingCart item = db.ShoppingCarts.First(a => a.Id == guestId && a.ProductId == addItem.ProductId);
                    item.Count = addItem.Count;
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}






//below


















//below










//Hi Austin and CK:

//1.
//I think there is a conflict in your code, Austin used "Guest ID" to mark both the guests' and users' carts. CustomerID will be always Null.
//But in CK's code, he only saved the "Session ID" in the user table. 
//Thus, the cart information cannot be retrieved by CustomerID(User ID) for registered users.
//For current situation, I cannot retrieved any data from the shopping cart table after login.Do you think we can revise this ?

//2.
//I saw the customerId were generated by GUID, it means that every time when we launch the program without commenting out the database creation code in startup file, the customerId will be changed.
//Then, the customerId in ShoppingCart also needs to follow the changes of customerId in the Users table.So I think maybe we need to comment out the database creation code ?

//3.
//For the cart icon, when users login, shoppingGallery page cannot see the cart icon, because it is depending on ViewData["sessionId"], but each ViewData is only available for its respective page.
//When users go to a new page, ViewData will be empty unless it is assigned in the corresponding controller(like index()).Maybe add "ViewData["sessionId"] = HttpContext.Request.Cookies["sessionId"]" will allow registered user see the icon.

