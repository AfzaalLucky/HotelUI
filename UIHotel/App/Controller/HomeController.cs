﻿using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIHotel.App.Controller
{
    public class HomeController : BaseController
    {
        public IResourceHandler index()
        {
            return View();
        }
    }
}
