using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Helpers;

namespace Globeweigh.UI.Touch
{
    public class ViewModelLocator
    {
        private static bool _isInitialized;

        public ViewModelLocator()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            if(UtilitiesShared.InDesignMode)return;

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<SelectRefDataViewModel>();
            SimpleIoc.Default.Register<KeypadViewModel>();
            SimpleIoc.Default.Register<SelectDateViewModel>();
            SimpleIoc.Default.Register<CleanScreenViewModel>();
            SimpleIoc.Default.Register<KeypadViewModel>();
            SimpleIoc.Default.Register<KeyboardViewModel>();
            SimpleIoc.Default.Register<OkCancelViewModel>();
            SimpleIoc.Default.Register<DownloadNewUpdateViewModel>();
            SimpleIoc.Default.Register<BatchListViewModel>();
            SimpleIoc.Default.Register<AddBatchViewModel>();
            SimpleIoc.Default.Register<SelectProductViewModel>();
            SimpleIoc.Default.Register<BatchDetailsViewModel>();
            SimpleIoc.Default.Register<SelectOperatorViewModel>();
        }

        public MainWindowViewModel MainWindow => SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        public HomeViewModel HomeView => SimpleIoc.Default.GetInstance<HomeViewModel>();
        public CleanScreenViewModel CleanScreenView => SimpleIoc.Default.GetInstance<CleanScreenViewModel>();
        public BatchListViewModel BatchListView => SimpleIoc.Default.GetInstance<BatchListViewModel>();
        public BatchDetailsViewModel BatchDetailsView => SimpleIoc.Default.GetInstance<BatchDetailsViewModel>();
    }
}