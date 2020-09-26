using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Clients
{
    public class AppClientUnknown : AppClientApbstract
    {
        public AppClientUnknown() : base()
        {
            ac = new MenuAccessCode[] { MenuAccessCode.COMMON_ACCESS };
        }
    }

}