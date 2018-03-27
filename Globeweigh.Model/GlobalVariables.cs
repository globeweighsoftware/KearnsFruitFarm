using System;
using System.IO;
using Globeweigh.Model;

namespace Globeweigh.Model
{
    public static class GlobalVariables
    {
        public static bool InTestMode { get; set; }
//        public static bool IsDisplayDeviceOverride { get; set; }
        //        public static GlobeweighUser CurrentGlobeweighUser { get; set; }
        public static string CurrentMsiVersion { get; set; }
        public static string ConnectionString { get; set; }
        public static Device CurrentDevice { get; set; }
        public static string ApplicationClickOnceProgramShortcut { get; set; }
        public static string ReleaseBuildDirectory { get; set; }
        public static readonly string CommonApplicationDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Globeweigh\\");


        public static int DeviceId
        {
            get
            {
                if (CurrentDevice != null)
                {
                    return CurrentDevice.id;
                }
                return 0;
            }
        }
    }
}
