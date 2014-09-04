using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace Loggers
{
    public class PMSLogPrinter
    {
        public static string PrintLogItem(LogItem logItem, Func<object, string> serializeParametersFunc)
        {
            string logLevelLine = null;
            if (logItem.OriginalLogLevel != logItem.LogLevel)
            {
                logLevelLine = string.Format("[{0}]", logItem.OriginalLogLevel.ToString().ToUpper()) + Environment.NewLine;
            }

            string refLine = null;
            if (logItem.Reference != null)
            {
                refLine = "Reference: " + logItem.Reference + Environment.NewLine;
            }
            string headerLine = null;
            if (logItem.Header != null)
            {
                headerLine = logItem.Header + Environment.NewLine;
            }

            string parametersLine = null;
            if (logItem.Parameters != null && serializeParametersFunc != null)
            {
                parametersLine = serializeParametersFunc(logItem.Parameters) + Environment.NewLine;// JsonConvert.SerializeObject(logItem.Parameters, Formatting.Indented);
            }

            string knownSolutionLine = null;
            if (logItem.KnownSolution != null && serializeParametersFunc != null)
            {
                knownSolutionLine = "Suggested solution: " + serializeParametersFunc(logItem.KnownSolution) + Environment.NewLine; //JsonConvert.SerializeObject(logItem.KnownSolution, Formatting.Indented);
            }

            string messageLine = null;
            if (logItem.Message != null)
            {
                messageLine = logItem.Message + Environment.NewLine;
            }

            string exLine = null;
            if (logItem.Exception != null)
            {
                exLine = "Exception: " + Environment.NewLine +  logItem.Exception.ToString() + Environment.NewLine;
            }

            string fileLine = null;
            if (logItem.File != null)
            {
                fileLine = "File: " + logItem.File + Environment.NewLine;
            }
            string lineNumberLine = null;
            if (logItem.LineNumber != 0)
            {
                fileLine += "Line number: " + logItem.LineNumber + Environment.NewLine;
            }


            string printed = messageLine
                   + parametersLine
                   + knownSolutionLine
                   + exLine
                   + Environment.NewLine
                   + fileLine
                   + lineNumberLine
                   + refLine
                   + headerLine;

            string debugLogLevelLine = "[" + logItem.LogLevel.ToString().ToUpper() + "] " + Environment.NewLine;

            System.Diagnostics.Debug.WriteLine(debugLogLevelLine + printed
                + Environment.NewLine + "---------------" + Environment.NewLine);

            return printed;

        }
    }
}
