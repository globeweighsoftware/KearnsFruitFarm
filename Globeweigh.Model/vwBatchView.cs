//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Globeweigh.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class vwBatchView : INotifyPropertyChanged
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("id");
                }
            }
        }
        private int _ProductId;
        public int ProductId
        {
            get { return _ProductId; }
            set
            {
                if (_ProductId != value)
                {
                    _ProductId = value;
                    OnPropertyChanged("ProductId");
                }
            }
        }
        private bool _Closed;
        public bool Closed
        {
            get { return _Closed; }
            set
            {
                if (_Closed != value)
                {
                    _Closed = value;
                    OnPropertyChanged("Closed");
                }
            }
        }
        private System.DateTime _DateCreated;
        public System.DateTime DateCreated
        {
            get { return _DateCreated; }
            set
            {
                if (_DateCreated != value)
                {
                    _DateCreated = value;
                    OnPropertyChanged("DateCreated");
                }
            }
        }
        private bool _Deleted;
        public bool Deleted
        {
            get { return _Deleted; }
            set
            {
                if (_Deleted != value)
                {
                    _Deleted = value;
                    OnPropertyChanged("Deleted");
                }
            }
        }
        private Nullable<int> _PortionCount;
        public Nullable<int> PortionCount
        {
            get { return _PortionCount; }
            set
            {
                if (_PortionCount != value)
                {
                    _PortionCount = value;
                    OnPropertyChanged("PortionCount");
                }
            }
        }
        private Nullable<System.DateTime> _DateClosed;
        public Nullable<System.DateTime> DateClosed
        {
            get { return _DateClosed; }
            set
            {
                if (_DateClosed != value)
                {
                    _DateClosed = value;
                    OnPropertyChanged("DateClosed");
                }
            }
        }
        private long _TimeElapsedTicks;
        public long TimeElapsedTicks
        {
            get { return _TimeElapsedTicks; }
            set
            {
                if (_TimeElapsedTicks != value)
                {
                    _TimeElapsedTicks = value;
                    OnPropertyChanged("TimeElapsedTicks");
                }
            }
        }
        private Nullable<int> _NominalOverride;
        public Nullable<int> NominalOverride
        {
            get { return _NominalOverride; }
            set
            {
                if (_NominalOverride != value)
                {
                    _NominalOverride = value;
                    OnPropertyChanged("NominalOverride");
                }
            }
        }
        private Nullable<int> _UpperLimitOverride;
        public Nullable<int> UpperLimitOverride
        {
            get { return _UpperLimitOverride; }
            set
            {
                if (_UpperLimitOverride != value)
                {
                    _UpperLimitOverride = value;
                    OnPropertyChanged("UpperLimitOverride");
                }
            }
        }
        private Nullable<int> _LowerLimitOverride;
        public Nullable<int> LowerLimitOverride
        {
            get { return _LowerLimitOverride; }
            set
            {
                if (_LowerLimitOverride != value)
                {
                    _LowerLimitOverride = value;
                    OnPropertyChanged("LowerLimitOverride");
                }
            }
        }
        private bool _Override;
        public bool Override
        {
            get { return _Override; }
            set
            {
                if (_Override != value)
                {
                    _Override = value;
                    OnPropertyChanged("Override");
                }
            }
        }
        private Nullable<int> _TareOverride;
        public Nullable<int> TareOverride
        {
            get { return _TareOverride; }
            set
            {
                if (_TareOverride != value)
                {
                    _TareOverride = value;
                    OnPropertyChanged("TareOverride");
                }
            }
        }
        private string _ProductName;
        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                if (_ProductName != value)
                {
                    _ProductName = value;
                    OnPropertyChanged("ProductName");
                }
            }
        }
        private Nullable<int> _LowerLimit;
        public Nullable<int> LowerLimit
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
        private Nullable<int> _UpperLimit;
        public Nullable<int> UpperLimit
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
        private Nullable<int> _Tare;
        public Nullable<int> Tare
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
        private Nullable<int> _NominalWeight;
        public Nullable<int> NominalWeight
        {
            get { return _NominalWeight; }
            set
            {
                if (_NominalWeight != value)
                {
                    _NominalWeight = value;
                    OnPropertyChanged("NominalWeight");
                }
            }
        }
        private Nullable<int> _AveragePacksPerminute;
        public Nullable<int> AveragePacksPerminute
        {
            get { return _AveragePacksPerminute; }
            set
            {
                if (_AveragePacksPerminute != value)
                {
                    _AveragePacksPerminute = value;
                    OnPropertyChanged("AveragePacksPerminute");
                }
            }
        }
    
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
    
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion
    }
}
