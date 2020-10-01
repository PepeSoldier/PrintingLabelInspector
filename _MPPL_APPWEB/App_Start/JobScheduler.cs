using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;

namespace _MPPL_WEB_START.App_Start
{
    public class JobScheduler
    {
        public static void Start(string clientName)
        {
            string HostName = System.Environment.MachineName;
            List<string> HostsDev = new List<string>() { "C4AEDF-NB", "IN0003", "DESKTOP-VTS936D", "SAMKAM10", "IN0001" };

            if (!HostsDev.Contains(HostName))
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                IScheduler sched = schedFact.GetScheduler().GetAwaiter().GetResult();

                switch (clientName)
                {
                    case "ElectroluxPLV": 
                        //ScheduleJOB_TcpToWeb(sched, clientName);
                        ScheduleJOB_Inspection(sched, clientName);
                        break;
                    default: NoJOB(); break;
                }

                sched.Start();
            }
        }

        public static void ScheduleJOB_TcpToWeb(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobTcp2Web>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobTcp2Web", "group2")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    //.WithDailyTimeIntervalSchedule(s =>
                    //    //s.WithIntervalInMinutes(1))
                    //    s.WithIntervalInSeconds(15))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }
        public static void ScheduleJOB_Inspection(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobInspection>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobInspection", "group2")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    //.WithDailyTimeIntervalSchedule(s =>
                    //    //s.WithIntervalInMinutes(1))
                    //    s.WithIntervalInSeconds(15))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }
        public static void NoJOB() { }
    }
}