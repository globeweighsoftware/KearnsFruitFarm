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
    public class SelectOperatorViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        private readonly IOperatorRepository _OperatorRepo = SimpleIoc.Default.GetInstance<IOperatorRepository>();

        #endregion

        #region Properties

        private List<vwOperatorBatch> _AllOperatorList;
        public List<vwOperatorBatch> AllOperatorList
        {
            get { return _AllOperatorList; }
            set { Set(ref _AllOperatorList, value); }
        }

        private List<vwOperatorBatch> _OperatorList;
        public List<vwOperatorBatch> OperatorList
        {
            get { return _OperatorList; }
            set { Set(ref _OperatorList, value); }
        }

        private vwOperatorBatch _SelectedOperator;
        public vwOperatorBatch SelectedOperator
        {
            get { return _SelectedOperator; }
            set { Set(ref _SelectedOperator, value); }
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
            if (SelectedOperator == null) return false;
            return true;
        }

        private void GetDistinctCodeCharacterList()
        {
            int searchCharCount = 0;
            if (!string.IsNullOrEmpty(KeypadDisplayValue)) searchCharCount = KeypadDisplayValue.Length;
            try
            {
                DistinctLetterList = new ObservableCollection<string>(OperatorList.Where(a => a.LastName != null).Select(s => s.LastName.Substring(searchCharCount, 1)).Distinct().ToList());
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
                OperatorList = new List<vwOperatorBatch>(AllOperatorList);
                GetDistinctCodeCharacterList();
                return;
            }
            KeypadDisplayValue += param;
            FilterOperatorList();
        }

        private void FilterOperatorList()
        {
            OperatorList = new List<vwOperatorBatch>(AllOperatorList.Where(a => a.LastName != null && a.LastName.StartsWith(KeypadDisplayValue)));
            GetDistinctCodeCharacterList();
        }

        private async void RefreshOperators()
        {
//            AllOperatorList = new List<vwOperatorBatch>(await _OperatorRepo.GetOperators());
            OperatorList = new List<vwOperatorBatch>(AllOperatorList);
            GetDistinctCodeCharacterList();
        }

        public async void Load(FrameworkElement element)
        {
            RefreshOperators();
        }
        private async void OnSelect()
        {
            DialogResult = true;
        }

        public void Unload(FrameworkElement element)
        {
            KeypadDisplayValue = string.Empty;
            OperatorList = null;
            AllOperatorList = null;
            SelectedOperator = null;
            DialogResult = null;
            DistinctLetterList.Clear();
        }
    }
}
