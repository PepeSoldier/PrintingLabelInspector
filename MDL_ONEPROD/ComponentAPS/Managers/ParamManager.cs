using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MDL_ONEPROD.Manager
{
    public class ParamManager : XLIB_COMMON.Repo.Repo
    {
        public ParamManager(IDbContextOneprod db) : base(db)
        {
            params1 = db.Params.ToList();

            //decimal d = 5.098929432m;
            //decimal d5 = System.Math.Round(d, 5);

        }

        public DateTime ProdStartDate {
            //get { return Convert.ToDateTime(IsNull(params1.FirstOrDefault(p => p.Name == "ProdStartDate"), DateTime.Now.Date.AddDays(-2).ToString("yyyy-mm-dd"))); }
            //set { Param prm = params1.FirstOrDefault(p => p.Name == "ProdStartDate"); prm.Value = value.ToString("yyyy-MM-dd HH:mm"); AddOrUpdateObject(prm); }
            get { return getDateValue("ProdStartDate", DateTime.Now.Date ); }
            set { setDateValue("ProdStartDate", value); }
        }
        public DateTime ProdEndDate {
            //get { return Convert.ToDateTime(IsNull(params1.FirstOrDefault(p => p.Name == "ProdEndDate"), DateTime.Now.Date.AddDays(5).ToString("yyyy-mm-dd"))); }
            //set { Param prm = params1.FirstOrDefault(p => p.Name == "ProdEndDate"); prm.Value = value.ToString("yyyy-MM-dd HH:mm"); AddOrUpdateObject(prm); }
            get { return getDateValue("ProdEndDate", DateTime.Now.Date); }
            set { setDateValue("ProdEndDate", value); }
        }
        public DateTime ScheduleTime {
            get { return getDateValue("ScheduleTime", DateTime.Now.Date); }
            set { setDateValue("ScheduleTime", value); }
        }
        public int ScheduleUserId {
            get { return getIntValue("ScheduleUserId", 1); }
            set { setIntValue("ScheduleUserId", value); }
        }
        public int GanttChartZoom
        {
            get { return getIntValue("GanttChartZoom", 2); }
            set { setIntValue("GanttChartZoom", value); }
        }

        private List<Param> params1;

        private void setDateValue(string paramName, DateTime paramValue)
        {
            Param prm = params1.FirstOrDefault(p => p.Name == paramName);

            if (prm == null)
                prm = new Param { Name = paramName };

            prm.Value = paramValue.ToString("yyyy-MM-dd HH:mm");
            AddOrUpdateObject(prm);
        }
        private DateTime getDateValue(string paramName, DateTime defaultValue)
        {
            Param prm = params1.FirstOrDefault(p => p.Name == paramName);
            if (prm == null)
                return defaultValue;
            else
                return Convert.ToDateTime(prm.Value);
        }

        private void setIntValue(string paramName, int paramValue)
        {
            Param prm = params1.FirstOrDefault(p => p.Name == paramName);

            if (prm == null)
                prm = new Param { Name = paramName };

            prm.Value = paramValue.ToString();
            AddOrUpdateObject(prm);
        }
        private int getIntValue(string paramName, int defaultValue)
        {
            Param prm = params1.FirstOrDefault(p => p.Name == paramName);
            if (prm == null)
                return defaultValue;
            else
                return Convert.ToInt32(prm.Value);
        }
        
        private string IsNull(Param input, string defaultVal)
        {
            return input != null ? input.Value : defaultVal;
        }

    }
}
