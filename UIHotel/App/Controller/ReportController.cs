﻿using CefSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIHotel.App.Attributes;
using UIHotel.Data;
using UIHotel.Data.Table;

namespace UIHotel.App.Controller
{
    [Authorize(Auth.AuthLevel.Manager)]
    [Authorize(Auth.AuthLevel.Administrator)]
    public class ReportController : BaseController
    {
        public ReportController(IRequest request) : base(request)
        {

        }

        public IResourceHandler checkin()
        {
            return View("Report.Checkin");
        }

        public IResourceHandler finance()
        {
            return View("Report.Finance");
        }

        public IResourceHandler getReportMoney()
        {
            var token = jToken;
            var range_type = token.Value<string>("range_type");
            var bdate = token.Value<DateTime?>("bdate");
            var edate = token.Value<DateTime?>("edate");

            using (var model = new DataContext())
            {
                try
                {
                    switch (range_type)
                    {
                        case "d":
                            // Days
                            break;
                        case "m":
                            // Monthly
                            break;
                        case "y":
                            // Yearly
                            break;
                    }

                    return Json(new { success = true });
                }
                catch
                {
                    return Json(new { success = false });
                }
            }
        }

        public IResourceHandler getReportCheckin()
        {
            var token = jToken;
            var range_type = token.Value<string>("range_type");
            var bdate = token.Value<DateTime>("bdate");
            var edate = token.Value<DateTime?>("edate");

            using (var model = new DataContext())
            {
                try
                {
                    switch (range_type)
                    {
                        case "d":
                            // Days
                            edate = bdate.AddDays(1);
                            break;
                        case "m":
                            // Monthly
                            bdate = new DateTime(bdate.Year, bdate.Month, 1);
                            edate = bdate.AddMonths(1);
                            break;
                        case "y":
                            // Yearly
                            bdate = new DateTime(bdate.Year, 1, 1);
                            edate = bdate.AddYears(1);
                            break;
                    }

                    var iBooking = (from a in model.Bookings
                                    where a.CreateAt >= bdate
                                    where a.CreateAt < edate
                                    select a);
                    var iCheckin = (from a in model.CheckIn
                                    where a.CheckinAt >= bdate
                                    where a.CheckinAt < edate
                                    select a);
                    var iCheckout = (from a in model.CheckIn
                                     where a.CheckoutAt >= bdate
                                     where a.CheckoutAt < edate
                                     where a.CheckoutAt.HasValue
                                     select a);

                    var listBooking = iBooking.ToList();
                    var listCheckin = iCheckin.ToList();
                    var listCheckout = iCheckout.ToList();

                    var grpCheckin = listCheckin
                        .GroupBy(x => x.CheckinAt.Date)
                        .Select(x => new CheckinResult()
                        {
                            Date = x.Key,
                            ListCheckin = x.ToList()
                        }).ToList();
                    var grpCheckout = listCheckout
                        .GroupBy(x => x.CheckoutAt.Value.Date)
                        .Select(x => new CheckinResult()
                        {
                            Date = x.Key,
                            ListCheckin = x.ToList()
                        }).ToList();
                    var grpBooking = listBooking
                        .GroupBy(x => x.CreateAt.Date)
                        .Select(x => new BookingResult()
                        {
                            Date = x.Key,
                            ListBooking = x.ToList()
                        }).ToList();

                    var i = bdate;
                    var data = new List<ReportResult>();

                    do
                    {
                        var result = new ReportResult()
                        {
                            Date = i,
                            BookingCount = (from a in grpBooking where a.Date == i select a.Count).FirstOrDefault(),
                            CheckinCount = (from a in grpCheckin where a.Date == i select a.Count).FirstOrDefault(),
                            CheckoutCount = (from a in grpCheckout where a.Date == i select a.Count).FirstOrDefault(),
                        };

                        data.Add(result);

                        i = i.AddDays(1);
                    } while (i <= edate);

                    return Json(new { success = true, data });
                } catch
                {
                    return Json(new { success = false });
                }
            }
        }
    }

    public class BookingResult
    {
        public DateTime Date { get; set; }

        [JsonIgnore]
        public List<Booking> ListBooking { get; set; }

        public int Count
        {
            get
            {
                return ListBooking.Sum(x => x.CountAdult) + ListBooking.Sum(x => x.CountChild);
            }
        }
    }

    public class CheckinResult
    {
        public DateTime Date { get; set; }

        [JsonIgnore]
        public List<Checkin> ListCheckin { get; set; }

        public int Count
        {
            get
            {
                return ListCheckin.Sum(x => x.CountAdult) + ListCheckin.Sum(x => x.CountChild);
            }
        }
    }

    public class ReportResult
    {
        public DateTime Date { get; set; }
        public int CheckinCount { get; set; }
        public int CheckoutCount { get; set; }
        public int BookingCount { get; set; }
    }
}
