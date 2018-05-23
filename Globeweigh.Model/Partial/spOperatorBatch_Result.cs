using System;
using System.Diagnostics;

namespace Globeweigh.Model
{
    public partial class spOperatorBatch_Result
    {
//        public string FullName
//        {
//            get
//            {
//                return FirstName + " " + LastName;
//            }
//        }


        public decimal PacksPerMin
        {
            get
            {
                if (TimeElapsedTicks == 0) return 0;
                if (PortionCount == 0) return 0;

                double seconds = TimeSpan.FromTicks((long) TimeElapsedTicks).TotalSeconds;
                if (seconds == 0) return 0;
                return ((decimal)(PortionCount / seconds) * 60);


                return -1;
            }
        }

        public string TimeElapsedDisplay
        {
            get
            {
                if (TimeElapsedTicks == null) return "";


                var timeElapsed = new TimeSpan(TimeElapsedTicks);


//                return timeElapsed;

//                return TimeSpan.FromTicks((long) TimeElapsedTicks).ToString();
//                if (timeElapsed == null) return "";
//                int days = 0;
//                if (timeElapsed.Day > 1)
//                    days = ((DateTime)TimeElapsed).Day - 1;

//                var timeSpan = new TimeSpan(days, ((DateTime)TimeElapsed).Hour, ((DateTime)TimeElapsed).Minute, ((DateTime)TimeElapsed).Second);
                return ((int)timeElapsed.TotalHours).ToString("D2") + ":" + timeElapsed.Minutes.ToString("00") + ":" + timeElapsed.Seconds.ToString("00");
            }
        }
    }
}
