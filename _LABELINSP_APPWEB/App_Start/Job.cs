using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using _LABELINSP_APPWEB.Migrations;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_LABELINSP.Interfaces;
using MDL_LABELINSP.Models;
using Quartz;

namespace _LABELINSP_APPWEB.App_Start
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

    public class JobTcp2Web : IStoppableJob
    {
        private Tcp2Web tcp2Web;

        public Task Execute(IJobExecutionContext context)
        {
            Start();
            return null;
        }

        public void Start()
        {
            List<string> tcpL = JsonSerializer.Deserialize<List<string>>(Properties.Settings.Default.TCPListeners);

            tcp2Web = new Tcp2Web("http://10.26.10.90:84", "/LABELINSP/Quality/TCPBarcodeReceived");

            foreach (string tcp in tcpL)
            {
                if (tcp.Contains(":"))
                {
                    string ip = tcp.Split(':')[0];
                    string port = tcp.Split(':')[1];

                    tcp2Web.RegisterAndRunTCPListener(ip, port);
                }
            }
        }

        public void Stop()
        {
            tcp2Web.Stop();
        }
    }

    public class JobInspection : IStoppableJob
    {
        LabelInspectionManager labelInspectionManager;

        public Task Execute(IJobExecutionContext context)
        {
           

            return null;
        }

        public void Start()
        {
            //IDbContextLabelInsp db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextLabelInsp>();
            IDbContextLabelInsp db = new DbContextAPP_ElectroluxPLV();
            labelInspectionManager = new LabelInspectionManager(db);
            labelInspectionManager.Start();
        }

        public void Stop()
        {
            labelInspectionManager.Stop();
        }
    }

}