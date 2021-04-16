using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ESSD_CA.Models;
using ESSD_CA.Db;
using Microsoft.AspNetCore.Http;

namespace ESSD_CA.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly DbESSDCA db;

        public PurchaseController(DbESSDCA db)
        {
            this.db = db;
        }

        public IActionResult History()
        {
            ViewData["Is_PurchaseHistory"] = "bold_menu";
            // Check logged in?
            string sessionId = Request.Cookies["sessionId"];
            if (String.IsNullOrEmpty(sessionId))
                return RedirectToAction("Index", "Login");
            
            // retrieve all purchased items by user id from db
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);
            
            List<PurchaseOrder> userOrders = null;
            if (user != null)
                userOrders  = db.PurchaseOrders.Where(x => x.UserId == user.UserId).ToList();

            IEnumerable<HistoryViewModel> iter = null;
            if (userOrders != null && userOrders.Count > 0)
            {
                ViewData["hasHistory"] = true;

                List<PurchaseOrderDetails> poDetails = userOrders[0].PODetails.ToList();
                
                for (int i =0; i<userOrders.Count-1; i++)
                { 
                    poDetails.AddRange(userOrders[i+1].PODetails.ToList());
                }
            

                iter = 
                    from pod in poDetails
                    group pod by new 
                    { pod.OrderId,
                      pod.ProductId
                    }
                    into g
                    select new HistoryViewModel
                    {
                        Order = g.Select(x => x.Order).First(),
                        Product = g.Select(x => x.Product).First(),
                        ActivationCdList = g.Select(x => x.ActivationCode).ToList(),
                    };
            }
            else
            {
                ViewData["hasHistory"] = false;
            }

            ViewData["sessionId"] = sessionId;

            return View(iter);
        }

        private void AddPO(double totalPrice, string orderid, string userid)
        {           
            db.PurchaseOrders.Add(new PurchaseOrder
            {
                OrderId = orderid,
                PurchaseDate = DateTime.Now.ToUniversalTime(),
                GrandTotal = totalPrice,
                UserId = userid,
            });
        }
        private void AddPODetail(string orderId, string productId)
        {
            db.PODetails.Add(new PurchaseOrderDetails
            {
                ActivationCode = GenerateActivationCode(),
                ProductId = productId,
                OrderId = orderId
            });
        }


        private string GenerateActivationCode()
        {
            return (Guid.NewGuid().ToString() ); //additional logic can be added to return string 
        }

        private double PriceCalculation(List<ShoppingCart> shoppingCart)
        {
            double totalPrice = 0;
            foreach (var sc in shoppingCart)
            {
                Product product = db.Products.FirstOrDefault(x => x.Id == sc.Product.Id);
                totalPrice += product.UnitPrice * sc.Count;
            }
            return totalPrice;
        }

        private double DiscountAmt(/*future argument to be added*/)
        {
            //future discount logic here

            double discountAmt = 0;
            return discountAmt;
        }

        public IActionResult Checkout()
        {
            string sessionId = Request.Cookies["sessionId"];

            if (String.IsNullOrEmpty(sessionId))
                return RedirectToAction("Index", "Login");
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);
            List<ShoppingCart> shoppingCart = db.ShoppingCarts.Where(x => x.UserId == user.UserId).ToList();

            if (shoppingCart == null)
                return RedirectToAction("Product", "Index"); // divert empty shopping cart back to product page.

            double totalPrice = PriceCalculation(shoppingCart) - DiscountAmt(/*future argument to be passed*/);
            string orderid = Guid.NewGuid().ToString();
            AddPO(totalPrice, orderid, user.UserId);

            foreach (var sc in shoppingCart)
            {
                for (int i = 0; i < sc.Count; i++)
                {
                    AddPODetail(orderid, sc.ProductId);
                }
                db.SaveChanges();
            }

            foreach (ShoppingCart sc in shoppingCart)
                db.ShoppingCarts.Remove(sc);

            db.SaveChanges();

            SetShopIconCount(sessionId);

            return RedirectToAction("History", "Purchase");
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
    }
}
