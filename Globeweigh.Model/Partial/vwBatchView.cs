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

        public int TotalWeightKg
        {
            get
            {
                if (TotalWeight == null) return 0;
                if (TotalWeight == 0) return 0;
                else return (int) (TotalWeight / 1000);
            }
        }


        public int AverageGiveaway
        {
            get
            {
                if (AverageWeight == null) return 0;
                if (AverageWeight == 0) return 0;
                if (NominalWeight == null) return 0;
                if (NominalWeight == 0) return 0;
                return (int) (AverageWeight - NominalWeight);
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
