using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentHSE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Repo;

namespace MDL_OTHER.ComponentHSE.Repos
{
    public class SafetyCrossRepo : RepoGenericAbstract<SafetyCross>
    {
        protected new IDbContextOtherHSE db;

        public SafetyCrossRepo(IDbContextOtherHSE db) : base(db)
        {
            this.db = db;
        }

        public SafetyCross GetByDate(DateTime date)
        {
            return db.SafetyCrosses.FirstOrDefault(x => x.Date == date);
        }

        public int GetNumberOfDaysWithoutAccident()
        {
            var obj = db.SafetyCrosses.Where(x => x.State == SafetyCrossState.Accident).OrderByDescending(x => x.Date).Take(1).FirstOrDefault();
            if(obj != null)
            {
                return (DateTime.Now - obj.Date).Days;
            }
            else
            {
                return 0;
            }
            
        }

        public int GetRecordNumberOfDaysWithoutAccident()
        {
            List<SafetyCross> list = new List<SafetyCross>();
            list.Add(new SafetyCross() { Date = DateTime.Now.Date, State = SafetyCrossState.Accident });
            list.AddRange(db.SafetyCrosses.Where(x => x.State == SafetyCrossState.Accident).OrderByDescending(x => x.Date).ToList());

            int record = 0;

            if (list.Count > 1)
            {
                int recordTemp = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    recordTemp = Math.Abs((list[i].Date - list[i-1].Date).Days);
                    record = recordTemp > record ? recordTemp : record;
                }
            }
            else
            {
                SafetyCross obj = list.FirstOrDefault();
                record = (obj != null)? (DateTime.Now - obj.Date).Days : 999;
            }

            return record;
        }

    }
}
