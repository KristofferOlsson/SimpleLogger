using Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggableService
{
    interface IWorker
    {
        void Work(PMSLogger logger);
    }
}
