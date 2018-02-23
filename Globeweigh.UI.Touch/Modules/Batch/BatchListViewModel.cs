using System;
using System.Collections.Generic;
using System.Media;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model;
using Globeweigh.UI.Shared;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.UI.Shared.Services;
using System.Threading.Tasks;
using DevExpress.CodeParser;
using Globeweigh.Model.Custom;
using DevExpress.XtraReports.UI;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.UI.Touch.Helpers;
using IDialogService = MvvmDialogs.IDialogService;

namespace Globeweigh.UI.Touch
{
    public class BatchListViewModel : BindableBase, IViewModel
    {
        #region private fields

        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

        private DateTime _FilterDate;
        public DateTime FilterDate
        {
            get { return _FilterDate; }
            set { Set(ref _FilterDate, value); }
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { Set(ref _CurrentDate, value); }
        }

        private List<vwBatchView> _BatchList;
        public List<vwBatchView> BatchList
        {
            get { return _BatchList; }
            set { Set(ref _BatchList, value); }
        }

        private vwBatchView _SelectedBatch;
        public vwBatchView SelectedBatch
        {
            get { return _SelectedBatch; }
            set { Set(ref _SelectedBatch, value); }
        }

        #endregion

        #region Commands

        public RelayCommand AddNewBatchCommand => new RelayCommand(ShowAddNewBatchDialog);

        public RelayCommand BatchDetailsCommand => new RelayCommand(OnBatchDetails, CanBatchDetailsExecute);
        public RelayCommand CloseOpenBatchCommand => new RelayCommand(OnCloseOpenBatch);
        public RelayCommand DeleteBatchCommand => new RelayCommand(OnBatchDelete);
        public RelayCommand EditBatchCommand => new RelayCommand(OnEditBatch);
        public RelayCommand FilterDateBackCommand => new RelayCommand(OnFilterDateBack);
        public RelayCommand FilterDateForwardCommand => new RelayCommand(OnFilterDateForward);
        public RelayCommand HomeCommand => new RelayCommand(NavigateToHomeView);
        public RelayCommand BatchReportCommand => new RelayCommand(OnBatchReport);



        #endregion

        private bool CanBatchDetailsExecute()
        {
            if (SelectedBatch == null) return false;
            if (SelectedBatch.Closed) return false;
            return true;
        }

        private async void OnBatchReport()
        {
            //var rpt = new rptBatchReport2(SelectedBatch.id, SelectedBatch, _batchRepo, _portionRepo);
            //rpt.ShowPreviewDialog();
        }

        private void OnEditBatch()
        {
            var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<AddBatchViewModel>();
            dialogViewModel.BatchId = SelectedBatch.id;
            bool? success = _dialogService.ShowDialog<AddBatchView>(this, dialogViewModel);
            if (success == true)
            {
                SelectedBatch = null;
                RefreshBatchList();
            }
        }

        private async void OnCloseOpenBatch()
        {
            var message = SelectedBatch.Closed ? "Re-open Batch?" : "Close Batch?";

            var res = _dialogService.ShowMessageBox(this, message, "", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                await _batchRepo.CloseOpenBatch(SelectedBatch.id, !SelectedBatch.Closed);
                RefreshBatchList();
                SelectedBatch = null;
            }
        }

        private async void OnBatchDelete()
        {
            var res = _dialogService.ShowMessageBox(this, "Delete Batch?", "", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                await _batchRepo.MarkBatchAsDeletedAsync(SelectedBatch.id);
                SelectedBatch = null;
                RefreshBatchList();
            }
        }

        private void OnBatchDetails()
        {
            var batchDetailsViewModel = SimpleIoc.Default.GetInstance<BatchDetailsViewModel>();
            batchDetailsViewModel.SelectedBatchView = SelectedBatch;
            SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = batchDetailsViewModel;
        }

        private void OnFilterDateBack()
        {
            FilterDate = FilterDate.AddMonths(-1);
            RefreshBatchList();
        }

        private void OnFilterDateForward()
        {
            FilterDate = FilterDate.AddMonths(1);
            RefreshBatchList();
        }

        private void NavigateToHomeView()
        {
            SelectedBatch = null;
            BatchList = null;
            SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>();
        }

        private void ShowAddNewBatchDialog()
        {
            var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<AddBatchViewModel>();
            bool? success = _dialogService.ShowDialog<AddBatchView>(this, dialogViewModel);
            if (success == true)
            {
                RefreshBatchList();
            }
        }

        public async void RefreshBatchList()
        {
            BatchList = new List<vwBatchView>(await _batchRepo.GetBatchesAsync(FilterDate));
        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            FilterDate = DateTime.Today;
            CurrentDate = DateTime.Now;
            if (SelectedBatch == null) RefreshBatchList();
            RefreshBatchList();
            if (SelectedBatch != null) RefreshBatchList();
        }

        public void Unload(FrameworkElement element)
        {

        }
    }
}
