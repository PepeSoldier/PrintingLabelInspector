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

            Tcp2Web tcp2Web = new Tcp2Web("http://10.26.10.90:84", "/LABELINSP/Quality/TCPBarcodeReceived");

            foreach(string tcp in tcpL)
            {
                if (tcp.Contains(":"))
                {
                    string ip = tcp.Split(':')[0];
                    string port = tcp.Split(':')[1];

                    tcp2Web.RegisterAndRunTCPListener(ip, port);
                }
            }

            return null;
        }
    }

    public class JobInspection : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            LabelInspectionManager labelInspectionManager = new LabelInspectionManager();
            labelInspectionManager.Start();

            return null;
        }
    }

}