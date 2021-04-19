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
                ViewData["sessionId"] = sessionId;
                SetShopIconCount(sessionId);
                User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);     //Retrive all users and find the certain user.
                if (user == null)           //If this user is not exist in database, kick him out to other page
                {
                    return RedirectToAction("Index", "Logout");
                }

                RegisteredUserCart(user);   //get the registered user's cart information
                return View();

            }
            else if (guestId != null)
            {
                
                GuestUserCart(guestId);     //get the registered user's cart information
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        public IActionResult AdditemCart([FromBody] ShoppingCart addItem)   //update the cart information, including remove, add , update
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];        //get session id
            string guestId = HttpContext.Session.GetString("guestId");          //get guest id
            if (!string.IsNullOrEmpty(sessionId))                   //judge whether the session id is null or not, if not null this user is login
            {
                User user = db.Users.First(a => a.SessionId == sessionId);
                if (user == null)
                {
                    return Json(new { success = false });
                }
                if (addItem.Count <= 0)
                {
                    RemoveItems(addItem, user);
                }
                else
                {
                    UpdateItems(addItem,user);
                }
                SetShopIconCount(sessionId);
                return Json(new { success = true });
            }
            else if (guestId != null)
            {
                if (addItem.Count <= 0)
                {
                    RemoveItems(addItem, null,guestId);
                }
                else
                {
                    UpdateItems(addItem, null, guestId);
                }
                SetShopIconCount(guestId);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Remove items in Cart
        /// </summary>
        /// <param name="addItem"></param>
        /// <param name="user"></param>
        /// <param name="guestId"></param>
        private void RemoveItems(ShoppingCart addItem,User user, string guestId = null )
        {
            //for deleting the product information, use the guestid and uer id to locate the certain cart.
            ShoppingCart item = db.ShoppingCarts.FirstOrDefault(a => (user == null ? a.GuestId : a.UserId) == (user == null ? guestId : a.UserId) && a.ProductId == addItem.ProductId);
            db.ShoppingCarts.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Update item in Cart
        /// </summary>
        /// <param name="addItem"></param>
        /// <param name="user"></param>
        /// <param name="guestId"></param>
        private void UpdateItems(ShoppingCart addItem, User user, string guestId = null)
        {
            //for updating the product information, use the guestid and uer id to locate the certain cart.
            ShoppingCart item = db.ShoppingCarts.FirstOrDefault(a => (user == null ? a.GuestId:a.UserId) == (user == null ? guestId : a.UserId) && a.ProductId == addItem.ProductId);
            item.Count = addItem.Count;
            db.SaveChanges();
        }



        /// <summary>
        /// Calculate the Total Price
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="cartList"></param>
        private void Calculation(List<Product> productList, List<ShoppingCart> cartList)
        {
            double total = 0.0;
            for (int i = 0; i < cartList.Count; i++)
            {
                for (int j = 0; j < productList.Count; j++)
                {
                    if (cartList[i].ProductId == productList[j].Id && productList[j].ProductStatus == "Available")
                    {
                        total += cartList[i].Count * productList[j].UnitPrice;
                    }
                }
            }
            ViewData["TotalPrice"] = Math.Round(total,2);
        }

        /// <summary>
        /// Retrieve the cart items of Registered user
        /// </summary>
        /// <param name="user"></param>
        private void RegisteredUserCart(User user)
        {
            List<ShoppingCart> add_list = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList();        // Retrieve carts' items to list.    
            ViewData["userItems"] = add_list;       //Deliver to viewdata(to the cart page)
            List<Product> prods = db.Products.ToList();     //Retrieve all product details
            List<Product> _prods = new List<Product>();         //create a new list to save the product information that are in the cart

            foreach (ShoppingCart it in add_list)
            {
                _prods.Add(prods.FirstOrDefault(x => x.Id == it.ProductId));          //select products which are in the cart
            }
            ViewData["addItems"] = _prods;          //deliver the cart items to cart page    //update the sessionId in viewData
            Calculation(_prods, add_list);          //compute the total price
        }

        /// <summary>
        /// Retrieve the cart items of Non-Registered user
        /// </summary>
        /// <param name="guestId"></param>
        public void GuestUserCart(string guestId)
        {
            ViewData["guestId"] = guestId;
            List<ShoppingCart> _guestCart = db.ShoppingCarts.Where(a => a.GuestId == guestId).ToList();      //GusetId & Id(in the table of ShoppingCarts)
            ViewData["userItems"] = _guestCart;
            List<Product> prods = db.Products.ToList();     //Retrieve all product details
            List<Product> _prods = new List<Product>();
            foreach (ShoppingCart it in _guestCart)
            {
                _prods.Add(prods.FirstOrDefault(x => x.Id == it.ProductId));          //select products which are in the cart
            }
            ViewData["addItems"] = _prods;      // the products' detail that user added
            Calculation(_prods, _guestCart);
        }
        private void SetShopIconCount(string sessionId)
        {
            //below for setting up shop cart icon count
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId && x.SessionId != null);
            if (user != null)
            {
                List<ShoppingCart> items = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList();
                int count = 0;
                foreach (ShoppingCart item in items)
                {
                    count += item.Count;
                }
                HttpContext.Session.SetInt32("ShoppingCartIcon", count);
            }
            else
            {
                List<ShoppingCart> items = db.ShoppingCarts.Where(x => x.GuestId ==
                    HttpContext.Session.GetString("guestId")).ToList();
                int count = 0;
                foreach (ShoppingCart item in items)
                {
                    count += item.Count;
                }
                HttpContext.Session.SetInt32("ShoppingCartIcon", count);
            }
        }
    }
}
