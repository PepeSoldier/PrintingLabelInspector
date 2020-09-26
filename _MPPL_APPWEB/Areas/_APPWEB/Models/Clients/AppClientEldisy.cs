using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Clients
{
    public class AppClientEldisy : AppClientApbstract
    {
        public AppClientEldisy() : base()
        {
            ac = new MenuAccessCode[] {
                    MenuAccessCode.COMMON_ACCESS,
                    MenuAccessCode.PFEP_PCKINSTR_ACCESS,
                    //MenuAccessCode.PFEP_PCKINST_ACCESS,
                };
        }
    }
    public class AppClientEldisy2 : AppClientApbstract
    {
        public AppClientEldisy2() : base()
        {
            ac = new MenuAccessCode[] {
                    MenuAccessCode.COMMON_ACCESS,
                    MenuAccessCode.PFEP_ACCESS,
                    MenuAccessCode.PFEP_PCKINSTR_ACCESS,
                    MenuAccessCode.ONEPROD_ACCESS,
                    //MenuAccessCode.ONEPROD_APS_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_REPORTS_ACCESS,
                    MenuAccessCode.ONEPROD_MES_ACCESS,
                    //MenuAccessCode.ONEPROD_MES_TRACE_ACCESS,
                    //MenuAccessCode.ONEPROD_RTV_ACCESS,
                    //MenuAccessCode.ONEPROD_WMS_ACCESS
                };
        }
    }
}