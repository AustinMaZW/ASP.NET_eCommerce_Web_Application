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
                User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);     //Retrive all users and find the certain user. can be replaced by one code, will change it later.
                if (user == null)           //If this user is not exist in database, kick him out to other page
                {
                    return RedirectToAction("Index", "Logout");
                }

                RegisteredUserCart(user);
                return View();

            }
            else if (guestId != null)
            {
                GuestUserCart(guestId);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        public IActionResult AdditemCart([FromBody] ShoppingCart addItem)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            string guestId = HttpContext.Session.GetString("guestId");
            if (!string.IsNullOrEmpty(sessionId))
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
                    if (cartList[i].ProductId == productList[j].Id)
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
            List<ShoppingCart> add_list = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList();        // Retrieve carts to list.    
            ViewData["userItems"] = add_list;       //Deliver to viedata(to the cart page)
            List<Product> prods = db.Products.ToList();     //Retrieve all product details
            List<Product> _prods = new List<Product>();

            foreach (ShoppingCart it in add_list)
            {
                _prods.Add(prods.Find(x => x.Id == it.ProductId));          //select products which are in the cart
            }
            ViewData["addItems"] = _prods;          //deliver the cart items to cart page    //update the sessionId in viewData
            Calculation(_prods, add_list);
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
                _prods.Add(prods.Find(x => x.Id == it.ProductId));          //select products which are in the cart
            }
            ViewData["addItems"] = _prods;      // the products' detail that user added
            Calculation(_prods, _guestCart);
        }
    }
}
