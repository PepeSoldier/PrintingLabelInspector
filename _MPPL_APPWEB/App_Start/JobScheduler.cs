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
                    //case "Dev": DevInjections(); break;
                    //case "DevK": DevInjections(); break;
                    //case "DevP": DevInjections(); break;
                    //case "ElectroluxPLV": ElectroluxPLVInjections(); break;
                    //case "ElectroluxPLV_Staging": ElectroluxPLVInjections(); break;
                    case "ElectroluxPLB": 
                        ScheduleImportSAPFiles(sched, clientName);
                        ScheduleImportWorkordersFromSAP(sched, clientName);
                        //ScheduleImporStocksFromSAP(sched, clientName);
                        ScheduleExportMovementsToSAP(sched, clientName);
                        break;
                    //case "Eldisy": EldisyInjections(); break;
                    //case "Eldisy2": Eldisy2Injections(); break;
                    //case "Grandhome": GrandhomeInjections(); break;
                    case "WRP": 
                        ScheduleEnergyMetersImport(sched);
                        //Funkcje wymagają zalogowanego usera. Jak podamy id Admina, to w wpisach podmieni się used.
                        //ScheduleSaveReport(sched);
                        break;
                    default: NoJOB(); break;
                }

                sched.Start();
            }            
        }

        public static void ScheduleImportSAPFiles(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobImportDeliveries>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobImportDeliveries", "group2")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s =>
                        //s.WithIntervalInMinutes(1))
                        s.WithIntervalInSeconds(120))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }
        public static void ScheduleImportWorkordersFromSAP(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobImportWorkorders>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobImportWorkorders", "group5")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s =>
                        //s.WithIntervalInMinutes(1))
                        s.WithIntervalInSeconds(240))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }
        public static void ScheduleImporStocksFromSAP(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobImportStocks>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobImportStocks", "group5")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s =>
                        //s.WithIntervalInMinutes(1))
                        s.WithIntervalInSeconds(300))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }
        public static void ScheduleExportMovementsToSAP(IScheduler sched, string clientName)
        {
            IJobDetail job = JobBuilder.Create<JobExportMovements>()
                .UsingJobData("clientName", clientName)
                .WithIdentity("JobExportMovements", "group3")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s =>
                        //s.WithIntervalInMinutes(1))
                        s.WithIntervalInSeconds(300))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }

        public static void ScheduleEnergyMetersImport(IScheduler sched)
        {
            IJobDetail job = JobBuilder.Create<JobEnergyMetersImport>().WithIdentity("myJob", "group1").Build();

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(s =>
                        s.WithIntervalInMinutes(10))
                        //.OnMondayThroughFriday())
                    .Build();
            sched.ScheduleJob(job, trigger);
        }
        public static void ScheduleSaveReport(IScheduler sched)
        {
            IJobDetail job = JobBuilder.Create<JobOEEReportsSave>().WithIdentity("myJobOEEReportsSave", "groupJobOEEReportsSave").Build();

            var startAt = DateTimeOffset.Now.Date.AddHours(6).AddMinutes(5);

            ITrigger trigger = TriggerBuilder.Create()
                    .StartAt(startAt)
                    .WithDailyTimeIntervalSchedule(s => s.WithIntervalInHours(8))
                    .Build();
            sched.ScheduleJob(job, trigger);
        }

        public static void NoJOB() { }
    }
}