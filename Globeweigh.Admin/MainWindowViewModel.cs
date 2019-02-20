using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Admin.Helpers;
using Globeweigh.UI.Shared;
using Globeweigh.UI.Shared.Helpers;
using System.Reflection;
using System.Threading.Tasks;
using Globeweigh.Model;
using IDialogService = MvvmDialogs.IDialogService;

namespace Globeweigh.Admin
{
    public class MainWindowViewModel : AdminViewModelBase, IViewModel
    {
        #region private fields

        private string _newVersion;
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        Assembly _currentAssembly = Assembly.GetExecutingAssembly();
        private System.Windows.Threading.DispatcherTimer _dispatcherTimer;

        #endregion

        #region Properties

        public string NewReleaseFullFileName { get; set; }

        private bool _InTestMode;
        public bool InTestMode
        {
            get { return _InTestMode; }
            set { Set(ref _InTestMode, value); }
        }

        private object _CurrentViewModel;
        public object CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { Set(ref _CurrentViewModel, value); }
        }

        private string _MainHeader;
        public string MainHeader
        {
            get { return _MainHeader; }
            set { Set(ref _MainHeader, value); }
        }

        private string _LoggedInUserDisplay;
        public string LoggedInUserDisplay
        {
            get { return _LoggedInUserDisplay; }
            set { Set(ref _LoggedInUserDisplay, value); }
        }

        private string _CurrentVersion;
        public string CurrentVersion
        {
            get { return _CurrentVersion; }
            set { Set(ref _CurrentVersion, value); }
        }

        private bool _IsGlobeweighAdmin;
        public bool IsGlobeweighAdmin
        {
            get { return _IsGlobeweighAdmin; }
            set { Set(ref _IsGlobeweighAdmin, value); }
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

        private ListBoxItem _SelectedNavItem;
        public ListBoxItem SelectedNavItem
        {
            get { return _SelectedNavItem; }
            set
            {
                Set(ref _SelectedNavItem, null);
                Set(ref _SelectedNavItem, value);
                SetCurrentView();
            }
        }

        #endregion

        #region Commands

        public RelayCommand DownloadNewVersionCommand => new RelayCommand(OnDownloadNewVersion);

        #endregion

        private void SetCurrentView()
        {
            switch (SelectedNavItem.Name)
            {
                case "batch":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<PortionControlMasterViewModel>();
                    MainHeader = "Batches";
                    break;
                case "products":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<ProductListViewModel>();
                    MainHeader = "Products";
                    break;
                case "operators":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<OperatorListViewModel>();
                    MainHeader = "Operators";
                    break;
                case "scales":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<ScaleListViewModel>();
                    MainHeader = "Scales";
                    break;
                case "devices":
                    CurrentViewModel = null;
                    MainHeader = "Devices";
                    break;
                case "entities":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<EntitiesViewModel>();
                    MainHeader = "Entities";
                    break;
                default:
                    CurrentViewModel = null;
                    break;
            }
        }

        public async void Load(FrameworkElement element)
        {
//            var dialogViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();
//            bool? success = _dialogService.ShowDialog<LoginView>(this, dialogViewModel);
//            if (success == true)
//            {
//                WeighbridgeVisible = true;
//                ProductsVisible = true;
//                if (GlobalVariables.CurrentGlobeweighUser.RoleId == (int)eUserRole.Accounts)
//                {
//                    ProductsVisible = false;
//                }
//                else if (GlobalVariables.CurrentGlobeweighUser.RoleId == (int)eUserRole.Dispatch)
//                {
//                    ProductsVisible = false;
//                }
//            }
//            else
//            {
//                App.Current.Shutdown();
//                return;
//            }

            if (UtilitiesShared.IsMyMachine) IsGlobeweighAdmin = true;
            InTestMode = GlobalVariables.InTestMode;
            //            LoggedInUserDisplay = GlobalVariables.CurrentGlobeweighUser.Email;
            LoggedInUserDisplay = "Administrator";
            CurrentVersion = _currentAssembly.GetName().Version.ToString();
            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            _dispatcherTimer.Start();
            await Task.Run(() => CheckForUpdate());
//            CheckForUpdate();
        }

        private async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() => CheckForUpdate());
        }

        private async void CheckForUpdate()
        {
            try
            {
                CurrentVersion = UtilitiesShared.GetCurrentMsiVersion();
                if (UtilitiesShared.IsMyMachine) return;
                NewReleaseFullFileName = UtilitiesShared.CheckforUpdate(CurrentVersion, "AdminSetup", out _newVersion);
                if (NewReleaseFullFileName != null)
                {
                    NewVersionAvailable = true;
                    NewVersionAvailableMessage = "New update available (" + _newVersion + ")";
                }
                else
                {
                    NewVersionAvailable = false;
                    _newVersion = null;
                }
            }
            catch (Exception e)
            {
               ExceptionHelper.LogError(e, "Admin MainWindowViewModel OnTick");
            }
        }

        private async void OnDownloadNewVersion()
        {
            var dialogViewModel = SimpleIoc.Default.GetInstance<DownloadNewUpdateViewModel>();
            dialogViewModel.NewReleaseFullFileName = NewReleaseFullFileName;
            bool? success = _dialogService.ShowDialog<DownloadNewUpdateView>(this, dialogViewModel);
            if (success == true)
            {
            }
        }

        public void Unload(FrameworkElement element)
        {
//            _dispatcherTimer.Stop();
//            _dispatcherTimer.Tick -= dispatcherTimer_Tick;
//            _dispatcherTimer = null;
        }
    }
}