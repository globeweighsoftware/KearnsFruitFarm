using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globeweigh.UI.Touch.Model;

namespace Globeweigh.Model
{
    public partial class Batch
    {
        private int? _UpperLimit;
        public int? UpperLimit
        {
            get { return _UpperLimit; }
            set
            {
                if (_UpperLimit != value)
                {
                    _UpperLimit = value;
                    OnPropertyChanged("UpperLimit");
                }
            }
        }

        private int? _LowerLimit;
        public int? LowerLimit
        {
            get { return _LowerLimit; }
            set
            {
                if (_LowerLimit != value)
                {
                    _LowerLimit = value;
                    OnPropertyChanged("LowerLimit");
                }
            }
        }

        private int? _Nominal;
        public int? Nominal
        {
            get { return _Nominal; }
            set
            {
                if (_Nominal != value)
                {
                    _Nominal = value;
                    OnPropertyChanged("Nominal");
                }
            }
        }

        private int? _Tare;
        public int? Tare
        {
            get { return _Tare; }
            set
            {
                if (_Tare != value)
                {
                    _Tare = value;
                    OnPropertyChanged("Tare");
                }
            }
        }
    }
}
