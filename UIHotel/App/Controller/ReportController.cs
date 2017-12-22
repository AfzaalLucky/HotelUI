﻿using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIHotel.App.Controller
{
    public class ReportController : BaseController
    {
        public ReportController(IRequest request) : base(request)
        {

        }

        public IResourceHandler checkin()
        {
            return View("Report.Checkin");
        }

        public IResourceHandler booking()
        {
            return View("Report.Booking");
        }

        public IResourceHandler finance()
        {
            return View("Report.Finance");
        }

        public IResourceHandler guest()
        {
            return View("Report.Guest");
        }
    }
}