using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class CalculationManager
    {
        public List<ResourceOP> Areas { get; set; }
        public List<AtcsAlgorithm> Calculations { get; set; }
        public DateTime CalculationDateStart { get; set; }
        public DateTime CalculationDateEnd { get; set; }
        public bool ConsiderCalendar { get; set; }
        public bool BatchTasks { get; set; }
        public string Guid { get; set; }


        public CalculationManager(IDbContextOneprodAPS dbAPS, IDbContextOneprodWMS dbWMS, List<ResourceOP> areas)
        {
            Calculations = new List<AtcsAlgorithm>();
            Areas = areas;
            
            foreach (ResourceOP area in Areas)
            {
                if (area.StageNo > 0)
                {
                    AtcsAlgorithm alg = new AtcsAlgorithm(dbAPS, dbWMS, this, area);
                    Calculations.Add(alg);
                }
            }
        }

        //public void Start()
        //{
        //    LoadLastCalculations();
        //    StartCalculations();
        //}

        public void LoadLastCalculations()
        {
            foreach(AtcsAlgorithm alg in Calculations)
            {
                if (!alg.ResourceGroup.Consider)
                {
                    alg.CalculationLoadLast();
                }
            }
        }
        public void StartCalculations()
        {
            //foreach (AtcsAlgorithm alg in Calculations)
            //{
            //    if (alg.Area.Consider)
            //    {
            //        alg.CalculationRuleStart();
            //    }
            //}
        }
    }
}