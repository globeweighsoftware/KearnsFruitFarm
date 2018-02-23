using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globeweigh.UI.Touch.Model;

namespace Globeweigh.Model
{
    public partial class Scale
    {
        public NetConnection TcpConnection { get; set; }

        private bool _IsConnected;
        public bool IsConnected
        {
            get { return _IsConnected; }
            set
            {
                if (_IsConnected != value)
                {
                    _IsConnected = value;
                    OnPropertyChanged("IsConnected");
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

        private long _TimeElapsedTicks;
        public long TimeElapsedTicks
        {
            get { return _TimeElapsedTicks; }
            set
            {
                if (_TimeElapsedTicks != value)
                {
                    _TimeElapsedTicks = value;
                    OnPropertyChanged("TimeElapsed");
                    OnPropertyChanged("UserPacksPerMin");
                    OnPropertyChanged("TimeElapsedDisplay");
                }
            }
        }

        public string TimeElapsedDisplay
        {
            get
            {
                return TimeSpan.FromTicks(TimeElapsedTicks).ToString();
            }
        }

        public decimal UserPacksPerMin
        {
            get
            {
                if (TimeElapsedTicks == null) return 0;
                if (UserPackCount == 0) return 0;
                double seconds = TimeSpan.FromTicks(TimeElapsedTicks).TotalSeconds;
                if (seconds == 0) return UserPackCount;
                return ((decimal)(UserPackCount/ seconds)*60);
            }
        }

        private int _UserPackCount;
        public int UserPackCount
        {
            get { return _UserPackCount; }
            set
            {
                if (_UserPackCount != value)
                {
                    _UserPackCount = value;
                    OnPropertyChanged("UserPackCount");
                    OnPropertyChanged("UserPacksPerMin");
                }
            }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (_IsBusy != value)
                {
                    _IsBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        public bool IsVisible { get; set; }
        public bool OperatorChanged { get; set; }


    }
}
