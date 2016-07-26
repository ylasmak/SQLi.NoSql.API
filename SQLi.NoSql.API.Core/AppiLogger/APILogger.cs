using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace SQLi.NoSql.API.Core.AppiLogger
{
    public class APILogger
    {

        private static ILog Log = LogManager.GetLogger(typeof(APILogger));

        public static void Write(string message , LogLevel levvel)
        {

            var logMessage = new StringBuilder();
            logMessage.AppendLine(APIContext.Instance.DbContext);
            logMessage.AppendLine(APIContext.Instance.ConnexionString);
            logMessage.AppendLine(message);

            log4net.Config.XmlConfigurator.Configure();

            switch (levvel)
            {
                case LogLevel.Error:
                    Log.Error(logMessage);
                    break;
                case LogLevel.Information:
                    Log.Info(logMessage);
                    break;
                case LogLevel.Warning:
                    Log.Warn(logMessage);
                    break;
            }

        }
    }


    public enum LogLevel
    {
        Information,
        Warning,
        Error

    }
}
