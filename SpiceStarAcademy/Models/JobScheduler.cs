using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiceStarAcademy.Models
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Jobclass>().Build();
            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(3)
            .RepeatForever())
            .Build();

            IJobDetail job1 = JobBuilder.Create<JobFeeClass>().Build();
            ITrigger trigger1 = TriggerBuilder.Create()
            .WithIdentity("trigger2", "group2")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(45)
            .RepeatForever())
            .Build();

            IJobDetail job2 = JobBuilder.Create<JobTRClass>().Build();
            ITrigger trigger2 = TriggerBuilder.Create()
            .WithIdentity("trigger3", "group3")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInHours(1)
            .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.ScheduleJob(job1, trigger1);
            scheduler.ScheduleJob(job2, trigger2);
        }
    }
}