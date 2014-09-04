using Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace LoggableService
{
    public class Worker1 : IWorker
    {
        public string SomeValue = "test";
        public void Work(PMSLogger logger)
        {
            var rnd = new Random();
            logger.Log("start");
            var tasks = new List<Task>();
            using (logger.StartTimer("Main work"))
            {
                for (int i = 0; i < 3; i++)
                {
                    var iter = i;
                    tasks.Add(Task.Run(() => {

                        using (logger.StartTimer("running task " + iter))
                        {
                            int sleep = rnd.Next(500, 1500);
                            logger.Log("Starting to feel sleepy... in task " + iter, new
                                {
                                    TaskId = iter,
                                    Context = new
                                    {
                                        Key = 1 + iter,
                                        Value = "One + " + iter
                                    }
                                });
                            Thread.Sleep(sleep);
                            logger.Log("done sleeping in task " + iter, LogLevel.Debug);
                        }
                    }));   
                }
            }
        }
    }
}