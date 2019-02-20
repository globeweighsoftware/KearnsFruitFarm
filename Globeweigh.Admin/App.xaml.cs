using System;
using System.Reflection;
using System.Threading.Tasks;
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

        protected async override void OnStartup(StartupEventArgs e)
        {
            SplashView splash = new SplashView();
            splash.Show();
            var databaseOk = await Task.Run(StartupMethod);
            if (!databaseOk)
            {
                var errorWindow = new DbErrorWindow();
                errorWindow.Show();
                splash.Close();
                return;
            }
            MainWindow main = new MainWindow();
            main.InitializeComponent();
            Application.Current.MainWindow = main;
            //            await Task.Delay(3000);
            splash.Close();
            main.Show();


        }

        private async Task<bool> StartupMethod()
        {
            ExceptionHelper.Initialize();
            var success = Database.SetAndTestDbConnection();
            if (!success) { return false; }
            SimpleIocRegistration();
//            GlobalVariables.GlobalDbSettings = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetSettings();
            DispatcherHelper.Initialize();
            UtilitiesShared.RegisterDevice(SimpleIoc.Default.GetInstance<IDeviceRepository>(), false);
            return true;
        }

        private void SimpleIocRegistration()
        {
            SimpleIoc.Default.Register<IDeviceRepository>(() => new DeviceRepository());
            SimpleIoc.Default.Register<IDialogService>(() => new MvvmDialogs.DialogService());
            SimpleIoc.Default.Register<IReferenceDataRepository>(() => new ReferenceDataRepository());
            SimpleIoc.Default.Register<IProductRepository>(() => new ProductRepository());
            SimpleIoc.Default.Register<IOperatorRepository>(() => new OperatorRepository());
            SimpleIoc.Default.Register<IScaleRepository>(() => new ScaleRepository());
            SimpleIoc.Default.Register<IBatchRepository>(() => new BatchRepository());
            SimpleIoc.Default.Register<IPortionRepository>(() => new PortionRepository());
        }
    }


}
