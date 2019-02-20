using Globeweigh.UI.Shared;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Ioc;

namespace Globeweigh.Admin
{
    public class PortionControlMasterViewModel : AdminViewModelBase, IViewModel
    {
        #region Properties

        private object _CurrentViewModel;
        public object CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { Set(ref _CurrentViewModel, value); }
        }

        private ListBoxItem _SelectedNavItem;
        public ListBoxItem SelectedNavItem
        {
            get { return _SelectedNavItem; }
            set
            {
                Set(ref _SelectedNavItem, value);
                if (SelectedNavItem != null) SetCurrentView();
            }
        }

        #endregion

        private void SetCurrentView()
        {
            switch (SelectedNavItem.Name)
            {
                case "Batches":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<BatchListViewModel>();
                    break;
                case "Products":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<BatchListViewModel>();
                    break;
                case "Operators":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<OperatorListViewModel>();
                    break;
                case "Scales":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<ScaleListViewModel>();
                    break;
                case "OperatorPerformance":
                    CurrentViewModel = SimpleIoc.Default.GetInstance<OperatorPerformanceViewModel>();
                    break;
                   
                default:
                    CurrentViewModel = null;
                    break;
            }
        }

        public async void Load(FrameworkElement element)
        {
        }

        public void Unload(FrameworkElement element)
        {
        }
    }
}
