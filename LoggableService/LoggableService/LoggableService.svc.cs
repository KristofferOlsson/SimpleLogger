using Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LoggableService
{
    public class LoggableService : ILoggableService
    {
        public string Test(string command)
        {
            int config = -1;

            config = int.TryParse(command, out config) ? config: new Random().Next(5);

            string reference = new Random().Next(1000000000, int.MaxValue).ToString();
            IPMSLogger logger = new PMSLogger(reference, logger_OnLog);
            logger.Header.OrganisationId = config.ToString();
            logger.Header.BookingCode = command == "2" ? "ABCD12" : null;

            logger.IsInDebugMode = true;

            using (logger.StartTimer("worker.Work()"))
            {
                try
                {
                    logger.Header.ConnectorCode = command;
                    IWorker worker = config % 2 == 0 ? (IWorker)new Worker1() : (IWorker)new Worker2();
                    if (config == 1)
                    {
                        throw new Exception("config was 1!");
                    }
                    logger.Header.ConnectorTypeName = worker.GetType().ToString();
                    logger.IsInDebugMode = false;
                    worker.Work(logger);
                }
                catch (Exception e)
                {
                    logger.LogException("Error occured!", e, null,
                        knownSolution: new
                        {
                            Possibly = "Make better code",
                            InternalComment = "Coad sux"
                        });
                }
            }

            return logger.Reference;
        }

        private void logger_OnLog(LogItem logItem)
        {
            string printed = PMSLogPrinter.PrintLogItem(logItem, obj => JsonConvert.SerializeObject(obj, new JsonSerializerSettings(){
                Formatting = Formatting.Indented,
            }));
        }

    }
}
