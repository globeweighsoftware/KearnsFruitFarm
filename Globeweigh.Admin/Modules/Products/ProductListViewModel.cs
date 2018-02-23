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
    public class ProductListViewModel : AdminViewModelBase, IViewModel
    {
        #region private fields

        private readonly IProductRepository _productRepo = SimpleIoc.Default.GetInstance<IProductRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

        private List<Product> _ProductList;
        public List<Product> ProductList
        {
            get { return _ProductList; }
            set { Set(ref _ProductList, value); }
        }

        private Product _SelectedProduct;
        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set { Set(ref _SelectedProduct, value); }
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
            AddEditNewRefDataDialog(SelectedProduct.id);
        }

        private async void OnRefresh()
        {
            await RefreshList();
        }

        private async void OnDelete()
        {
            var result = _dialogService.ShowMessageBox(this, "Delete Item?", "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel) return;
            await _productRepo.MarkProductAsDeletedAsync(SelectedProduct.id);
            await RefreshList();
            SelectedProduct = null;
        }

        private async void AddEditNewRefDataDialog(int productId)
        {
            var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<AddEditProductViewModel>();
            dialogViewModel.ProductId = productId;
            bool? success = _dialogService.ShowDialog<AddEditProductView>(this, dialogViewModel);
            if (success == true)
            {
                SelectedProduct = null;
                await RefreshList();
            }
        }

        private async Task RefreshList()
        {
            ProductList = new List<Product>(await _productRepo.GetProducts());
        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            await RefreshList();
        }

        public void Unload(FrameworkElement element)
        {
            ProductList = null;
            SelectedProduct = null;
            IsBusy = false;
        }

    }
}
