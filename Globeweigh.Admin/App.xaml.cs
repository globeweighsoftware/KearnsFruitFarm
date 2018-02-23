using System;
using System.Reflection;
using Globeweigh.Model;
using System.Windows;
using DevExpress.Xpf.Core;
using MvvmDialogs;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.UI.Shared.Services;
using DispatcherHelper = GalaSoft.MvvmLight.Threading.DispatcherHelper;
using Globeweigh.UI.Shared;

namespace Globeweigh.Admin
{
    public partial class App : Application
    {
        public App()
        {
            ApplicationThemeHelper.UseLegacyDefaultTheme = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ExceptionHelper.Initialize();
            var success = Database.SetAndTestDbConnection();
            if (!success)
            {
                Application.Current.StartupUri = new Uri("/Globeweigh.UI.Shared;component/ErrorHandling/DbErrorWindow.xaml", UriKind.Relative);
                return;
            }
            SimpleIocRegistration();
            //            var autoStartConfigured = AppShortcut.AutoStart(false);
            DispatcherHelper.Initialize();
            //            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.HybridApp.Name;
            UtilitiesShared.RegisterDevice(SimpleIoc.Default.GetInstance<IDeviceRepository>(), false);

//            var dialog = new LoginView();
//
//            if (dialog.ShowDialog() == true)
//            {
//                var mainWindow = new MainWindow();
//                mainWindow.Show();
//                //Re-enable normal shutdown mode.
//                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
//                Current.MainWindow = mainWindow;
//                mainWindow.Show();
//                dialog.Close();
//            }


        }

        private void SimpleIocRegistration()
        {
            SimpleIoc.Default.Register<IDeviceRepository>(() => new DeviceRepository());
            SimpleIoc.Default.Register<IDialogService>(() => new MvvmDialogs.DialogService());
            SimpleIoc.Default.Register<IReferenceDataRepository>(() => new ReferenceDataRepository());
            SimpleIoc.Default.Register<IProductRepository>(() => new ProductRepository());
            SimpleIoc.Default.Register<IOperatorRepository>(() => new OperatorRepository());
        }
    }


}
