using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner_SAWA_KIM.Logging
{
    /// <summary>
    /// Wraps the log4j2 logger instances by realizing interface ILoggerWrapper.
    /// This avoids direct dependencies to log4j2 package.
    /// </summary>
    public class Log4NetWrapper : ILoggerWrapper
    {
        private log4net.ILog logger;

        public static Log4NetWrapper CreateLogger(string configPath, string caller)
        {
            if (!File.Exists(configPath))
            {
                throw new ArgumentException("Does not exist.", nameof(configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(caller);  // System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
            return new Log4NetWrapper(logger);
        }

        private Log4NetWrapper(log4net.ILog logger)
        {
            this.logger = logger;
        }

        public void Debug(string message)
        {
            this.logger.Debug(message);
        }
        public void Warn(string message)
        {
            this.logger.Warn(message);
        }

        public void Error(string message)
        {
            this.logger.Error(message);
        }

        public void Fatal(string message)
        {
            this.logger.Fatal(message);
        }
    }
}
