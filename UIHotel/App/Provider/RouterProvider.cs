﻿using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIHotel.App.Controller;

namespace UIHotel.App.Provider
{
    public class RouterProvider : ServiceProvider
    {
        private List<RouteModel> listRoute { get; set; }
        private string controllerNamespace { get; set; } = "UIHotel.App.Controller";

        public override void Register()
        {
            base.Register();

            listRoute = new List<RouteModel>();
            listRoute.Add(new RouteModel("home/get/{action}", "HomeController"));
            listRoute.Add(new RouteModel("home/post/{action}", "HomeController", Method: "POST"));
        }

        public override void Boot()
        {
            base.Boot();
            //
        }

        public ResourceHandler GetResponse(IRequest request)
        {
            foreach (RouteModel model in listRoute)
            {
                if (model.IsMatch(request))
                    return model.GetResponse(request);
            }

            return new ResourceHandler()
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                StatusText = "Not Found"
            };
        }

        public bool HasMatch(IRequest request)
        {
            foreach (RouteModel model in listRoute)
                if (model.IsMatch(request))
                    return true;

            return false;
        }
    }

    public class RouteModel
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Namespace { get; set; } = "UIHotel.App.Controller";
        public string Controller { get; set; }
        public string Action { get; set; }
        public string[] Params { get; set; }
        
        public RouteModel(string Path, string Controller, string Namespace = "UIHotel.App.Controller", string Action = "index", string Method = "GET")
        {
            this.Path = Path;
            this.Controller = Controller;
            this.Action = Action;
            this.Method = Method;
        }

        public bool IsMatch(IRequest request)
        {
            var Url = new Uri(request.Url);
            var Path = Url.AbsolutePath;
            var pattern = Regex.Replace(this.Path, @"{\w+}", @"([^\/\n]+)");

            return Regex.IsMatch(Path, pattern, RegexOptions.IgnoreCase) && request.Method == Method;
        }

        public string GetController(string Path)
        {
            var isControllerContextExists = Regex.IsMatch(this.Path, @"{controller}", RegexOptions.IgnoreCase);

            if (isControllerContextExists)
            {
                var pattern = Regex.Replace(this.Path, @"{controller}", @"([^\/\n]+)");
                var matchColl = Regex.Matches(Path, pattern, RegexOptions.IgnoreCase);
                var match = matchColl[0];

                if (match.Captures.Count == 2)
                    return match.Captures[1].Value;
            }
            
            return Controller;
        }

        public string GetAction(string Path)
        {
            var isActionContextExists = Regex.IsMatch(this.Path, @"{action}", RegexOptions.IgnoreCase);

            if (isActionContextExists)
            {
                var pattern = Regex.Replace(this.Path, @"{action}", @"([^\/\n]+)");
                var matchColl = Regex.Matches(Path, pattern, RegexOptions.IgnoreCase);
                var match = matchColl[0];

                if (match.Captures.Count == 2)
                    return match.Captures[1].Value;
            }

            return Action;
        }

        public ResourceHandler GetResponse(IRequest request)
        {
            var Url = new Uri(request.Url);
            var Path = Url.AbsolutePath;
            var Controller = GetController(Path);
            var Action = GetAction(Path);
            var ClassName = Namespace + "." + Controller;
            
            if (IsMethodExists(ClassName, Action))
            {
                Type type = Type.GetType(ClassName);

                return CreateInstance(type, Action, request);
            }

            return new ResourceHandler() {
                StatusCode = (int)HttpStatusCode.NotFound,
                StatusText = "Not Found",
            };
        }

        private ResourceHandler CreateInstance(Type Type, string Method, IRequest request)
        {
            ConstructorInfo constInfo = Type.GetConstructor(new[] { typeof(IRequest) });
            MethodInfo method = Type.GetMethod(Method);
            Object instance = constInfo.Invoke(new object[] { request });
            
            var result = method?.Invoke(instance, new object[] { });

            if (result != null)
                return (ResourceHandler)result;
            else
                return new ResourceHandler() { StatusCode = (int)HttpStatusCode.NotFound, StatusText = "Method '" + Method + "' Not Found" };
        }

        private bool IsClassExists(string ClassName)
        {
            Type type = Type.GetType(ClassName);

            if (type != null)
                return type.IsClass;

            return false;
        }

        private bool IsMethodExists(string ClassName, string Method)
        {
            if (IsClassExists(ClassName))
            {
                Type type = Type.GetType(ClassName);
                MethodInfo[] methods = type.GetMethods();

                foreach (MethodInfo method in methods)
                    if (method.Name.Equals(Method, StringComparison.OrdinalIgnoreCase) && method.ReturnType != typeof(void))
                        return true;
            }

            return false;
        }
    }
}
