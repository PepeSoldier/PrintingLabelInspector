using _MPPL_WEB_START.App_Start;
using _MPPL_WEB_START.Areas.ONEPROD.Controllers;
using _MPPL_WEB_START.Migrations;
using _MPPL_WEB_START.Models;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using MDL_AP.Interfaces;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.Interface;
using MDL_PFEP.Interface;
//using MDL_PRD.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START
{
    //public partial class Startup
    //{
    //    public static DependencyInjectorMPPL RegisterComponents()
    //    {
    //        string clientName = Properties.Settings.Default.Client;
    //        DependencyInjectorMPPL di = new DependencyInjectorMPPL(clientName);
    //        return di;
    //    }
    //}

    //public class DependencyInjectorMPPL
    //{
    //    //public UnityContainer container = new UnityContainer();
    //    public ContainerBuilder builder = new ContainerBuilder();
    //    public InjectionConstructor accountInjectionConstructor;
    //    public IContainer container;
    //    public DependencyInjectorMPPL(string clientName)
    //    {
    //        switch (clientName)
    //        {
    //            case "Dev": DevInjections(); break;
    //            case "Electrolux": ElectroluxInjections(); break;
    //            //case "Eldisy": EldisyInjections(); break;
    //            //case "Grandhome": GrandhomeInjections(); break;
    //            default: DevInjections(); break;
    //            //default: DefaultInjections(); break;
    //        }
    //       // ClientIndependentInjections();

    //       // builder.RegisterType<ConfigurationController>();
    //       // builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdExecution>().WithParameter("nameOrConnectionString", "DevPConnection");
    //       // builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdOEE>().WithParameter("nameOrConnectionString", "DevPConnection");

    //       // builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
    //       // builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
    //       //// builder.Register<IDataProtectionProvider>(c => GetDataProtectionProvider()).InstancePerRequest();

    //       // builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
    //       // builder.RegisterType<UserStore<User>>().As<IUserStore<User>>().WithParameter("nameOrConnectionString", "DevPConnection"); 
    //       // builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>().WithParameter("nameOrConnectionString", "DevPConnection");
    //       // builder.RegisterType<DbContextAPP_Dev>().As<IDbContextBase>().WithParameter("nameOrConnectionString", "DevPConnection"); 
    //       // builder.RegisterControllers(typeof(MvcApplication).Assembly);

    //       // container = builder.Build();
    //       //// builder.RegisterControllers(typeof(MvcApplication).Assembly);
    //       // DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
    //    }

    //    private void DevInjections()
    //    {
    //        //string connectionNameStr = DbContextAPP_Dev.GetConnectionName();
    //        //InjectionConstructor connectionInjectionConstructor = new InjectionConstructor(connectionNameStr);
    //        //accountInjectionConstructor = new InjectionConstructor(new DbContextAPP_Dev(connectionNameStr));

    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextAP>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextBase>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextAP>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPRD>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPFEP>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextPFEP_Eldisy>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextONEPROD>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdExecution>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdOEE>();
    //        builder.RegisterType<DbContextAPP_Dev>().As<IDbContextOneProdRTV>();

    //        //builder.RegisterType<DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextBase, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextAP, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextPRD, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextPFEP, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextPFEP_Eldisy, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextONEPROD, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextOneProdExecution, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextOneProdOEE, DbContextAPP_Dev>(connectionInjectionConstructor);
    //        //builder.RegisterType<IDbContextOneProdRTV, DbContextAPP_Dev>(connectionInjectionConstructor);

    //    }
    //    private void ElectroluxInjections()
    //    {
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextBase>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextAP>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextPRD>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextPFEP>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextONEPROD>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextOneProdOEE>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextOneProdExecution>();
    //        builder.RegisterType<DbContextAPP_Electrolux>().As<IDbContextOneProdRTV>();

    //        //    container.RegisterType<DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextBase, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextAP, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextPRD, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextPFEP, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextONEPROD, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextOneProdOEE, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextOneProdExecution, DbContextAPP_Electrolux>();
    //        //    container.RegisterType<IDbContextOneProdRTV, DbContextAPP_Electrolux>();
    //        //    accountInjectionConstructor = new InjectionConstructor(new DbContextAPP_Electrolux());
    //    }
    //    //private void EldisyInjections()
    //    //{
    //    //    container.RegisterType<DbContextAPP_Eldisy>();
    //    //    container.RegisterType<IDbContextBase, DbContextAPP_Eldisy>();
    //    //    container.RegisterType<IDbContextPFEP_Eldisy, DbContextAPP_Eldisy>();
    //    //    accountInjectionConstructor = new InjectionConstructor(new DbContextAPP_Eldisy());
    //    //}
    //    //private void GrandhomeInjections()
    //    //{
    //    //    container.RegisterType<IDbContextBase, DbContextAPP_Grandhome>();
    //    //    container.RegisterType<IDbContextONEPROD, DbContextAPP_Grandhome>();
    //    //    accountInjectionConstructor = new InjectionConstructor(new DbContextAPP_Grandhome());
    //    //}
    //    //private void DefaultInjections()
    //    //{
    //    //    container.RegisterType<DbContextAPP_>();
    //    //    container.RegisterType<IDbContextBase, DbContextAPP_>();
    //    //    accountInjectionConstructor = new InjectionConstructor(new DbContextAPP_());
    //    //}
    //    private void ClientIndependentInjections()
    //    {
    //        //InjectionFactory iFactory = new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication);
    //        builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
    //        builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
    //        builder.RegisterType<UserStore<User>>().As<IUserStore<User>>();
    //        builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>();
    //        builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
    //        //builder.RegisterType<IUserStore<User>, UserStore<User>>(accountInjectionConstructor);
    //        //builder.RegisterType<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>(accountInjectionConstructor);
    //        //container.RegisterType<IAuthenticationManager>(iFactory);
    //    }
    //}

}