using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{

    public abstract class ChartDataPreparer
    {
        protected DateTime dateFrom;
        protected DateTime dateTo;
        protected int intervalInHours;

        public ChartDataPreparer()
        {
            this.dateFrom = DateTime.Now.Date.AddDays(-7).AddHours(6);
            this.dateTo = dateFrom.AddDays(7);
            this.intervalInHours = 24;
        }
        public ChartDataPreparer(DateTime dateFrom, DateTime dateTo, int intervalInHours)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.intervalInHours = intervalInHours;
        }

        public virtual List<string> PrepareLabels()
        {
            List<string> labels = new List<string>();
            string label = string.Empty;

            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                label = PrepareDataLabel_Base(date, intervalInHours);
                label = PrepareDataLabel_AddWeekNum(label, date, intervalInHours, dateFrom, dateTo);
                labels.Add(label);
            }

            return labels;
        }

        public static string PrepareDataLabel(DateTime date, int intervalInHours, DateTime dateFrom, DateTime dateTo)
        {
            string label = PrepareDataLabel_Base(date, intervalInHours);
            label = PrepareDataLabel_AddWeekNum(label, date, intervalInHours, dateFrom, dateTo);
            return label;
        }
        public static string PrepareDataLabel_Base(DateTime date, int intervalInHours)
        {
            string label;
            if (intervalInHours == 24)
            {
                label = date.ToString("ddd");
            }
            else if (intervalInHours == 8)
            {
                int hour = (int)date.Hour;
                string shift = (hour >= 22 || hour < 6) ? "III" : (hour >= 14) ? "II" : "I";
                label = date.ToString("ddd") + " " + shift;
            }
            else if (intervalInHours == 168)
            {
                //label = "'" + date.ToString("yy");
                int weeknum = Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                label = "w" + weeknum.ToString();
            }
            else
            {
                label = date.ToString("yyyy-MM-dd");
            }

            return label;
        }
        public static string PrepareDataLabel_AddWeekNum(string label, DateTime date, int intervalInHours, DateTime dateFrom, DateTime dateTo)
        {
            if ((dateTo - dateFrom).TotalDays > -1 && !(intervalInHours == 168))
            {
                int weeknum = Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                label += " w" + weeknum.ToString();
            }
            return label;
        }

       
    }

}