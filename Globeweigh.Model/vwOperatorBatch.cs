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
    
    public partial class vwOperatorBatch : INotifyPropertyChanged
    {
        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set
            {
                if (_FullName != value)
                {
                    _FullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }
        private Nullable<long> _TimeElapsedTicks;
        public Nullable<long> TimeElapsedTicks
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
        private Nullable<int> _TimeElapsedId;
        public Nullable<int> TimeElapsedId
        {
            get { return _TimeElapsedId; }
            set
            {
                if (_TimeElapsedId != value)
                {
                    _TimeElapsedId = value;
                    OnPropertyChanged("TimeElapsedId");
                }
            }
        }
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
        private Nullable<int> _BatchId;
        public Nullable<int> BatchId
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
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }
        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
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
