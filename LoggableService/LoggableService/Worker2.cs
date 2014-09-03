using Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoggableService
{

    public class Worker2 : IWorker
    {
        public void Work(IPMSLogger logger)
        {
            logger.Log("I have not been implemented yet.", LogLevel.Error);
        }
    }
}