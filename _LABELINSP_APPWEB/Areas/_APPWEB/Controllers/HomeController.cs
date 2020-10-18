using _LABELINSP_APPWEB.Areas._APPWEB.Controllers;
using _LABELINSP_APPWEB.Areas.IDENTITY;
using MDLX_CORE.Interfaces;
using MDLX_CORE.Models.IDENTITY;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Repo.IDENTITY;

namespace _LABELINSP_APPWEB.Areas._APPWEB.Controllers
{
    [PasswordExpiredAttribute]
    public class HomeController : BaseController
    {
        private readonly UserRepo UserRepo;
        private readonly IDbContextCore db;
        public HomeController(IUserStore<User, string> userStore, IDbContextCore db)
        {
            this.db = db;
            UserRepo = new UserRepo(userStore, db);
        }
        public ActionResult Index()
        {
            User usr = UserRepo.FindById(User.Identity.GetUserId());
            if(usr != null)
            {
                TimeSpan ts = DateTime.Today - usr.LastPasswordChangedDate;
                if(PasswordExpiresInDays - ts.TotalDays <= 10 && PasswordExpiresInDays > 0)
                {
                    int expirationPassword = (int)(PasswordExpiresInDays - ts.TotalDays);
                    ViewBag.ExpirationPasswordInfo = "Hasło wygaśnie za: " + expirationPassword + " dni";
                }
            }
          
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        
    }
}