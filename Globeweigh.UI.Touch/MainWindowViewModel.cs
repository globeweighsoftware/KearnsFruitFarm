using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Globeweigh.UI.Shared;
using System.Windows;
using Globeweigh.Model;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.UI.Shared.Services;
using Microsoft.Win32;
using MvvmDialogs;

namespace Globeweigh.UI.Touch
{
    public class MainWindowViewModel : BindableBase, IViewModel
    {
        #region private fields

        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        private bool _ignoreUpdate;
        Assembly _currentAssembly = Assembly.GetExecutingAssembly();

        #endregion

        #region Properties

        private object _CurrentViewModel;
        public object CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { Set(ref _CurrentViewModel, value); }
        }

        private bool _InTestMode;

        public bool InTestMode
        {
            get { return _InTestMode; }
            set { Set(ref _InTestMode, value); }
        }

        private string _CurrentVersion;
        public string CurrentVersion
        {
            get { return _CurrentVersion; }
            set { Set(ref _CurrentVersion, value); }
        }

        #endregion

        public async void Load(FrameworkElement element)
        {
            //Device must be registered first

            await UtilitiesShared.RegisterDevice(SimpleIoc.Default.GetInstance<IDeviceRepository>(), false);


            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                GlobalVariables.CurrentDevice.IsDisplayDevice = true;
            }

            if (_ignoreUpdate) return;

            CurrentVersion = UtilitiesShared.GetCurrentMsiVersion();
            if (!UtilitiesShared.IsMyMachine)
            {
                var newReleaseFullFileName = UtilitiesShared.CheckforUpdate(CurrentVersion, "TouchSetup", out string newReleaseVersion);
                if (newReleaseFullFileName != null)
                {
                    var dialogViewModel = SimpleIoc.Default.GetInstance<DownloadNewUpdateViewModel>();
                    dialogViewModel.NewReleaseFullFileName = newReleaseFullFileName;
                    bool? success = _dialogService.ShowDialog<DownloadNewUpdateView>(this, dialogViewModel);
                    if (success == true)
                    {
                    }
                }
            }
            _ignoreUpdate = true;



            if (GlobalVariables.CurrentDevice == null)return;
            if (GlobalVariables.CurrentDevice.IsDisplayDevice)
            {
                InTestMode = GlobalVariables.InTestMode;
                var homeViewModel = SimpleIoc.Default.GetInstance<BatchDisplay2ViewModel>();
                CurrentViewModel = homeViewModel;
            }
            else
            {
                InTestMode = GlobalVariables.InTestMode;
                var homeViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>();
                CurrentViewModel = homeViewModel;
            }



        }

        public void Unload(FrameworkElement element)
        {

        }
    }
}
