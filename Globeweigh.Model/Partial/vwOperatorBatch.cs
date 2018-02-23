using System;

namespace Globeweigh.Model
{
    public partial class vwOperatorBatch
    {
        public decimal PacksPerMin
        {
            get
            {
                if (TimeElapsedTicks == 0) return 0;
                if (PortionCount == 0) return 0;
                double seconds = TimeSpan.FromTicks(TimeElapsedTicks).TotalSeconds;
                if (seconds == 0) return 0;
                return ((decimal)(PortionCount / seconds) * 60);
            }
        }

        public string TimeElapsedDisplay
        {
            get
            {
                return TimeSpan.FromTicks(TimeElapsedTicks).ToString();
//                if (TimeElapsed == null) return "";
//                int days = 0;
//                if (((DateTime)TimeElapsed).Day > 1)
//                    days = ((DateTime)TimeElapsed).Day - 1;
//
//                var timeSpan = new TimeSpan(days, ((DateTime)TimeElapsed).Hour, ((DateTime)TimeElapsed).Minute, ((DateTime)TimeElapsed).Second);
//                return ((int)timeSpan.TotalHours).ToString("D2") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
            }
        }
    }
}
