using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globeweigh.UI.Shared.Helpers
{
    public class StopWatchWithOffset
    {
        private Stopwatch _stopwatch = null;
        TimeSpan _offsetTimeSpan;

        public StopWatchWithOffset(TimeSpan offsetElapsedTimeSpan)
        {
            _offsetTimeSpan = offsetElapsedTimeSpan;
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public bool IsRunning => _stopwatch.IsRunning;

//        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public TimeSpan ElapsedTimeSpan
        {
            get
            {
                return _stopwatch.Elapsed + _offsetTimeSpan;
            }
            set
            {
                _offsetTimeSpan = value;
            }
        }
    }
}
