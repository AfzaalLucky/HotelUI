﻿using CefSharp;
using MySql.Data.MySqlClient;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIHotel.App.Provider;
using UIHotel.App.View;
using UIHotel.Data;

namespace UIHotel.App.Controller
{
    public class BaseController
    {
        private DataContext Model { get; set; }
        private MySqlConnection DBConnection { get; set; }
        private IPostData PostData { get; set; }
        private IRequest Request { get; set; }
        private DynamicViewBag _ViewBag
        {
            get
            {
                var expObject = ViewBag as ExpandoObject;
                var dictObject = expObject.ToDictionary(z => z.Key, x => x.Value);
                var viewBag = new DynamicViewBag();

                viewBag.AddDictionary(dictObject);

                return viewBag;
            }
        }
        public dynamic ViewBag { get; set; } = new ExpandoObject();

        public BaseController()
        {
            DBConnection = new MySqlConnection(Properties.Settings.Default.MyDB);
            Model = new DataContext(DBConnection, false);
            
            DBConnection.Open();
        }

        public BaseController(IRequest Request)
        {
            this.Request = Request;
            this.DBConnection = new MySqlConnection(Properties.Settings.Default.MyDB);
            this.Model = new DataContext(DBConnection, false);

            DBConnection.Open();
        }

        public IResourceHandler View(string viewName)
        {
            ViewProvider viewProvider = AppMain.Main["view"] as ViewProvider;

            try
            {
                string renderResult = viewProvider.ViewManager.Render(viewName);

                return ResourceHandler.FromString(renderResult, Encoding.UTF8);
            } catch (ViewNotFoundException ex)
            {
                return ResourceHandler.FromString(ex.ToString());
            }
        }

        public IResourceHandler View(string viewName, object viewData)
        {
            var viewProvider = AppMain.Main["view"] as ViewProvider;

            try
            {
                string renderResult = viewProvider.ViewManager.Render(viewName, viewData, _ViewBag);

                return ResourceHandler.FromString(renderResult, Encoding.UTF8);
            } catch (ViewNotFoundException ex)
            {
                return ResourceHandler.FromString(ex.ToString());
            }
        }
    }
}
