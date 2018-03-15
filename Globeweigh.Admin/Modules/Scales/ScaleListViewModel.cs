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
    public class ScaleListViewModel : AdminViewModelBase, IViewModel
    {
        #region private fields

        private readonly IScaleRepository _ScaleRepo = SimpleIoc.Default.GetInstance<IScaleRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

        private List<Scale> _ScaleList;
        public List<Scale> ScaleList
        {
            get { return _ScaleList; }
            set { Set(ref _ScaleList, value); }
        }

        private Scale _SelectedScale;
        public Scale SelectedScale
        {
            get { return _SelectedScale; }
            set { Set(ref _SelectedScale, value); }
        }


        #endregion

        #region Commands

        public RelayCommand AddCommand => new RelayCommand(OnAdd);
        public RelayCommand EditCommand => new RelayCommand(OnEdit);
        public RelayCommand DeleteCommand => new RelayCommand(OnDelete);
        public RelayCommand RefreshCommand => new RelayCommand(OnRefresh);




        #endregion


        private void OnAdd()
        {
            AddEditNewRefDataDialog(0);
        }
        private void OnEdit()
        {
            AddEditNewRefDataDialog(SelectedScale.id);
        }

        private async void OnRefresh()
        {
            await RefreshList();
        }

        private async void OnDelete()
        {
            var result = _dialogService.ShowMessageBox(this, "Delete Item?", "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel) return;
            await _ScaleRepo.DeletedScaleAsync(SelectedScale.id);
            await RefreshList();
            SelectedScale = null;
        }

        private async void AddEditNewRefDataDialog(int ScaleId)
        {
            var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<AddEditScaleViewModel>();
            dialogViewModel.ScaleId = ScaleId;
            bool? success = _dialogService.ShowDialog<AddEditScaleView>(this, dialogViewModel);
            if (success == true)
            {
                SelectedScale = null;
                await RefreshList();
            }
        }

        private async Task RefreshList()
        {
            ScaleList = new List<Scale>(await _ScaleRepo.GetAllScales());
        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            await RefreshList();
        }

        public void Unload(FrameworkElement element)
        {
            ScaleList = null;
            SelectedScale = null;
            IsBusy = false;
        }

    }
}
