using _LABELINSP_APPWEB.App_Start;
using _LABELINSP_APPWEB.Migrations;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_LABELINSP.Interfaces;
using MDLX_MASTERDATA._Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(_LABELINSP_APPWEB.Startup))]

namespace _LABELINSP_APPWEB
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
                .Concat(new string[] { "~/Areas/_APPWEB/Views/{1}/{0}.cshtml", "~/Areas/_APPWEB/Views/{0}.cshtml" }).ToArray();

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

            JobScheduler.Instance.Start(clientName);
            app.MapSignalR();
        }
    }

    public class MyRazorViewEngine : RazorViewEngine
    {
        public MyRazorViewEngine() : base()
        {
        }

        public MyRazorViewEngine(IViewPageActivator viewPageActivator) : base(viewPageActivator)
        {
        }

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
                case "PackingLabel": PackingLabelInjections(); break;
                case "ElectroluxPLV": ElectroluxPLVInjections(); break;
                default: DefaultInjections(); break;
            }
            // REGISTER CONTROLLERS SO DEPENDENCIES ARE CONSTRUCTOR INJECTED
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            ClientIndependentInjections(appk);
        }

        private void PackingLabelInjections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<DbContextAPP_PackingLabel>()
            );

            builder.RegisterType<DbContextAPP_PackingLabel>().AsSelf().InstancePerDependency();
            builder.RegisterType<DbContextAPP_PackingLabel>().As<IDbContextLabelInsp>();
            builder.RegisterType<DbContextAPP_PackingLabel>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_PackingLabel>().As<IDbContextMasterData>();
            //builder.RegisterType<DbContextAPP_PackingLabel>().As<IDbContextOneprodQuality>();
        }

        private void DevInjections()
        {
            string connectionNameStr = DbContextAPP_Dev.GetConnectionName();
            dbContextParameter = new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext),
                                                           (pi, ctx) => ctx.Resolve<DbContextAPP_Dev>());

            builder.RegisterType<DbContextAPP_PackingLabel>().AsSelf().InstancePerDependency();
            builder.RegisterType<DbContextAPP_PackingLabel>().As<IDbContextCore>();
            builder.RegisterType<DbContextAPP_PackingLabel>().As<IDbContextMasterData>();
        }

        private void ElectroluxPLVInjections()
        {
            string connectionName = DbContextAPP_ElectroluxPLV.GetConnectionName();
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => new DbContextAPP_ElectroluxPLV(connectionName)
            );

            builder.RegisterType<DbContextAPP_ElectroluxPLV>().AsSelf().InstancePerDependency().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextCore>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextMasterData>().WithParameter("connectionName", connectionName);
            builder.RegisterType<DbContextAPP_ElectroluxPLV>().As<IDbContextLabelInsp>();
            //builder.RegisterType<BarcodeParserEluxPLVTech>().As<IBarcodeParser>();
        }

        private void DefaultInjections()
        {
            dbContextParameter = new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<DbContextAPP_Dev>());

            builder.RegisterType<DbContextAPP_>().AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextAPP_>().As<IDbContextCore>();
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