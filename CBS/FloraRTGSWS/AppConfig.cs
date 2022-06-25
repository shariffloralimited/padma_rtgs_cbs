using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace FloraRTGSWS
{
    public static class AppConfig
    {
        public static string CBSURL  = ConfigurationManager.AppSettings["CBSURL"];
        public static string LogPath = ConfigurationManager.AppSettings["logpath"];
    }


}