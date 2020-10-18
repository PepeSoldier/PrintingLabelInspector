using _LABELINSP_APPWEB.App_Start;
using Autofac;
using Autofac.Integration.Mvc;
using MDLX_CORE.Interfaces;
using MDLX_CORE.Models.IDENTITY;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Security.Principal;
using System.Web.Mvc;
using XLIB_COMMON.Repo.IDENTITY;

namespace _LABELINSP_APPWEB.Areas.IDENTITY
{
    public class PasswordExpiredAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private static readonly int PasswordExpiresInDays = Properties.Settings.Default.PasswordExpiresInDays;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IPrincipal user = filterContext.HttpContext.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                IDbContextCore dbiLOGIS = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextCore>();
                var UserRepo = new UserRepo(new ApplicationUserStore<User>((DbContext)dbiLOGIS), dbiLOGIS);
                User usr = UserRepo.FindById(user.Identity.GetUserId());

                if (usr != null)
                {
                    TimeSpan ts = DateTime.Today - usr.LastPasswordChangedDate;

                    // If true, that means the user's password expired
                    // Let's force him to change his password before using the application

                    if (ts.TotalDays > PasswordExpiresInDays && PasswordExpiresInDays > 0)
                    {
                        //filterContext.HttpContext.Response.Redirect("Login");
                        filterContext.HttpContext.Response.Redirect(
                            string.Format("~/{0}/{1}/{2}?{3}", "IDENTITY", "Manage", "ChangePassword",
                            "reason=expired"), false);
                        //return;
                    }
                    else if (PasswordExpiresInDays - ts.TotalDays <= 10)
                    {
                        //int numberOfDaysToChangePassword = (int)(PasswordExpiresInDays - ts.TotalDays);
                        //filterContext.HttpContext.Response.Redirect(
                        //    string.Format("~/{0}/{1}?{2}", "Home", "Index",
                        //    "expirationDays=" + numberOfDaysToChangePassword), false);
                    }

                    base.OnAuthorization(filterContext);
                }
                else
                {
                    //base.
                    //Debug.WriteLine()
                    Console.WriteLine("nieznany user");
                }
            }
        }
    }
}