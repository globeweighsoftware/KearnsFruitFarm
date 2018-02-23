using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globeweigh.UI.Touch.Model;

namespace Globeweigh.Model
{
    public partial class vwBatchView
    {
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public int Band
        {
            get { return (int) (UpperLimit - NominalWeight); }
        }

        public int BatchUpperLimit
        {
            get
            {
                if (Override)
                    return (int) UpperLimitOverride;
                return (int)UpperLimit;
            }
        }

        public int BatchLowerLimit
        {
            get
            {
                if (Override)
                    return (int)LowerLimitOverride;
                return (int)LowerLimit;
            }
        }

//        public int BatchTare
//        {
//            get
//            {
//                if (Override)
//                    return (int)TareOverride;
//                return (int)Tare;
//            }
//        }
//
//        public int BatchNominal
//        {
//            get
//            {
//                if (Override)
//                    return (int)NominalOverride;
//                return (int)NominalWeight;
//            }
//        }

        private string _TimeElapsedDisplay;
        public string TimeElapsedDisplay
        {
            get { return _TimeElapsedDisplay; }
            set
            {
                if (_TimeElapsedDisplay != value)
                {
                    _TimeElapsedDisplay = value;
                    OnPropertyChanged("TimeElapsedDisplay");
                }
            }
        }
    }
}
