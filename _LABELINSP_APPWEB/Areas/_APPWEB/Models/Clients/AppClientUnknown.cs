using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _LABELINSP_APPWEB.Areas._APPWEB.Models.Clients
{
    public class AppClientUnknown : AppClientApbstract
    {
        public AppClientUnknown() : base()
        {
            ac = new MenuAccessCode[] { MenuAccessCode.COMMON_ACCESS };
        }
    }

}