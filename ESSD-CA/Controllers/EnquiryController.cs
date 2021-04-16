using ESSD_CA.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESSD_CA.Models;
using System.Diagnostics;

namespace ESSD_CA.Controllers
{
    public class EnquiryController : Controller
    {
        private readonly DbESSDCA db;

        public EnquiryController(DbESSDCA db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult SendMessage(string name, string email, string phone, string message)
        {
            Debug.WriteLine("Hello");
            CustomerMessage newMessage = new CustomerMessage
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                PhoneNumber = phone,
                Message = message
            };
            db.CustomerMessages.Add(newMessage);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
