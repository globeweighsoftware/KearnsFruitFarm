using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Grid;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.Admin.Helpers;
using Globeweigh.UI.Shared;

namespace Globeweigh.Admin
{
    public class DialogViewModelBase : ViewModelBase
    {
        private string _HeaderText;
        public virtual string HeaderText
        {
            get { return _HeaderText; }
            set { Set(ref _HeaderText, value); }
        }

        private bool? _DialogResult;
        public virtual bool? DialogResult
        {
            get => _DialogResult;
            set => Set(ref _DialogResult, value);
        }

        public RelayCommand _CancelCommand;
        public virtual RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand(()=>
                {
                    DialogResult = false;
                }));
            }
        }
    }
}
