using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loggers
{
    public class PMSLogger : IPMSLogger
    {
        public LogHeader Header { get; private set; }

        public PMSLogger(string reference, Action<LogItem> onLogCallback, [CallerMemberName] string caller = "")
        {
            _onLog = onLogCallback;
            Reference = reference;
            Header = new LogHeader()
            {
                Initiator = caller,
            };
        }

        public void Log(string message, LogLevel logLevel = LogLevel.Info, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            WriteMessage(message, null, logLevel, null, null, lineNumber, filePath);
        }

        public void Log(string message, object parameters, LogLevel logLevel = LogLevel.Info, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            WriteMessage(message, null, logLevel, parameters, null, lineNumber, filePath);
        }

        public void Log(string message, object parameters, object knownSolution, LogLevel logLevel = LogLevel.Info, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            WriteMessage(message, null, logLevel, parameters, knownSolution, lineNumber, filePath);
        }

        public void LogException(string message, Exception e, LogLevel logLevel = LogLevel.Error, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            WriteMessage(message, e, logLevel, null, null, lineNumber, filePath);
        }

        public void LogException(string message, Exception e, object parameters, LogLevel logLevel = LogLevel.Error, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            WriteMessage(message, e, logLevel, parameters, null, lineNumber, filePath);
        }

        public void LogException(string message, Exception e, object parameters, object knownSolution, LogLevel logLevel = LogLevel.Error, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            WriteMessage(message, e, logLevel, parameters, knownSolution, lineNumber, filePath);
        }

        private void WriteMessage(string message, Exception e, LogLevel logLevel, object parameters, object knownSolution, int lineNumber, string filePath)
        {
            var logItem = new LogItem(Header, message, Reference, e, GetActualLogLevel(logLevel), logLevel, parameters, knownSolution, filePath, lineNumber);
            if (_onLog != null)
            {
                _onLog(logItem);
            }
        }

        private LogLevel GetActualLogLevel(LogLevel logLevel)
        {
            if (logLevel > LogLevel.Debug)
                return logLevel;

            return IsInDebugMode ? LogLevel.Info : LogLevel.Debug;
        }


        public IDisposable StartTimer(string message, LogLevel logLevel = LogLevel.Debug, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            return LogTimer.StartNew(this, message, logLevel, lineNumber, filePath);
        }

        public IDisposable StartTimer(string message, object parameters, LogLevel logLevel = LogLevel.Debug, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            return LogTimer.StartNew(this, message, parameters, logLevel, lineNumber, filePath);
        }

        public bool IsInDebugMode { get; set; }


        private Action<LogItem> _onLog;


        public string Reference { get; set; }

    }
}
