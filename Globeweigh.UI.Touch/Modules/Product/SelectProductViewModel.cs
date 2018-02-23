using System;
using System.Collections.Generic;
using System.Linq;
using Globeweigh.UI.Shared;
using Globeweigh.UI.Shared.Services;
using Globeweigh.Model;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model.Custom;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.Native;

namespace Globeweigh.UI.Touch
{
    public class SelectProductViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        private readonly IProductRepository _productRepo = SimpleIoc.Default.GetInstance<IProductRepository>();

        #endregion

        #region Properties

        private List<Product> _AllProductList;
        public List<Product> AllProductList
        {
            get { return _AllProductList; }
            set { Set(ref _AllProductList, value); }
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

        private ObservableCollection<string> _DistinctLetterList;
        public ObservableCollection<string> DistinctLetterList
        {
            get { return _DistinctLetterList; }
            set { Set(ref _DistinctLetterList, value); }
        }

        private string _KeypadDisplayValue;
        public string KeypadDisplayValue
        {
            get { return _KeypadDisplayValue; }
            private set { Set(ref _KeypadDisplayValue, value); }
        }

        #endregion

        #region Commands

        public RelayCommand SelectCommand => new RelayCommand(OnSelect, CanSelect);
        public RelayCommand<string> KeypadCommand => new RelayCommand<string>(OnKeypadCommand);

        #endregion

        private bool CanSelect()
        {
            if (SelectedProduct == null) return false;
            return true;
        }

        private void GetDistinctCodeCharacterList()
        {
            int searchCharCount = 0;
            if (!string.IsNullOrEmpty(KeypadDisplayValue)) searchCharCount = KeypadDisplayValue.Length;
            try
            {
                DistinctLetterList = new ObservableCollection<string>(ProductList.Where(a => a.Description != null).Select(s => s.Description.Substring(searchCharCount, 1)).Distinct().ToList());
            }
            catch (Exception ex)
            {
                DistinctLetterList = new ObservableCollection<string>();
            }
            DistinctLetterList.Add("Clear");

        }

        private void OnKeypadCommand(string param)
        {
            if (param == "Clear")
            {
                KeypadDisplayValue = string.Empty;
                ProductList = new List<Product>(AllProductList);
                GetDistinctCodeCharacterList();
                return;
            }
            KeypadDisplayValue += param;
            FilterProductList();
        }

        private void FilterProductList()
        {
            ProductList = new List<Product>(AllProductList.Where(a => a.Description != null && a.Description.StartsWith(KeypadDisplayValue)));
            GetDistinctCodeCharacterList();
        }

        private async void RefreshProducts()
        {
            AllProductList = new List<Product>(await _productRepo.GetProducts());
            ProductList = new List<Product>(AllProductList);
            GetDistinctCodeCharacterList();
        }

        public async void Load(FrameworkElement element)
        {
            RefreshProducts();
        }
        private async void OnSelect()
        {
            DialogResult = true;
        }

        public void Unload(FrameworkElement element)
        {
            KeypadDisplayValue = string.Empty;
            ProductList = null;
            AllProductList = null;
            SelectedProduct = null;
            DialogResult = null;
            DistinctLetterList.Clear();
        }
    }
}
