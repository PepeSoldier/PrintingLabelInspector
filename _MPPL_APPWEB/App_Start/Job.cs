using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _MPPL_WEB_START.Areas.iLOGIS.Controllers;
using _MPPL_WEB_START.Areas.ONEPROD.Controllers;
using _MPPL_WEB_START.Migrations;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_ONEPROD.ComponentENERGY;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using Quartz;

namespace _MPPL_WEB_START.App_Start
{
    public class JobEnergyMetersImport : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextOneProdENERGY db3 = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneProdENERGY>();
            IDbContextOneProdOEE db2 = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneProdOEE>();

            //IDbContextOneProdENERGY db3 = new DbContextAPP_WRP("connectionName");
            //IDbContextOneProdOEE db2 = new DbContextAPP_WRP("connectionName");
            var result = new EnergyController(db3, db2).CheckAndImportNewDataFromCSV();
            return null;
        }
    }

    public class JobOEEReportsSave : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextOneProdOEE dbOEE = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneProdOEE>();
            IDbContextOneProdRTV dbRTV = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneProdRTV>();
            IDbContextOneprodMes dbMes = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneprodMes>();

            UnitOfWorkOneProdMes uowMes = new UnitOfWorkOneProdMes(dbMes);

            var workspaces = uowMes.WorkplaceRepo.GetList().ToList();

            DateTime n = DateTime.Now;
            int endShiftHour = n.Hour < 6 ? 6 : n.Hour < 14 ? 14 : n.Hour > 22 ? 30 : 22;
            DateTime endShift = DateTime.Now.Date.AddHours(endShiftHour-8);
            DateTime startShift = DateTime.Now.Date.AddHours(endShiftHour-16);

            foreach (Workplace w in workspaces)
            {
                new OEEReportOnlineController(dbOEE, dbRTV, dbMes).SaveReport_(w.MachineId, w.Id, 1, startShift, endShift, "efc38079-6bc2-49d7-9feb-8f6b14ef5d0a");
            }

            return null;
        }
    }

    public class JobImportDeliveries : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextiLOGIS db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextiLOGIS>();
            string clientName = context.JobDetail.JobDataMap.FirstOrDefault(x => x.Key == "clientName").Value.ToString();

            new ApiController(db).ImportDeliveries(clientName);
            
            return null;
        }
    }
    public class JobImportWorkorders : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextiLOGIS db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextiLOGIS>();
            string clientName = context.JobDetail.JobDataMap.FirstOrDefault(x => x.Key == "clientName").Value.ToString();

            new ApiController(db).ImportWorkorders(clientName);

            return null;
        }
    }
    public class JobImportStocks: IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextiLOGIS db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextiLOGIS>();
            string clientName = context.JobDetail.JobDataMap.FirstOrDefault(x => x.Key == "clientName").Value.ToString();

            new ApiController(db).ImportStocks(clientName);

            return null;
        }
    }
    public class JobExportMovements : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextiLOGIS db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextiLOGIS>();
            string clientName = context.JobDetail.JobDataMap.FirstOrDefault(x => x.Key == "clientName").Value.ToString();

            new ApiController(db).ExportMovements(clientName);

            return null;
        }
    }
    public class JobResetSerialNumbers : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            IDbContextiLOGIS db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextiLOGIS>();
            string clientName = context.JobDetail.JobDataMap.FirstOrDefault(x => x.Key == "clientName").Value.ToString();

            new ApiController(db).ExportMovements(clientName);

            return null;
        }
    }
}