﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactPanel.Controllers
{
    public class FactsController : Controller
    {
        // GET: Facts
        public ActionResult Index()
        {
            return View();
        }
    }
}