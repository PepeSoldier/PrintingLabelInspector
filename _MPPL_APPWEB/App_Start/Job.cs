using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_LABELINSP.Models;
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

    public class JobTcp2Web : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            List<string> tcpL = JsonSerializer.Deserialize<List<string>>(Properties.Settings.Default.TCPListeners);

            //Tcp2Web tcp2Web = new Tcp2Web("", "");
            //tcp2Web.RegisterAndRunTCPListener("", "");
            //tcp2Web.RegisterAndRunTCPListener("", "");

            return null;
        }
    }

}