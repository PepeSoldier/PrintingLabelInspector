using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System.Web;
using Autofac;
using _MPPL_WEB_START.App_Start;
using _MPPL_WEB_START.Areas.ONEPROD.Controllers;
using _MPPL_WEB_START.Migrations;
using Autofac.Integration.Mvc;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Autofac.Core;
using System.Data.Entity;
using MDL_AP.Interfaces;
using MDL_PFEP.Interface;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_PRD.Interface;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentBase.Models;
using MDLX_MASTERDATA._Interfaces;
using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentWMS.Repos;
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_ONEPROD.ComponentENERGY;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;
using System;
using MDL_ONEPROD.ComponentQuality._Interfaces;

[assembly: OwinStartupAttribute(typeof(_MPPL_WEB_START.Startup))]
namespace _MPPL_WEB_START
{
    public partial class Startup
    {
        public IContainer container;
        public void Configuration(IAppBuilder app)
        {
            string clientName = Properties.Settings.Default.Client;
            DependencyInjectorMPPLTwo di = new DependencyInjectorMPPLTwo(clientName, app);

            var razorEngine = ViewEngines.Engines.OfType<MyRazorViewEngine>().FirstOrDefault();

            razorEngine.ViewLocationFormats = razorEngine.ViewLocationFormats
                .Concat(new string[] {"~/Areas/_APPWEB/Views/{1}/{0}.cshtml", "~/Areas/_APPWEB/Views/{0}.cshtml"}).ToArray();

            razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats.
                Concat(new string[] { "~/Areas/_APPWEB/Views/Shared/{0}.cshtml" }).ToArray();

            razorEngine.AreaViewLocationFormats = razorEngine.AreaViewLocationFormats
                .Concat(new string[] { "~/Areas/{2}/Views/{0}.cshtml" }).ToArray();

            // BUILD THE CONTAINER
            container = di.builder.Build();

            // REPLACE THE MVC DEPENDENCY RESOLVER WITH AUTOFAC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // REGISTER WITH OWIN
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);
            JobScheduler.Start(clientName);

            app.MapSignalR();
        }
}

    public class MyRazorViewEngine : RazorViewEngine
    {
        public MyRazorViewEngine() : base() { }
        public MyRazorViewEngine(IViewPageActivator viewPageActivator) : base(viewPageActivator) { }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }
    }

    public class DependencyInjectorMPPLTwo
    {
        public ContainerBuilder builder = new ContainerBuilder();
        public ResolvedParameter dbContextParameter;
        public DependencyInjectorMPPLTwo(string clientName, IAppBuilder appk)
        {
            switch (clientName)
            {
                case "Dev": DevInjections(); break;
                case "DevK": DevInjections(); break;
                case "DevP": DevInjections(); break;
                case "ElectroluxPLB": ElectroluxPLBInjections(); break;
                case "ElectroluxPLS": ElectroluxPLSInjections(); break;
                case "ElectroluxPLV": ElectroluxPLVInjections(); break;
                case "ElectroluxPLV_Staging": ElectroluxPLVInjections(); break;
                case "Eldisy": EldisyInjections(); break;
                case "Eldisy2": Eldisy2Injections(); break;
                case "Grandhome": GrandhomeInjections(); break;
                case "WRP": WRPInjections(); break;
                    default: DefaultInjections(); break;
            }
            // REGISTER CONTROLLERS SO DEPENDENCIES ARE CONSTRUCTOR INJECTED
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            ClientIndependentInjections(appk);
        }

        private void ElectroluxPLSInjections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<DbContextAPP_ElectroluxPLS>()
            );

            builder.RegisterType<DbContextAPP_ElectroluxPLS>().AsSelf().InstancePerDependency();
            builder.RegisterType<DbContextAPP_ElectroluxPLS>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_ElectroluxPLS>().As<IDbContextMasterData>();
            builder.RegisterType<DbContextAPP_ElectroluxPLS>().As<IDbContextiLOGIS>();
        }

        private void DevInjections()
        {
            string connectionNameStr = DbContextAPP_Dev.GetConnectionName();
            dbContextParameter = new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext),
                                                           (pi, ctx) => ctx.Resolve<DbContextAPP_Dev>());

            builder.RegisterType<DbContextAPP_Dev>().AsSelf().InstancePerRequest().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextAP>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextCore>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPRD>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPFEP>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPFEP_Eldisy>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneprod>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneprodWMS>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdOEE>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneprodMes>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdRTV>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneprodAPS>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextMasterData>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdENERGY>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextiLOGIS>().WithParameter("nameOrConnectionString", connectionNameStr);
            //builder.RegisterType<StockUnitRepo_FSDS_PLV>().As<ILocataionManager>();
            //builder.RegisterType<DbContextAPP_Dev>().As<IDbContextVisualControl>().WithParameter("nameOrConnectionString", connectionNameStr);
            builder.RegisterType<StockUnitRepo_FAKE>().As<ILocataionManager>();
            builder.RegisterType<BarcodeParserEluxPLVTech>().As<IBarcodeParser>();
            builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPickByLight>().WithParameter("nameOrConnectionString", connectionNameStr);
        }

        //IDbContextOtherHSE/
        private void ElectroluxPLBInjections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => new DbContextAPP_ElectroluxPLB()
            );

            builder.RegisterType<DbContextAPP_ElectroluxPLB>().AsSelf().InstancePerDependency();
            builder.RegisterType<DbContextAPP_ElectroluxPLB>()
                .AsSelf()
                .As<IdentityDbContext<User, ApplicationRole, string, UserLogin, UserRole, UserClaim>>()
                .InstancePerDependency();
            builder.RegisterType<DbContextAPP_ElectroluxPLB>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_ElectroluxPLB>().As<IDbContextOtherHSE>();
            builder.RegisterType<DbContextAPP_ElectroluxPLB>().As<IDbContextPRD>();
            builder.RegisterType<DbContextAPP_ElectroluxPLB>().As<IDbContextMasterData>();
            builder.RegisterType<DbContextAPP_ElectroluxPLB>().As<IDbContextiLOGIS>();
            builder.RegisterType<BarcodeParserEluxPLVTech>().As<IBarcodeParser>();

            builder.RegisterType<StockUnitRepo_FAKE>().As<ILocataionManager>();
        }
        private void ElectroluxPLVInjections()
        {
            string connectionName = DbContextAPP_ElectroluxPLV.GetConnectionName();
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => new DbContextAPP_ElectroluxPLV(connectionName)
            );

            builder.RegisterType<DbContextAPP_ElectroluxPLV>().AsSelf().InstancePerDependency().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextAP>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextCore>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextPRD>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextPFEP>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneprod>().InstancePerLifetimeScope().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneprodWMS>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneProdOEE>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneprodMes>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneProdRTV>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneprodAPS>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextOneprodQuality>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextMasterData>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextiLOGIS>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextVisualControl>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextPickByLight>().WithParameter("connectionName", connectionName);
            builder.RegisterType<BarcodeParserEluxPLVTech>().As<IBarcodeParser>();
            builder.RegisterType<StockUnitRepo_FSDS_PLV>().As<ILocataionManager>();
        }
        private void WRPInjections()
        {
            string connectionName = DbContextAPP_WRP.GetConnectionName();
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => new DbContextAPP_WRP(connectionName)
            );

            builder.RegisterType<DbContextAPP_WRP>().AsSelf().InstancePerDependency().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextCore>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextOneprod>().InstancePerLifetimeScope().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextOneProdOEE>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextOneprodMes>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextOneProdRTV>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextOneProdENERGY>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_WRP>().As<IDbContextMasterData>().WithParameter("connectionName", connectionName);
        }
        private void EldisyInjections()
        {
            dbContextParameter = new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext),
                                                           (pi, ctx) => ctx.Resolve<DbContextAPP_Eldisy>());

            builder.RegisterType<DbContextAPP_Eldisy>().AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextAPP_Eldisy>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_Eldisy>().As<IDbContextPFEP_Eldisy>();
            builder.RegisterType<StockUnitRepo_FAKE>().As<ILocataionManager>();
            builder.RegisterType<ImportDataEldisy>().As<ImportData>();
        }
        private void Eldisy2Injections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<DbContextAPP_Eldisy2>());

            builder.RegisterType<DbContextAPP_Eldisy2>().AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextPFEP_Eldisy>();
            builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextOneprod>();
            builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextOneProdOEE>();
            builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextOneprodMes>();
            builder.RegisterType<StockUnitRepo_FAKE>().As<ILocataionManager>();
            //builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextOneprodWMS>(); //Trzeba zabrać im dostęp
            //builder.RegisterType<DbContextAPP_Eldisy2>().As<IDbContextOneprodAPS>(); //Trzeba zabrać im dostęp
            builder.RegisterType<ImportDataEldisy>().As<ImportData>();
        }
        private void GrandhomeInjections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<DbContextAPP_Grandhome>());

            builder.RegisterType<DbContextAPP_Grandhome>().AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextOneprod>().InstancePerLifetimeScope();
            builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextOneprodMes>();
            builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextOneprodAPS>();
            //builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextOneprodWMS>();
            builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextMasterData>();
            builder.RegisterType<DbContextAPP_Grandhome>().As<IDbContextiLOGIS>();
            builder.RegisterType<StockUnitRepo_FAKE>().As<ILocataionManager>();
            builder.RegisterType<ImportDataGrandhome>().As<ImportData>();
        }
        private void DefaultInjections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<DbContextAPP_Dev>());

            builder.RegisterType<DbContextAPP_>().AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextAPP_>().As<IDbContextCore>();
            builder.RegisterType<StockUnitRepo_FAKE>().As<ILocataionManager>();
        }

        private void ClientIndependentInjections(IAppBuilder app)
        {
            builder.RegisterType<IdentityDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
            builder.RegisterType<ApplicationUserStore<User>>().As<IUserStore<User, string>>().WithParameter(dbContextParameter).InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleStore>().As<IRoleStore<ApplicationRole, string>>().WithParameter(dbContextParameter).InstancePerLifetimeScope();
            ////builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>().WithParameter(dbContextParameter).InstancePerLifetimeScope();
        }
    }
}
