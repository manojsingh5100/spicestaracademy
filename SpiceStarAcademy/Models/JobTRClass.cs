using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SpiceStarAcademy.Models
{
    public class JobTRClass:IJob
    {
        Common c = null;
        public JobTRClass()
        {
            c = new Common();
        }
        public void Execute(IJobExecutionContext context)
        {
           c.TerminationResignationSheduler();
        }
    }
}