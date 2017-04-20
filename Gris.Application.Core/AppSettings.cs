using System.Configuration;

namespace Gris.Application.Core
{
    public static class AppSettings
    {
        public static int PageSize
        {
            get { return int.Parse(ConfigurationManager.AppSettings["PageSize"]); }
        }

        public static int DefaultAvailableHours
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DefaultAvailableHours"]); }
        }
    }
}