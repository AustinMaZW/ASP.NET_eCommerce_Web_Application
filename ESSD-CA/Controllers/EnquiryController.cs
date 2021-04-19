using ESSD_CA.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESSD_CA.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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
            ViewData["Is_ContactUs"] = "bold_menu";
            return View();
        }
        public IActionResult SendMessage(string name, string email, string phone, string message)
        {
            CustomerMessage newMessage = new CustomerMessage
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                PhoneNumber = phone,
                Message = message,
                MessageDate = DateTime.Now,
                EnquiryStatus = false
            };
            db.CustomerMessages.Add(newMessage);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Manage()
        {
            if (HttpContext.Session.GetString("AccountType") != "Admin")
            {
                return RedirectToAction("Index");
            }
            ViewData["Is_ContactMgmt"] = "bold_menu";
            ViewData["CustomerMessages"] = db.CustomerMessages.OrderBy(c=>c.MessageDate).ToList();

            return View();
        }

        public JsonResult Archive([FromBody] MessageStatus message)
        {
            CustomerMessage customerMessage =  db.CustomerMessages.FirstOrDefault(x => x.Id == message.Id);
            customerMessage.EnquiryStatus = message.EnquiryStatus;
            db.Update(customerMessage);
            db.SaveChanges();

            return Json(new { success = true });
        }
    }
}
