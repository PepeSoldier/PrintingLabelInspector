using _MPPL_WEB_START.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace _MPPL_WEBAPI.Models
{
    public class DbContext_Elux
    {
        public DbConnector db;

        public DbContext_Elux()
        {
            string cstr = ConfigurationManager.ConnectionStrings["DevKConnection"].ConnectionString;
            db = new DbConnector(Logger2FileSingleton.Instance, cstr);
        }
    }
}