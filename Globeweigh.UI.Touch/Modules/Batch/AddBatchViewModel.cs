using System.Collections.Generic;
using System.Linq;
using Globeweigh.UI.Shared;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Services;
using Globeweigh.UI.Touch.Model;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model.Custom;

namespace Globeweigh.UI.Touch
{
    public class AddBatchViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        private readonly IProductRepository _productRepo = SimpleIoc.Default.GetInstance<IProductRepository>();
        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();

        #endregion

        #region Properties

        private int _BatchId;
        public int BatchId
        {
            get { return _BatchId; }
            set { Set(ref _BatchId, value); }
        }

        private Batch _CurrentBatch;
        public Batch CurrentBatch
        {
            get { return _CurrentBatch; }
            set { Set(ref _CurrentBatch, value); }
        }

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

        public RelayCommand SaveCommand => new RelayCommand(OnSave, CanSave);

        #endregion

        public async void MouseUpCommand(string name)
        {
            if (name == "Product")
            {
                var dialogViewModel = SimpleIoc.Default.GetInstance<SelectProductViewModel>();
                dialogViewModel.AllProductList = new List<Product>(ProductList);
                bool? success = _dialogService.ShowDialog<SelectProductView>(this, dialogViewModel);
                if (success == true)
                {
                    CurrentBatch.ProductId = dialogViewModel.SelectedProduct.id;
                    SelectedProduct = dialogViewModel.SelectedProduct;
                }
            }
            else if (name == "BatchNo")
            {
                var dialogViewModel = SimpleIoc.Default.GetInstance<KeyboardViewModel>();
                dialogViewModel.HeaderText = "Enter Batch No";
                bool? success = _dialogService.ShowDialog<KeyboardView>(this, dialogViewModel);
                if (success == true)
                {
                    CurrentBatch.BatchNo = dialogViewModel.KeyboardValue;
                }
            }
            else if (name == "BatchNo2")
            {
                var dialogViewModel = SimpleIoc.Default.GetInstance<KeyboardViewModel>();
                dialogViewModel.HeaderText = "Enter Batch No 2";
                bool? success = _dialogService.ShowDialog<KeyboardView>(this, dialogViewModel);
                if (success == true)
                {
                    CurrentBatch.BatchNo2 = dialogViewModel.KeyboardValue;
                }
            }
        }

        public async void Load(FrameworkElement element)
        {
            ProductList = new List<Product>(await _productRepo.GetProducts());

            if (BatchId == 0)
            {
                CurrentBatch = new Batch();
            }
            else
            {
                CurrentBatch = await _batchRepo.GetBatchAsync(BatchId);
                SelectedProduct = ProductList.FirstOrDefault(a => a.id == CurrentBatch.ProductId);
            }
        }

        private async void OnSave()
        {
            await _batchRepo.AddUpdateBatchAsync(CurrentBatch);
            DialogResult = true;
        }

        private bool CanSave()
        {
            if (CurrentBatch == null) return false;
            if (CurrentBatch.ProductId == 0) return false;
            return true;
        }

        public void Unload(FrameworkElement element)
        {
            CurrentBatch = null;
            DialogResult = null;
            ProductList = null;
        }
    }
}
