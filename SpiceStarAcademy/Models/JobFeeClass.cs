using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiceStarAcademy.Models
{
    public class JobFeeClass:IJob
    {
        Common c = null;
        public JobFeeClass()
        {
            c = new Common();
        }
        public void Execute(IJobExecutionContext context)
        {
          // c.SendFeeNotificationToCadidates();
        }
    }
}