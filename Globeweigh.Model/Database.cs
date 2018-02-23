using Globeweigh.Model.Custom;
using System;
using Globeweigh.Model.Helpers;
using System.IO;

namespace Globeweigh.Model
{
    public static class Database
    {
        public static void SetDatabaseConnectionString()
        {
            GlobeweighConfig cirrusConfig = Utilities.GetConfigData();
            const string defaultConnectionString = "metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlClient;provider connection string=';data source={0};initial catalog={1};user id={2};password={3};multipleactiveresultsets=True;App=EntityFramework';";
            var connectionString = String.Format(defaultConnectionString, cirrusConfig.server, cirrusConfig.database, cirrusConfig.user, cirrusConfig.password);

            GlobalVariables.ConnectionString = connectionString;
            GlobalVariables.OperaConnectionString = @"Provider=vfpoledb;Data Source=" + cirrusConfig.OperaConnectionString + ";Extended Properties=dBASE III;";

            if (Utilities.IsMyMachine)
            {
                GlobalVariables.OperaConnectionString =@"Prov]ider=vfpoledb;Data Source=C:\Globeweigh\KearnsFruitFarm\OperaData;Extended Properties=dBASE III;";
                GlobalVariables.ConnectionString = GetLOCALConnectionString();
            }
        }

        public static string GetLOCALConnectionString()
        {
            GlobalVariables.InTestMode = true;
            const string defaultLocalConnectionString = "metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlClient;provider connection string=';data source={0};initial catalog={1};integrated security=True;MultipleActiveResultSets=True;App=EntityFramework';";
            var cirrusConfig = new GlobeweighConfig();
            cirrusConfig.server = @"(local)\SQLEXPRESS";
            cirrusConfig.database = "GbKearnsFruitFarm";
            var connectionString = String.Format(defaultLocalConnectionString, cirrusConfig.server, cirrusConfig.database);
            return connectionString;
        }

        public static bool SetAndTestDbConnection()
        {
            SetDatabaseConnectionString();

            using (var db = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var conn = db.Database.Connection;
                try
                {
                    conn.Open();   // check the database connection
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
