using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Globeweigh.UI.Shared;
using System.Windows;
using Globeweigh.Model;
using System.Reflection;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Helpers;
using Microsoft.Win32;
using MvvmDialogs;

namespace Globeweigh.UI.Touch
{
    public class MainWindowViewModel : BindableBase, IViewModel
    {
        #region private fields

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


        //        private string _CurrentDate;
        //
        //        public string CurrentDate
        //        {
        //            get { return _CurrentDate; }
        //            set { Set(ref _CurrentDate, value); }
        //        }

        //        private string _CurrentVersion;
        //        public string CurrentVersion
        //        {
        //            get { return _CurrentVersion; }
        //            set { Set(ref _CurrentVersion, value); }
        //        }
        //
        //        private bool _NewVersionAvailable;
        //        public bool NewVersionAvailable
        //        {
        //            get { return _NewVersionAvailable; }
        //            set { Set(ref _NewVersionAvailable, value); }
        //        }
        //
        //        private string _NewVersionAvailableMessage;
        //        public string NewVersionAvailableMessage
        //        {
        //            get { return _NewVersionAvailableMessage; }
        //            set { Set(ref _NewVersionAvailableMessage, value); }
        //        }

        #endregion

        public async void Load(FrameworkElement element)
        {
            InTestMode = GlobalVariables.InTestMode;
            var homeViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>();
            CurrentViewModel = homeViewModel;
        }

        public void Unload(FrameworkElement element)
        {

        }
    }
}
