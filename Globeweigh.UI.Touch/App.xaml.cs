using System;
using System.Text.RegularExpressions;
using System.Windows;
using DevExpress.Xpf.Core;
using GalaSoft.MvvmLight.Ioc;
using MvvmDialogs;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.UI.Shared.Services;
using Globeweigh.UI.Shared;
using DialogService = MvvmDialogs.DialogService;
using DispatcherHelper = GalaSoft.MvvmLight.Threading.DispatcherHelper;

namespace Globeweigh.UI.Touch
{
    public partial class App : Application
    {
        public App()
        {
            ApplicationThemeHelper.UseLegacyDefaultTheme = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            if (UtilitiesShared.CheckForOtherInstanceOfApplication()) return;
            ExceptionHelper.Initialize();

            var success = Database.SetAndTestDbConnection();
            if (!success)
            {
                Application.Current.StartupUri = new Uri("/Globeweigh.UI.Shared;component/ErrorHandling/DbErrorWindow.xaml", UriKind.Relative);
                return;
            }
            SimpleIocRegistration();

            DispatcherHelper.Initialize();
            UtilitiesShared.RegisterDevice(SimpleIoc.Default.GetInstance<IDeviceRepository>(), false);
        }


        private void SimpleIocRegistration()
        {
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
            SimpleIoc.Default.Register<IReferenceDataRepository>(() => new ReferenceDataRepository());
            SimpleIoc.Default.Register<IDeviceRepository>(() => new DeviceRepository());
            SimpleIoc.Default.Register<IBatchRepository>(() => new BatchRepository());
            SimpleIoc.Default.Register<IOperatorRepository>(() => new OperatorRepository());
            SimpleIoc.Default.Register<IProductRepository>(() => new ProductRepository());
        }


    }
}
