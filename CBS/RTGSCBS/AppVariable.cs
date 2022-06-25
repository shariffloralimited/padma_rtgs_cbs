using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace RTGSCBS
{
    public class AppVariable
    {
        public static string ServerLogin = "server=" + ConfigurationManager.AppSettings["DBServer"] + ";database=RTGS;uid=floraweb;pwd=platinumfloor967";
    }
}
