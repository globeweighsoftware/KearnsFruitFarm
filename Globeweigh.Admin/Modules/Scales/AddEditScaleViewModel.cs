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
    public class AddEditScaleViewModel : DialogViewModelBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        private readonly IScaleRepository _ScaleRepo = SimpleIoc.Default.GetInstance<IScaleRepository>();

        #endregion

        #region Properties

        public int ScaleId { get; set; }

        private Scale _CurrentScale;
        public Scale CurrentScale
        {
            get { return _CurrentScale; }
            private set { Set(ref _CurrentScale, value); }
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

            if (ScaleId == 0)
            {
                CurrentScale = new Scale();
                HeaderText = "Add New Scale";
            }
            else
            {
                CurrentScale = await _ScaleRepo.GetScale(ScaleId);
                HeaderText = "Edit Scale";
            }
        }

        private async void OnSave()
        {
            await _ScaleRepo.AddUpdateScaleAsync(CurrentScale);
            DialogResult = true;
        }

        private bool CanSave()
        {
            if (CurrentScale == null) return false;
            if (string.IsNullOrEmpty(CurrentScale.ScaleIpAddress)) return false;
            if (CurrentScale.ScaleNumber <=0) return false;
            return true;
        }

        public void Unload(FrameworkElement element)
        {
            CurrentScale = null;
            ScaleId = 0;
            CodeAlreadyExists = false;
            DialogResult = null;
        }
    }
}
