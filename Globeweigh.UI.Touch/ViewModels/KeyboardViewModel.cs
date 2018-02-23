using Globeweigh.UI.Shared;
using Globeweigh.Model;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using WindowsInput;
using Globeweigh.Shared.UI.TouchScreenKeyboard;

namespace Globeweigh.UI.Touch
{
    public class KeyboardViewModel : BindableBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        #endregion

        #region Properties

        private bool? _DialogResult;
        public bool? DialogResult
        {
            get { return _DialogResult; }
            private set { Set(ref _DialogResult, value); }
        }

        private string _KeyboardValue;
        public string KeyboardValue
        {
            get { return _KeyboardValue; }
            set { Set(ref _KeyboardValue, value); }
        }

        private string _HeaderText;
        public string HeaderText
        {
            get { return _HeaderText; }
            set { Set(ref _HeaderText, value); }
        }

        #endregion

        #region Commands

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<string> KeypadCommand { get; private set; }

        #endregion

        public KeyboardViewModel()
        {
            OkCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private bool CanSave()
        {
            if (KeyboardValue == string.Empty) return false;
            return true;
        }

        public async void Load(FrameworkElement element)
        {
            if (KeyboardValue != null)
            {
                var vm = (KeyboardView)element;
                vm.textcontrol.CaretIndex = KeyboardValue.Length;
            }

        }

        private async void OnSave()
        {
            DialogResult = true;
        }

        private void OnCancel()
        {
            DialogResult = false;
        }

        //        private static IInputSimulator InputSimulator
        //        {
        //            get;
        //            set;
        //        }

        public void Unload(FrameworkElement element)
        {
            DialogResult = null;
            KeyboardValue = null;
            HeaderText = null;
            //            InputSimulator = InputSimulatorScope.Current;
            //            var isCapsLockOn = InputSimulator.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
            //            if (isCapsLockOn) InputSimulator.SimulateKeyPress(VirtualKeyCode.CAPITAL);
        }
    }
}
