using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        protected async override void OnStartup(StartupEventArgs e)
        {

            if (UtilitiesShared.CheckForOtherInstanceOfApplication()) return;
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


        private async void SimpleIocRegistration()
        {
            SimpleIoc.Default.Register<IDeviceRepository>(() => new DeviceRepository());
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
            SimpleIoc.Default.Register<IReferenceDataRepository>(() => new ReferenceDataRepository());
            SimpleIoc.Default.Register<IBatchRepository>(() => new BatchRepository());
            SimpleIoc.Default.Register<IOperatorRepository>(() => new OperatorRepository());
            SimpleIoc.Default.Register<IProductRepository>(() => new ProductRepository());
            SimpleIoc.Default.Register<IPortionRepository>(() => new PortionRepository());
            SimpleIoc.Default.Register<IScaleRepository>(() => new ScaleRepository());
            SimpleIoc.Default.Register<IPortionDisplayRepository>(() => new PortionDisplayRepository());
        }


    }
}
