using System;
using System.Globalization;
using Globeweigh.UI.Shared;
using Globeweigh.Model;
using MvvmDialogs;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace Globeweigh.UI.Touch
{
    public class KeypadViewModel : BindableBase, IModalDialogViewModel, IViewModel
    {
        #region private fields

        #endregion

        #region Properties

        public bool IsPassword { get; set; }

        private bool? _DialogResult;
        public bool? DialogResult
        {
            get { return _DialogResult; }
            private set { Set(ref _DialogResult, value); }
        }

        private string _KeypadValue;
        public string KeypadValue
        {
            get { return _KeypadValue; }
            private set
            {
                Set(ref _KeypadValue, value);
                if (IsPassword)
                {
                    int charCount = KeypadValue.Length;
                    KeypadDisplayValue = "";
                    for (var i = 0; i < charCount; i++)
                    {
                        KeypadDisplayValue += "*";
                    }
                }
                else
                {
                    KeypadDisplayValue = KeypadValue;
                }
            }
        }

        private string _TimeValue;
        public string TimeValue
        {
            get { return _TimeValue; }
            private set
            {
                Set(ref _TimeValue, value);
                TimeDisplayValue = TimeValue;
            }
        }

        private string _KeypadDisplayValue;
        public string KeypadDisplayValue
        {
            get { return _KeypadDisplayValue; }
            set { Set(ref _KeypadDisplayValue, value); }
        }

        private string _TimeDisplayValue;
        public string TimeDisplayValue
        {
            get { return _TimeDisplayValue; }
            set { Set(ref _TimeDisplayValue, value); }
        }

        private string _HeaderText;
        public string HeaderText
        {
            get { return _HeaderText; }
            set { Set(ref _HeaderText, value); }
        }

        private bool _IsDecimalPointVisible;
        public bool IsDecimalPointVisible
        {
            get { return _IsDecimalPointVisible; }
            set { Set(ref _IsDecimalPointVisible, value); }
        }

        private bool _IsTimePicker;
        public bool IsTimePicker
        {
            get { return _IsTimePicker; }
            set { Set(ref _IsTimePicker, value); }
        }

        private bool _ShowMinus;
        public bool ShowMinus
        {
            get { return _ShowMinus; }
            set { Set(ref _ShowMinus, value); }
        }

        private int _MaxValue;
        public int MaxValue
        {
            get { return _MaxValue; }
            set { Set(ref _MaxValue, value); }
        }



        #endregion

        #region Commands

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<string> KeypadCommand { get; private set; }

        #endregion

        public KeypadViewModel()
        {
            OkCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel);
            KeypadCommand = new RelayCommand<string>(OnKeypadCommand);
        }

        private bool CanSave()
        {
            if (IsTimePicker)
            {
                if (TimeValue?.Length != 5) return false;
                DateTime dt;
                if (!DateTime.TryParseExact(TimeValue, "HH:mm", CultureInfo.InvariantCulture,
                                                              DateTimeStyles.None, out dt))
                {
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(KeypadValue)) return false;
                if (MaxValue > 0)
                {
                    if (Convert.ToInt32(KeypadValue) > MaxValue) return false;
                }
            }
            return true;
        }

        private void OnKeypadCommand(string param)
        {
            if (IsTimePicker)
            {
                if (param == "Clear")
                {
                    TimeValue = string.Empty;
                    return;
                }
                else if (param == "Del")
                {
                    if (TimeValue == null) return;
                    if (TimeValue.Length == 0) return;
                    if (TimeValue.Length == 4) TimeValue = TimeValue.Substring(0, TimeValue.Length - 2);
                    else TimeValue = TimeValue.Substring(0, TimeValue.Length - 1);
                    return;
                }
                else
                {
                    if (TimeValue?.Length == 2) TimeValue += ":";
                    if (TimeValue != null && TimeValue.Length == 5) return;
                    TimeValue += param;
                }
            }
            else
            {
                if (param == "Clear")
                {
                    KeypadValue = string.Empty;
                    return;
                }
                if (param == "Del")
                {
                    if (KeypadValue == null) return;
                    if (KeypadValue.Length == 0) return;
                    KeypadValue = KeypadValue.Substring(0, KeypadValue.Length - 1);
                    return;
                }
                if (param == "-")
                {
                    if (!string.IsNullOrEmpty(KeypadValue)) return;
                }
                else if (param == ".")
                {
                    if (string.IsNullOrEmpty(KeypadValue)) return;
                    if (KeypadValue.Contains(".")) return;
                }
                KeypadValue += param;
            }

        }

        public async void Load(FrameworkElement element)
        {
            DialogResult = null;
        }

        private async void OnSave()
        {
            if (KeypadValue == string.Empty)
            {
                KeypadValue = "0";
            }
            DialogResult = true;
        }

        private void OnCancel()
        {
            DialogResult = false;
        }

        public void Unload(FrameworkElement element)
        {
            KeypadValue = string.Empty;
            DialogResult = null;
            MaxValue = 0;
        }
    }
}
