using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Quartz;

namespace _MPPL_WEB_START.App_Start
{
    public class JobEnergyMetersImport : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //IDbContextOneProdENERGY db3 = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneProdENERGY>();
            //IDbContextOneProdOEE db2 = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneProdOEE>();

            //var result = new EnergyController(db3, db2).CheckAndImportNewDataFromCSV();
            return null;
        }
    }

    //public class JobImportDeliveries : IJob
    //{
    //    public Task Execute(IJobExecutionContext context)
    //    {
    //        IDbContextiLOGIS db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextiLOGIS>();
    //        string clientName = context.JobDetail.JobDataMap.FirstOrDefault(x => x.Key == "clientName").Value.ToString();

    //        new ApiController(db).ImportDeliveries(clientName);
            
    //        return null;
    //    }
    //}

}