using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult History()
        {
            // retrieve all purchased items by customer id from db
            // 
            return View();
        }

        public void GenerateActivationCode()
        {

        }
    }
}
