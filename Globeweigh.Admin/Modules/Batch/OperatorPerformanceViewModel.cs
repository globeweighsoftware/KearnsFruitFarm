using System;
using System.Collections.Generic;
using Globeweigh.UI.Shared.Services;
using Globeweigh.UI.Shared;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.Model;
using GalaSoft.MvvmLight.Ioc;
using IDialogService = MvvmDialogs.IDialogService;
using System.Threading.Tasks;

namespace Globeweigh.Admin
{
    public class OperatorPerformanceViewModel : AdminViewModelBase, IViewModel
    {
        #region private fields

        private readonly IPortionRepository _portionRepo = SimpleIoc.Default.GetInstance<IPortionRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

        private List<spOperatorStats_Result> _OperatorList;
        public List<spOperatorStats_Result> OperatorList
        {
            get { return _OperatorList; }
            set { Set(ref _OperatorList, value); }
        }

//        private vwBatchView _SelectedBatch;
//        public vwBatchView SelectedBatch
//        {
//            get { return _SelectedBatch; }
//            set { Set(ref _SelectedBatch, value); }
//        }

        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime _DateTo;
        public DateTime DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }


        #endregion

        #region Commands

        public RelayCommand RefreshCommand => new RelayCommand(OnRefresh);
//        public RelayCommand ViewCommand => new RelayCommand(OnViewCommand);



        #endregion

//        private async void OnViewCommand()
//        {
//            if(se == null)return;
//
//          
//
//
//            var report = new rptBatchReport(SelectedBatch);
//            report.ShowPreviewDialog();
//
//
//        }

        private async void OnRefresh()
        {
            await RefreshList();
        }



        private async Task RefreshList()
        {
            IsBusy = true;
            OperatorList = new List<spOperatorStats_Result>(_portionRepo.GetOperatorStats(DateFrom.Date, DateTo.Date.AddDays(1).AddSeconds(-1)));
            IsBusy = false;
        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            await RefreshList();
        }

        public void Unload(FrameworkElement element)
        {
            OperatorList = null;
            IsBusy = false;
        }

    }
}
