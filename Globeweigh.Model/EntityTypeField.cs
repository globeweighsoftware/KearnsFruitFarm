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
    
    public partial class EntityTypeField : INotifyPropertyChanged
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
        private int _EntityTypeId;
        public int EntityTypeId
        {
            get { return _EntityTypeId; }
            set
            {
                if (_EntityTypeId != value)
                {
                    _EntityTypeId = value;
                    OnPropertyChanged("EntityTypeId");
                }
            }
        }
        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
            set
            {
                if (_FieldName != value)
                {
                    _FieldName = value;
                    OnPropertyChanged("FieldName");
                }
            }
        }
        private bool _IsVisible;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }
        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                if (_DisplayName != value)
                {
                    _DisplayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }
        private string _FormatMask;
        public string FormatMask
        {
            get { return _FormatMask; }
            set
            {
                if (_FormatMask != value)
                {
                    _FormatMask = value;
                    OnPropertyChanged("FormatMask");
                }
            }
        }
        private int _SortOrder;
        public int SortOrder
        {
            get { return _SortOrder; }
            set
            {
                if (_SortOrder != value)
                {
                    _SortOrder = value;
                    OnPropertyChanged("SortOrder");
                }
            }
        }
        private bool _IsUsed;
        public bool IsUsed
        {
            get { return _IsUsed; }
            set
            {
                if (_IsUsed != value)
                {
                    _IsUsed = value;
                    OnPropertyChanged("IsUsed");
                }
            }
        }
        private bool _IsRequired;
        public bool IsRequired
        {
            get { return _IsRequired; }
            set
            {
                if (_IsRequired != value)
                {
                    _IsRequired = value;
                    OnPropertyChanged("IsRequired");
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
