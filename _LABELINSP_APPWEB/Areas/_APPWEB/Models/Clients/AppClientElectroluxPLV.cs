using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _LABELINSP_APPWEB.Areas._APPWEB.Models.Clients
{
    public class AppClientElectroluxPLV : AppClientApbstract
    {
        public AppClientElectroluxPLV() : base()
        {
            ac = new MenuAccessCode[] {
                    MenuAccessCode.FULL_ACCESS,
                };
        }

        public override void Configure_LABELINSP()
        {
            SettingsLABELINSP.Test = false;
        }
    }
}