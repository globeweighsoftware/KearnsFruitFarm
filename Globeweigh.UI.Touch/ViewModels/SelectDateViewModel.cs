using Globeweigh.UI.Shared;
using Globeweigh.Model;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Globeweigh.UI.Touch
{
    public class SelectDateViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        #endregion

        #region Properties

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            private set { Set(ref _SelectedDate, value); }
        }

        private ObservableCollection<DateTime> _SelectedDates;
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return _SelectedDates; }
            set
            {
                if (_SelectedDates != value)
                {
                    _SelectedDates = value;
                    OnPropertyChanged("SelectedDates");
                }
            }
        }



        #endregion

        #region Commands
        public RelayCommand OkCommand => new RelayCommand(OnSave, CanSave);

        #endregion

        private bool CanSave()
        {
            if (SelectedDates == null) return false;
            if (SelectedDates.Count != 1) return false;
            return true;
        }

        public async void Load(FrameworkElement element)
        {
        }

        private async void OnSave()
        {
            SelectedDate = SelectedDates.FirstOrDefault();
            DialogResult = true;
        }

        private void OnCancel()
        {
            DialogResult = false;
        }

        public void Unload(FrameworkElement element)
        {
            SelectedDates = new ObservableCollection<DateTime>();
            DialogResult = null;
        }
    }
}
