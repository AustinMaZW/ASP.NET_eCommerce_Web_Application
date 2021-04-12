using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ESSD_CA.Models;
using ESSD_CA.Db;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly DbESSDCA db;

        public IActionResult History()
        {
            // retrieve all purchased items by customer id from db
            // 
            return View();
        }

        public void AddPODetail(string orderId, string productId)
        {
            db.PODetails.Add(new PurchaseOrderDetails
            {
                ActivationCode = GenerateActivationCode(),
                ProductId = productId,
                OrderId = orderId
            });

            db.SaveChanges();
            
        }


        private string GenerateActivationCode()
        {
            return (Guid.NewGuid().ToString());
        }
    }
}
