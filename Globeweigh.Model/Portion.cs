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
    
    public partial class Portion : INotifyPropertyChanged
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
        private int _BatchId;
        public int BatchId
        {
            get { return _BatchId; }
            set
            {
                if (_BatchId != value)
                {
                    _BatchId = value;
                    OnPropertyChanged("BatchId");
                }
            }
        }
        private int _ScaleNo;
        public int ScaleNo
        {
            get { return _ScaleNo; }
            set
            {
                if (_ScaleNo != value)
                {
                    _ScaleNo = value;
                    OnPropertyChanged("ScaleNo");
                }
            }
        }
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
        private int _Weight;
        public int Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged("Weight");
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
        private Nullable<System.DateTime> _DateDeleted;
        public Nullable<System.DateTime> DateDeleted
        {
            get { return _DateDeleted; }
            set
            {
                if (_DateDeleted != value)
                {
                    _DateDeleted = value;
                    OnPropertyChanged("DateDeleted");
                }
            }
        }
        private int _ScaleTare;
        public int ScaleTare
        {
            get { return _ScaleTare; }
            set
            {
                if (_ScaleTare != value)
                {
                    _ScaleTare = value;
                    OnPropertyChanged("ScaleTare");
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
