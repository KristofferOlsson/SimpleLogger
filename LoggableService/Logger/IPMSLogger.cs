using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Loggers
{
    public interface IPMSLogger
    {
        void Log(string message, LogLevel logLevel = LogLevel.Info, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        void Log(string message, object parameters, LogLevel logLevel = LogLevel.Info, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        void Log(string message, object parameters, object knownSolution, LogLevel logLevel = LogLevel.Info, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        void LogException(string message, Exception e, LogLevel logLevel = LogLevel.Error, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        void LogException(string message, Exception e, object parameters, LogLevel logLevel = LogLevel.Error, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        void LogException(string message, Exception e, object parameters, object knownSolution, LogLevel logLevel = LogLevel.Error, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        IDisposable StartTimer(string message, LogLevel logLevel = LogLevel.Debug, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);
        IDisposable StartTimer(string message, object parameters, LogLevel logLevel = LogLevel.Debug, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null);

        bool IsInDebugMode { get; set; }
        /// <summary>
        /// A unique, searchable reference that can be used to track all messages logged by this instance.
        /// </summary>
        string Reference { get; set; }
        LogHeader Header { get; }
    }

    public class LogHeader
    {
        public string Initiator{ get; internal set; }
        public string BookingCode { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string ConnectorTypeName { get; set; }
        public string ConnectorCode { get; set; }

        public override string ToString()
        {
            var toString = string.Join(Environment.NewLine,
                this.GetType().GetProperties()
                    .Select(p => new {p.Name, Value = p.GetValue(this)})
                    .Where(p => p.Value != null).Select(p => p.Name + ": " + p.Value));
            
            return toString;
        }
    }

    public class LogItem
    {
        public LogHeader Header;
        public string Message;
        public string Reference;
        public Exception Exception;
        public LogLevel LogLevel;
        public LogLevel OriginalLogLevel;
        public object Parameters;
        public object KnownSolution;
        public string File;
        public int LineNumber;

        public LogItem(LogHeader header, string message, string reference, Exception e, LogLevel logLevel, LogLevel originalLogLevel, object parameters, object knownSolution, string fileName, int lineNumber)
        {
            Header = header;
            Message = message;
            Reference = reference;
            Exception = e;
            LogLevel = logLevel;
            OriginalLogLevel = originalLogLevel;
            Parameters = parameters;
            KnownSolution = knownSolution;
            File = fileName;
            LineNumber = lineNumber;
        }
    }


    public enum LogLevel : int
    { 
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
    }
}
