using Nu.CommandLine;
using Nu.CommandLine.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace pfsim
{
    class Program
    {
        static void Main(string[] args)
        {
            SettingsManager.SetSettings(args);
            var rc = HostFactory.Run(x =>
            {
                HostLogger.UseLogger(new NullLogger());
                x.Service<ControlService>();
            });
        }
    }

    class NullLogger : HostLoggerConfigurator, LogWriterFactory, LogWriter
    {
        public bool IsDebugEnabled => false;

        public bool IsInfoEnabled => false;

        public bool IsWarnEnabled => false;

        public bool IsErrorEnabled => false;

        public bool IsFatalEnabled => false;

        public LogWriterFactory CreateLogWriterFactory()
        {
            return this;
        }

        public void Debug(object obj)
        {
        }

        public void Debug(object obj, Exception exception)
        {
        }

        public void Debug(LogWriterOutputProvider messageProvider)
        {
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void DebugFormat(string format, params object[] args)
        {
        }

        public void Error(object obj)
        {
        }

        public void Error(object obj, Exception exception)
        {
        }

        public void Error(LogWriterOutputProvider messageProvider)
        {
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void ErrorFormat(string format, params object[] args)
        {
        }

        public void Fatal(object obj)
        {
        }

        public void Fatal(object obj, Exception exception)
        {
        }

        public void Fatal(LogWriterOutputProvider messageProvider)
        {
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void FatalFormat(string format, params object[] args)
        {
        }

        public LogWriter Get(string name)
        {
            return this;
        }

        public void Info(object obj)
        {
        }

        public void Info(object obj, Exception exception)
        {
        }

        public void Info(LogWriterOutputProvider messageProvider)
        {
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void InfoFormat(string format, params object[] args)
        {
        }

        public void Log(LoggingLevel level, object obj)
        {
        }

        public void Log(LoggingLevel level, object obj, Exception exception)
        {
        }

        public void Log(LoggingLevel level, LogWriterOutputProvider messageProvider)
        {
        }

        public void LogFormat(LoggingLevel level, IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void LogFormat(LoggingLevel level, string format, params object[] args)
        {
        }

        public void Shutdown()
        {
        }

        public void Warn(object obj)
        {
        }

        public void Warn(object obj, Exception exception)
        {
        }

        public void Warn(LogWriterOutputProvider messageProvider)
        {
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void WarnFormat(string format, params object[] args)
        {
        }
    }
}
