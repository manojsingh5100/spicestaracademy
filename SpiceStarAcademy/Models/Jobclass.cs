using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace SpiceStarAcademy.Models
{
    public class Jobclass:IJob
    {
        Common c = null;
        public Jobclass()
        {
            c = new Common();
        }
        public void Execute(IJobExecutionContext context)
        {
          // c.SendRejectedEmailWithIn24Hours();
        }
    }
}