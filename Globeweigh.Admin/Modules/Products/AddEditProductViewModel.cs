using System.Collections.Generic;
using Globeweigh.UI.Shared;
using Globeweigh.UI.Shared.Services;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.Model;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Helpers;
using IDialogService = MvvmDialogs.IDialogService;

namespace Globeweigh.Admin
{
    public class AddEditProductViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        private readonly IProductRepository _productRepo = SimpleIoc.Default.GetInstance<IProductRepository>();

        #endregion

        #region Properties

        public int ProductId { get; set; }

        private Product _CurrentProduct;
        public Product CurrentProduct
        {
            get { return _CurrentProduct; }
            private set { Set(ref _CurrentProduct, value); }
        }

        private bool _CodeAlreadyExists;
        public bool CodeAlreadyExists
        {
            get { return _CodeAlreadyExists; }
            set { Set(ref _CodeAlreadyExists, value); }
        }

        #endregion

        #region Commands

        public RelayCommand SaveCommand => new RelayCommand(OnSave, CanSave);

        #endregion

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;

            if (ProductId == 0)
            {
                CurrentProduct = new Product();
                HeaderText = "Add New Product";
            }
            else
            {
                CurrentProduct = await _productRepo.GetProduct(ProductId);
                HeaderText = "Edit Product";
            }
            CurrentProduct.PropertyChanged += CurrentProduct_PropertyChanged;
        }

        private async void CurrentProduct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
//            if (e.PropertyName.Equals("BaseCode"))
//            {
//                CodeAlreadyExists = await _productRepo.DoesBaseCodeExist(CurrentProduct);
//                if (CodeAlreadyExists) return;
//            }
        }

        private async void OnSave()
        {
            await _productRepo.AddUpdateProductAsync(CurrentProduct);
            DialogResult = true;
        }

        private bool CanSave()
        {
            if (CurrentProduct == null) return false;
            if (!UtilitiesShared.IsNumeric(CurrentProduct.LowerLimit)) return false;
            if (!UtilitiesShared.IsNumeric(CurrentProduct.UpperLimit)) return false;
            if (!UtilitiesShared.IsNumeric(CurrentProduct.NominalWeight)) return false;
            if (!UtilitiesShared.IsNumeric(CurrentProduct.Tare)) return false;
            if (CurrentProduct.LowerLimit == null || CurrentProduct.UpperLimit == null ||
                CurrentProduct.NominalWeight == null || CurrentProduct.Tare == null) return false;
            return true;
        }

        public void Unload(FrameworkElement element)
        {
            CurrentProduct.PropertyChanged -= CurrentProduct_PropertyChanged;
            CurrentProduct = null;
            ProductId = 0;
            CodeAlreadyExists = false;
            DialogResult = null;
        }
    }
}
