﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ESSD_CA.Db;
using ESSD_CA.Models;
using System.Threading.Tasks;

namespace ESSD_CA.Controllers
{
    public class LogoutController : Controller
    {
        private readonly DbESSDCA db;

        public LogoutController(DbESSDCA db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            string sessionId = Request.Cookies["sessionId"];
            User user = db.Users.FirstOrDefault(x => x.SessionId == sessionId);

            if (user != null)
                user.SessionId = null;

            Response.Cookies.Delete("sessionId");

            return RedirectToAction("Index", "Home");
        }
    }
}
