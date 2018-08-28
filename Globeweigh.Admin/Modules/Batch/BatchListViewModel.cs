using System;
using System.Collections.Generic;
using System.Linq;
using Globeweigh.UI.Shared.Services;
using Globeweigh.UI.Shared;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.Model;
using GalaSoft.MvvmLight.Ioc;
using IDialogService = MvvmDialogs.IDialogService;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;
using Globeweigh.Reports;

namespace Globeweigh.Admin
{
    public class BatchListViewModel : AdminViewModelBase, IViewModel
    {
        #region private fields

        private readonly IBatchRepository _BatchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

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
        public RelayCommand ViewCommand => new RelayCommand(OnViewCommand);



        #endregion

        private async void OnViewCommand()
        {
            if(SelectedBatch == null)return;

            var report = new rptBatchReport2(SelectedBatch);
            report.ShowPreviewDialog();
        }

        private async void OnRefresh()
        {
            await RefreshList();
        }



        private async Task RefreshList()
        {
            IsBusy = true;
            BatchList = new List<vwBatchView>(await _BatchRepo.GetBatchesAsync(DateFrom.Date, DateTo.Date));
            IsBusy = false;
        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            DateFrom = DateTime.Today.AddMonths(-1);
            DateTo = DateTime.Today.AddDays(1);
            await RefreshList();
        }

        public void Unload(FrameworkElement element)
        {
            BatchList = null;
            SelectedBatch = null;
            IsBusy = false;
        }

    }
}
