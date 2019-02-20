using System;
using System.Diagnostics;

namespace Globeweigh.Model
{
    public partial class spOperatorStats_Result
    {
        public decimal TotalWeightKg
        {
            get
            {
                if (TotalWeightGrams == 0) return 0;
                return  ((decimal)TotalWeightGrams / 1000);
            }
        }

        public decimal TotalGiveawayKg
        {
            get
            {
                if (TotalGiveawayKg == 0) return 0;
                return (decimal)(TotalGiveawayKg / 1000);
            }
        }


        public decimal PacksPerMin
        {
            get
            {
                if (TotalTimeTicks == 0) return 0;
                if (PortionCount == 0) return 0;

                double minutes = TimeSpan.FromTicks((long)TotalTimeTicks).TotalMinutes;

//                var timeElapsed = new TimeSpan((long)TotalTimeTicks);


                if (minutes == 0) return 0;
                return ((decimal)(PortionCount / minutes));
            }
        }

        public string TotalTimeDisplay
        {
            get
            {
                if (TotalTimeTicks == null) return "";
                var timeElapsed = new TimeSpan((long) TotalTimeTicks);
                return ((int)timeElapsed.TotalHours).ToString("D2") + ":" + timeElapsed.Minutes.ToString("00") + ":" + timeElapsed.Seconds.ToString("00");
            }
        }
    }
}
