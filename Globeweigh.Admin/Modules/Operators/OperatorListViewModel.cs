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

namespace Globeweigh.Admin
{
    public class OperatorListViewModel : AdminViewModelBase, IViewModel
    {
        #region private fields

        private readonly IOperatorRepository _operatorRepo = SimpleIoc.Default.GetInstance<IOperatorRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

        private List<Operator> _OperatorList;
        public List<Operator> OperatorList
        {
            get { return _OperatorList; }
            set { Set(ref _OperatorList, value); }
        }

        private Operator _SelectedOperator;
        public Operator SelectedOperator
        {
            get { return _SelectedOperator; }
            set { Set(ref _SelectedOperator, value); }
        }


        #endregion

        #region Commands

        public RelayCommand AddCommand => new RelayCommand(OnAdd, CanAddEditDelete);
        public RelayCommand EditCommand => new RelayCommand(OnEdit, CanAddEditDelete);
        public RelayCommand DeleteCommand => new RelayCommand(OnDelete, CanAddEditDelete);
        public RelayCommand RefreshCommand => new RelayCommand(OnRefresh);




        #endregion

        private bool CanAddEditDelete()
        {
            return UtilitiesShared.IsMyMachine;
        }


        private void OnAdd()
        {
            AddEditNewRefDataDialog(0);
        }
        private void OnEdit()
        {
            AddEditNewRefDataDialog(SelectedOperator.id);
        }

        private async void OnRefresh()
        {
            await RefreshList();
        }

        private async void OnDelete()
        {
            var result = _dialogService.ShowMessageBox(this, "Delete Item?", "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel) return;
            await _operatorRepo.MarkOperatorAsDeletedAsync(SelectedOperator.id);
            await RefreshList();
            SelectedOperator = null;
        }

        private async void AddEditNewRefDataDialog(int OperatorId)
        {
            var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<AddEditOperatorViewModel>();
            dialogViewModel.OperatorId = OperatorId;
            bool? success = _dialogService.ShowDialog<AddEditOperatorView>(this, dialogViewModel);
            if (success == true)
            {
                SelectedOperator = null;
                await RefreshList();
            }
        }

        private async Task RefreshList()
        {
            OperatorList = new List<Operator>(await _operatorRepo.GetOperators());
        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            await RefreshList();
        }

        public void Unload(FrameworkElement element)
        {
            OperatorList = null;
            SelectedOperator = null;
            IsBusy = false;
        }

    }
}
