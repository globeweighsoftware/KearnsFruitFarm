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
    using System.ComponentModel;
    
    public partial class spOperatorStats_Result : INotifyPropertyChanged
    {
        private int _OperatorId;
        public int OperatorId
        {
            get { return _OperatorId; }
            set
            {
                if (_OperatorId != value)
                {
                    _OperatorId = value;
                    OnPropertyChanged("OperatorId");
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
        private Nullable<long> _TotalTimeTicks;
        public Nullable<long> TotalTimeTicks
        {
            get { return _TotalTimeTicks; }
            set
            {
                if (_TotalTimeTicks != value)
                {
                    _TotalTimeTicks = value;
                    OnPropertyChanged("TotalTimeTicks");
                }
            }
        }
        private Nullable<int> _TotalWeightGrams;
        public Nullable<int> TotalWeightGrams
        {
            get { return _TotalWeightGrams; }
            set
            {
                if (_TotalWeightGrams != value)
                {
                    _TotalWeightGrams = value;
                    OnPropertyChanged("TotalWeightGrams");
                }
            }
        }
        private Nullable<int> _TotalGiveawayGrams;
        public Nullable<int> TotalGiveawayGrams
        {
            get { return _TotalGiveawayGrams; }
            set
            {
                if (_TotalGiveawayGrams != value)
                {
                    _TotalGiveawayGrams = value;
                    OnPropertyChanged("TotalGiveawayGrams");
                }
            }
        }
        private string _OperatorName;
        public string OperatorName
        {
            get { return _OperatorName; }
            set
            {
                if (_OperatorName != value)
                {
                    _OperatorName = value;
                    OnPropertyChanged("OperatorName");
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