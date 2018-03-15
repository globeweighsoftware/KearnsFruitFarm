﻿using System;
using Globeweigh.Model.Custom;
using System.IO;
using System.Xml.Serialization;

namespace Globeweigh.Model.Helpers
{
    public static class Utilities
    {
        public static readonly string ConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Globeweigh\\globeweigh.config");
        public static bool IsMyMachine => Environment.MachineName == "NEIL-DELLXPS";


        public static GlobeweighConfig GetConfigData()
        {
            if (IsMyMachine)
            {
                if (File.Exists(ConfigFilename))
                {
                    File.Delete(ConfigFilename);
                }
            }

            if (!File.Exists(ConfigFilename))
            {
                var newConfig = new GlobeweighConfig();
                newConfig.server = @"(local)\SQLEXPRESS";
                newConfig.database = "GbKearnsFruitFarm";
                newConfig.user = "sa";
                newConfig.password = "ButternutSquash09";
                newConfig.ReleaseBuildDirectory = @"C:\Globeweigh\Releases";
                CreateCommonApplicationDataConfigFile(newConfig);

                return newConfig;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GlobeweighConfig));
                using (FileStream fs = new FileStream(ConfigFilename, FileMode.Open, FileAccess.Read))
                {
                    var config = (GlobeweighConfig)serializer.Deserialize(fs);
                    fs.Close();
                    return config;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void CreateCommonApplicationDataConfigFile(GlobeweighConfig config)
        {
            try
            {
                var folderpath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Globeweigh");
                if (!Directory.Exists(folderpath))
                    Directory.CreateDirectory(folderpath);


                XmlSerializer serializer = new XmlSerializer(typeof(GlobeweighConfig));
                using (TextWriter writer = new StreamWriter(ConfigFilename))
                {
                    serializer.Serialize(writer, config);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
