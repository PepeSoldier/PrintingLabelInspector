using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public static class CalendarMonths
    {
        public static List<CalendarMonth> Get()
        {
            List<CalendarMonth> l = new List<CalendarMonth>();

            DateTime dt = new DateTime(2000, 1, 1);

            for (int i = 0; i < 12; i++)
            {
                l.Add(new CalendarMonth { Id = i + 1, Name = (i+1).ToString() + ". " + dt.AddMonths(i).ToString("MMM") });
            }

            return l;
        }
    }

    public class CalendarMonth
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}