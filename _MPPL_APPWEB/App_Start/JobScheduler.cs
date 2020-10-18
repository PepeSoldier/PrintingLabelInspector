using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;

namespace _LABELINSP_APPWEB.App_Start
{
    public class JobScheduler
    {
        List<IStoppableJob> stoppableJobs;
        protected static object syncRoot = new Object();
        private static JobScheduler instance;
        public static JobScheduler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new JobScheduler();
                        }
                    }
                }
                return instance;
            }
        }

        private JobScheduler()
        {
            stoppableJobs = new List<IStoppableJob>();
        }

        public void Start(string clientName)
        {
            string HostName = System.Environment.MachineName;
            List<string> HostsDev = new List<string>() { "FAKE" };
            //List<string> HostsDev = new List<string>() { "C4AEDF-NB", "IN0003", "DESKTOP-VTS936D", "SAMKAM10", "IN0001" };

            if (!HostsDev.Contains(HostName))
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                IScheduler sched = schedFact.GetScheduler().GetAwaiter().GetResult();

                switch (clientName)
                {
                    case "ElectroluxPLV":
                        ScheduleJOB_TcpToWeb_2(sched, clientName);
                        ScheduleJOB_Inspection_2(sched, clientName);
                        break;
                    default: NoJOB(); break;
                }

                sched.Start();
            }
        }
        public void Stop()
        {
            foreach(var sj in stoppableJobs)
            {
                sj.Stop();
            }
        }

        public void ScheduleJOB_TcpToWeb(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobTcp2Web>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobTcp2Web", "group2")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .Build();

            sched.ScheduleJob(job, trigger);
            
        }
        public void ScheduleJOB_TcpToWeb_2(IScheduler sched, string clientName)
        {
            JobTcp2Web job = new JobTcp2Web();
            stoppableJobs.Add(job);
            job.Start();
        }
        public void ScheduleJOB_Inspection(IScheduler sched, string clientName)
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
        public void ScheduleJOB_Inspection_2(IScheduler sched, string clientName)
        {
            JobInspection job = new JobInspection();
            stoppableJobs.Add(job);
            job.Start();
        }
        public static void NoJOB() { }
    }

    interface IStoppableJob : IJob
    {
        void Start();
        void Stop();
    }
}