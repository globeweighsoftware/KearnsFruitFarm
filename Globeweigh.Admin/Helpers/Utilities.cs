using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Grid;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared;
using MvvmDialogs;

namespace Globeweigh.Admin.Helpers
{
    public static class Utilities
    {
        private static IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        public static MainWindow GetMainWindow()
        {
            return (from Window f in System.Windows.Application.Current.Windows where f.GetType() == typeof(MainWindow) select f).Cast<MainWindow>().FirstOrDefault();
        }

        public static void PrintExportGrid(TableView tableView, IViewModel vm)
        {

            var dialogViewModel = SimpleIoc.Default.GetInstance<PrintExportOptionsViewModel>();
            dialogViewModel.CurrentTableView = tableView;
            bool? success = _dialogService.ShowDialog<PrintExportOptionsView>(vm, dialogViewModel);
            if (success == true)
            {

            }
        }

        public static async Task<bool> PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                var reply =await pinger.SendPingAsync(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
            return pingable;
        }
    }
}
