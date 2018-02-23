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
    public class AddEditOperatorViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        private readonly IOperatorRepository _OperatorRepo = SimpleIoc.Default.GetInstance<IOperatorRepository>();

        #endregion

        #region Properties

        public int OperatorId { get; set; }

        private Operator _CurrentOperator;
        public Operator CurrentOperator
        {
            get { return _CurrentOperator; }
            private set { Set(ref _CurrentOperator, value); }
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

            if (OperatorId == 0)
            {
                CurrentOperator = new Operator();
                HeaderText = "Add New Operator";
            }
            else
            {
                CurrentOperator = await _OperatorRepo.GetOperator(OperatorId);
                HeaderText = "Edit Operator";
            }
        }

        private async void OnSave()
        {
            await _OperatorRepo.AddUpdateOperatorAsync(CurrentOperator);
            DialogResult = true;
        }

        private bool CanSave()
        {
            if (CurrentOperator == null) return false;
            if (string.IsNullOrEmpty(CurrentOperator.FirstName)) return false;
            if (string.IsNullOrEmpty(CurrentOperator.LastName)) return false;
            return true;
        }

        public void Unload(FrameworkElement element)
        {
            CurrentOperator = null;
            OperatorId = 0;
            CodeAlreadyExists = false;
            DialogResult = null;
        }
    }
}
