using Globeweigh.UI.Shared;
using Globeweigh.Model;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace Globeweigh.UI.Touch
{
    public class OkCancelViewModel : BindableBase, IModalDialogViewModel, IViewModel
    {
        #region Properties

        private string _MessageText;
        public string MessageText
        {
            get { return _MessageText; }
            set { Set(ref _MessageText, value); }
        }

        private bool? _DialogResult;
        public bool? DialogResult
        {
            get { return _DialogResult; }
            set { Set(ref _DialogResult, value); }
        }

        private bool _HideCancel;
        public bool HideCancel
        {
            get => _HideCancel;
            set => Set(ref _HideCancel, value);
        }

        private string _HeaderText;
        public string HeaderText
        {
            get { return _HeaderText; }
            set { Set(ref _HeaderText, value); }
        }

        private string _MainText;
        public string MainText
        {
            get { return _MainText; }
            set { Set(ref _MainText, value); }
        }

        private string _OkText;
        public string OkText
        {
            get { return _OkText; }
            set { Set(ref _OkText, value); }
        }

        #endregion

        #region Commands

        public RelayCommand OkCommand => new RelayCommand(()=>DialogResult = true);
        public RelayCommand CancelCommand => new RelayCommand(() => DialogResult = false);

        #endregion

        public async void Load(FrameworkElement element)
        {
        }

        public void Unload(FrameworkElement element)
        {
            HideCancel = false;
            OkText = null;
            HeaderText = null;
            MainText = null;
            DialogResult = null;
        }
    }
}
