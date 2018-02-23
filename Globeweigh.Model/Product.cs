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
    
    public partial class Product : INotifyPropertyChanged
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
        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
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
        private Nullable<System.DateTime> _DateLastModified;
        public Nullable<System.DateTime> DateLastModified
        {
            get { return _DateLastModified; }
            set
            {
                if (_DateLastModified != value)
                {
                    _DateLastModified = value;
                    OnPropertyChanged("DateLastModified");
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