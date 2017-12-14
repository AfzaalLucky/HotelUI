﻿using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIHotel.Model;
using CefSharp;
using System.Drawing;
using UIHotel.App.Provider;

namespace UIHotel.App
{
    public class AppMain : IDisposable
    {
        private ChromiumWebBrowser browser;
        private Form mainForm;
        private AppRequestHandler RequestHandler;
        private const string ViewPath = "View";
        private const string AssetPath = "Assets";
        private const string Domain = "localhost.com";
        private string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        private List<ServiceProvider> listServiceProvider = new List<ServiceProvider>();
        public static AppMain Main { get; set; }

        public void RegisterProvider()
        {
            foreach (ServiceProvider provider in listServiceProvider)
                provider.Register();
        }

        public void BootProvider()
        {
            foreach (ServiceProvider provider in listServiceProvider)
                provider.Boot();
        }

        public bool IsShowDevTool { get; set; }

        public string ViewFullPath
        {
            get => Path.Combine(BaseDir, ViewPath);
        }

        public string AssetsFullPath
        {
            get => Path.Combine(BaseDir, AssetPath);
        }

        public ServiceProvider this[string provide]
        {
            get
            {
                foreach (ServiceProvider provider in listServiceProvider)
                    if (provider.Provide() == provide)
                        return provider;

                return null;
            }
        }

        public AppMain()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IsShowDevTool = false;
            RequestHandler = new AppRequestHandler(Domain);
            
            ConfigureSetting();
            ConfigureRoute();

            listServiceProvider.Add(new RouterProvider());
            listServiceProvider.Add(new ViewProvider());

            RegisterProvider();
            BootProvider();

            browser = new ChromiumWebBrowser("about:blank");
            browser.RegisterAsyncJsObject("CS", new BaseModel());
            browser.Dock = DockStyle.Fill;
            browser.FrameLoadStart += Browser_FrameLoadStart;
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            mainForm = new Form();
            mainForm.WindowState = FormWindowState.Maximized;
            mainForm.Controls.Add(browser);
        }

        private void ConfigureSetting()
        {
            var settings = new CefSettings()
            {
                // Settings cef
                RemoteDebuggingPort = 54477,
                IgnoreCertificateErrors = true,
            };

            settings.RegisterScheme(RequestHandler.Scheme);

            Cef.Initialize(settings);
        }

        private void ConfigureRoute()
        {
            var LayoutTemplate = Path.Combine(ViewFullPath, "Layout\\Layout.cshtml");
            RequestHandler.RegisterPath(AssetsFullPath, "/");
            RequestHandler.RegisterPath(ViewFullPath, "/");
            RequestHandler.RegisterTemplate(LayoutTemplate, "Layout");
        }

        private void Browser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized && IsShowDevTool)
            {
                browser.ShowDevTools();
            }
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Run(string startUpUrl)
        {
            browser.Load(startUpUrl);

            Run();
        }

        public void Run()
        {
            Application.Run(mainForm);
            Cef.Shutdown();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~App() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
