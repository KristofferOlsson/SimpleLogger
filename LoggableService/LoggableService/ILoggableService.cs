using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LoggableService
{
    [ServiceContract]
    public interface ILoggableService
    {
        [OperationContract]
        string Test(string command);
    }

}
