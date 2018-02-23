using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared;
using MvvmDialogs;

namespace Globeweigh.UI.Touch.Helpers
{
    public static class UtilitiesTouch
    {

        private static readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        public static MainWindow GetMainWindow()
        {
            return (from Window f in System.Windows.Application.Current.Windows where f.GetType() == typeof(MainWindow) select f).Cast<MainWindow>().FirstOrDefault();
        }


        public static bool ShowOkCancelMessageView(IViewModel parentVM, string message, bool hideCancel)
        {
            var dialogVm = SimpleIoc.Default.GetInstance<OkCancelViewModel>();
            dialogVm.MessageText = message;
            dialogVm.HideCancel = hideCancel;
            var success = _dialogService.ShowDialog<OkCancelView>(parentVM, dialogVm);
            return success != null && (bool) success;




        }
    }
}
