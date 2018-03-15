using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model;
using Globeweigh.UI.Shared;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.UI.Shared.Helpers;
using System;
using System.Reflection;
using MvvmDialogs;

namespace Globeweigh.UI.Touch
{
    public class HomeViewModel : BindableBase, IViewModel
    {
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        Assembly _currentAssembly = Assembly.GetExecutingAssembly();


        #region Commands

        public RelayCommand ExitCommand => new RelayCommand(OnExit);
        public RelayCommand CleanScreenCommand => new RelayCommand(() => SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = SimpleIoc.Default.GetInstance<CleanScreenViewModel>());
        public RelayCommand DownloadNewVersionCommand => new RelayCommand(OnDownloadNewVersion);
        public RelayCommand BatchesCommand => new RelayCommand(() => SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = SimpleIoc.Default.GetInstance<BatchListViewModel>());

        #endregion

        #region Properties

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { Set(ref _CurrentDate, value); }
        }

        private string _CurrentVersion;
        public string CurrentVersion
        {
            get { return _CurrentVersion; }
            set { Set(ref _CurrentVersion, value); }
        }

        private bool _NewVersionAvailable;
        public bool NewVersionAvailable
        {
            get { return _NewVersionAvailable; }
            set { Set(ref _NewVersionAvailable, value); }
        }

        private string _NewVersionAvailableMessage;
        public string NewVersionAvailableMessage
        {
            get { return _NewVersionAvailableMessage; }
            set { Set(ref _NewVersionAvailableMessage, value); }
        }

        #endregion

        private async void OnDownloadNewVersion()
        {

        }



        private void OnExit()
        {
            Application.Current.Shutdown();
        }

        public async void Load(FrameworkElement element)
        {
            CurrentDate = DateTime.Now;
            if (UtilitiesShared.InDesignMode) return;






        }

        public void Unload(FrameworkElement element)
        {

        }
    }

}
