﻿using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorEngine.Templating;
using System;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using UIHotel.App.Provider;
using UIHotel.App.View;

namespace UIHotel.App.Controller
{
    public class BaseController
    {
        public IPostData PostData { get => Request.PostData; }
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

        public JToken jToken
        {
            get
            {
                if (Request.ResourceType == ResourceType.Xhr)
                {
                    var postElm = PostData.Elements;
                    var jsonContent = postElm[0].GetBody();

                    return JToken.Parse(jsonContent);
                } else
                {
                    return JToken.Parse("{}");
                }
            }
        }

        public NameValueCollection Query
        {
            get
            {
                var url = new Uri(Request.Url);

                return HttpUtility.ParseQueryString(url.Query);
            }
        }
        public IRequest Request { get; set; }
        public dynamic ViewBag { get; set; } = new ExpandoObject();

        public BaseController()
        {
        }

        public BaseController(IRequest Request)
        {
            this.Request = Request;
        }

        public IResourceHandler View(string viewName)
        {
            var viewProvider = AppMain.Main["view"] as ViewProvider;

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

        public IResourceHandler Json(object data)
        {
            try
            {
                var config = new JsonSerializerSettings();

                config.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                config.Formatting = Formatting.None;

                var dataJson = JsonConvert.SerializeObject(data, config);
                var retValue = ResourceHandler.FromString(dataJson, mimeType: "application/json");
                
                return retValue;
            }
            catch (Exception ex)
            {
                return ResourceHandler.FromString(ex.ToString());
            }
        }

        public IResourceHandler Redirect(string url)
        {
            var resource = new ResourceHandler();
            resource.StatusCode = (int)HttpStatusCode.Redirect;
            resource.Headers.Add("Location", url);

            return resource;
        }
    }
}
