using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Globeweigh.UI.Shared
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { Set(ref _IsBusy, value); }
        }

        protected virtual void Set<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property.Name));
        }
    }
}
