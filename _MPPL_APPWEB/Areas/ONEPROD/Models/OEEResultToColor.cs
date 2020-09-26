using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public static class OeeResultToColor
    {
        public static Color AssignColor(decimal targetValue, int d)
        {
            Color colorTransp;
            if (d >= targetValue)
            {
                colorTransp = GetGreenColor(0.4);
            }
            else if (targetValue - d < 5)
            {
                colorTransp = GetRedColor(0.4); //GetYellowColor(0.4);
            }
            else
            {
                colorTransp = GetRedColor(0.4);
            }

            return colorTransp;
        }
        private static Color GetGreenColor(double transparency = 1)
        {
            Color color = System.Drawing.Color.FromArgb((int)(255 * transparency), 54, 242, 4);
            return color;
        }
        private static Color GetYellowColor(double transparency = 1)
        {
            Color color = System.Drawing.Color.FromArgb((int)(255 * transparency), 216, 198, 8);
            return color;
        }
        private static Color GetRedColor(double transparency = 1)
        {
            Color color = System.Drawing.Color.FromArgb((int)(255 * transparency), 226, 39, 20);
            return color;
        }
    }
}