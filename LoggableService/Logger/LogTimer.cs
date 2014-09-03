using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Loggers
{

    public class LogTimer : IDisposable
    {
        System.Diagnostics.Stopwatch _timer = new System.Diagnostics.Stopwatch();
        IPMSLogger _logger;
        string _timedProcess;
        LogLevel _logLevel = LogLevel.Info;
        object _parameters;
        string _filePath;
        int _lineNumber;
        bool _isStarted;

        private LogTimer(IPMSLogger logger, string timedProcess, object parameters, LogLevel logLevel, int lineNumber, string filePath)
        {
            _logger = logger;
            _timedProcess = timedProcess;
            _logLevel = logLevel;
            _parameters = parameters;
            _filePath = filePath;
            _lineNumber = lineNumber;
        }

        public static LogTimer StartNew(IPMSLogger logger, string timedProcess, LogLevel logLevel = LogLevel.Debug, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            return StartInstance(logger, timedProcess, null, logLevel, lineNumber, filePath);
        }

        public static LogTimer StartNew(IPMSLogger logger, string timedProcess, object parameters, LogLevel logLevel = LogLevel.Debug, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            return StartInstance(logger, timedProcess, parameters, logLevel, lineNumber, filePath);
        }

        private static LogTimer StartInstance(IPMSLogger logger, string timedProcess, object parameters, LogLevel logLevel, int lineNumber, string filePath)
        {
            var logTimer = new LogTimer(logger, timedProcess, parameters, logLevel, lineNumber, filePath);
            if (logger.IsInDebugMode || logLevel > LogLevel.Debug)
            {
                logger.Log("[LogTimer - Starting] " + timedProcess, parameters, logLevel, lineNumber, filePath);

                logTimer._timer.Start();
                logTimer._isStarted = true;
            }
            return logTimer;
        }

        public void Dispose()
        {
            if (_isStarted)
            {
                _timer.Stop();

                _logger.Log("[LogTimer - Elapsed Milliseconds: " + _timer.ElapsedMilliseconds + "] " + _timedProcess, _parameters,  _logLevel, _lineNumber, _filePath);
            }
        }
    }
}
