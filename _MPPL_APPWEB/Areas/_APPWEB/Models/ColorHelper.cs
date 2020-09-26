using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models
{
    public static class ColorHelper
    {
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static Color GetColor(double transparency = 1)
        {
            Random random = new Random();
            Color color = System.Drawing.Color.FromArgb((int)(255 * transparency), random.Next(255), random.Next(255), random.Next(255));
            return color;
        }
        public static Color MapColorFromString(string colorString)
        {
            colorString = colorString == null ? "#888888" : colorString.ToLower();

            if (colorString.Contains("rgb"))
            {
                colorString = colorString.Replace("rgb", "").Replace("(", "").Replace(")", "");
                string[] colorRGB = colorString.Split(',');

                if (colorRGB.Length == 3)
                    return Color.FromArgb(255, Convert.ToInt32(colorRGB[0]), Convert.ToInt32(colorRGB[1]), Convert.ToInt32(colorRGB[2]));
                else
                    return Color.FromArgb(255, 128, 128, 128);
            }
            else if (colorString.Contains("#"))
            {
                return ColorTranslator.FromHtml(colorString.Length > 7 ? colorString.Substring(0,7) : colorString);
            }
            else
            {
                return Color.FromArgb(255, 128, 128, 128);
            }
        }
        public static Color ColorChangeTransparency(Color color, double transparency)
        {
            return System.Drawing.Color.FromArgb((int)(255 * transparency), color);
        }
        public static Color ColorChangeSaturation(Color color, double saturationPercent)
        {
            double hue, saturation, value;
            ColorToHSV(color, out hue, out saturation, out value);

            return ColorFromHSV(hue, saturation * saturationPercent, value);            
        }
        
        public static string ConvertToHtmlRGBA(Color color)
        {
            return "rgba(" + color.R + "," + color.G + "," + color.B + "," + ((decimal)color.A / (decimal)255).ToString().Replace(',', '.') + ")";
        }
        public static string ColorChangeTransparency(string colorString, double transparencyPercent)
        {
            return ConvertToHtmlRGBA(ColorChangeTransparency(MapColorFromString(colorString), transparencyPercent));
        }
        public static string ColorChangeSaturation(string colorString, double saturationPercent)
        {
            return ConvertToHtmlRGBA(ColorChangeSaturation(MapColorFromString(colorString), saturationPercent));
        }

    }
}