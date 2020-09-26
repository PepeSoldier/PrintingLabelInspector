
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Common
{
    public class AtcsParam
    {
        IAlgorithm atcs;
        public static double IndexDefaultVal = -9999.99;

        //<summary>Param K1</summary>
        double K1 { get; set; }
        /*<summary>Param K2</summary>*/
        double K2 { get; set; }
        /*Average setup time */
        double AvgS { get; set; }
        /*Average processing time */
        double avgP { get; set; }
        /*<summary>Setup time Severity Factor<summary>*/
        double N { get; set; }
        /*Average task per machine on area */
        double U { get; set; }
        /* to be explained */
        double B { get; set; }
        /* Makespan - estimated */
        double CMax { get; set; }
        /* Min due date */
        DateTime Dmin { get; set; }
        /* Max due date */
        DateTime Dmax { get; set; }
        /* due date tightness Factor */
        double TF { get; set; }
        /* due date range Factor */
        double R { get; set; }

        public AtcsParam(IAlgorithm atcsCalculation)
        {
            atcs = atcsCalculation;
        }

        
        ///<summary>
        ///Oblicza wspolczynniki
        ///</summary>
        public void ComputeFactors()
        {
            double avgS = GetAvgS();
            double avgP = GetAvgP();
            double n = SetupTimeSeverityFactor(avgS, avgP);
            double u = atcs.TaskManager.Tasks_ToBeScheduled.Count / atcs.MachineManager.AreaMachinesNumber();
            double B = 0.4 + 10 / (u * u) - n / 7;
            //double CMax = estimateTheMakespan(B, avgS, avgP, u);
            double CMax = EstimateTheMakespan2(avgS);
            DateTime dmin = GetDmin();
            DateTime dmax = GetDmax();
            double tF = DueDateTightnessFactor(CMax);
            double R = DueDateRangeFactor(dmin, dmax, CMax);

            //K1 = computeK1(R, u);
            //K2 = computeK2(tF, n);
            K1 = ComputeK1A(R);
            K2 = ComputeK2A(tF, n);


            if (tF < 0.5)
                K1 = K1 - 0.5;

            if (n < 0.5 && u > 5)
                K1 = K1 - 0.5;
        }

        //tF - dueDateTightnessFactor
        //
        private double DueDateTightnessFactor(double Cmax)
        {
            TimeSpan ts;
            double ddtf = 0;
            double djSum = 0;
            int n;
            List<Workorder> taskToBeScheduled = atcs.TaskManager.Tasks_ToBeScheduled;

            for (n = 0; n < taskToBeScheduled.Count; n++)
            {
                ts = taskToBeScheduled[n].DueDate - atcs.CalcManager.CalculationDateStart;
                djSum += Convert.ToDouble(ts.Minutes); //Poprawic
            }

            ddtf = (double)1 - (double)djSum / ((double)n * (double)Cmax);

            return ddtf;
        }
        //R - dueDateRangeFactor
        //dmin - min duedate
        //dmax - max duedate
        private double DueDateRangeFactor(DateTime dmin, DateTime dmax, double Cmax)
        {
            TimeSpan ts = dmax - dmin;
            double R;
            R = (ts.TotalMinutes) / Cmax;
            return R;
        }
        //n - setup time severity factor
        //
        private double SetupTimeSeverityFactor(double avgS, double avgP)
        {
            return avgS / avgP;
        }
        //Cmax estimation
        //
        private double EstimateTheMakespan(double B, double avgS, double avgP, double u)
        {
            double Cmax = 0;
            Cmax = (B * avgS + avgP) * u;
            return Cmax;
        }

        private double EstimateTheMakespan2(double avgS)
        {
            double Cmax = 0;
            List<Workorder> taskToBeScheduled = atcs.TaskManager.Tasks_ToBeScheduled;

            int n = taskToBeScheduled.Count;

            for (int j = 1; j <= n; j++)
            {
                Cmax += taskToBeScheduled[j - 1].ProcessingTime;
            }

            Cmax = Cmax + Convert.ToDouble(n * avgS);

            return Cmax;
        }

        private double ComputeK1(double R, double u)
        {
            return 1.2 * Math.Log(u) - R;
        }
        private double ComputeK2(double tF, double n)
        {
            double A2 = 0;

            if (tF < 0.8)
                A2 = 1.8;
            else if (tF >= 0.8)
                A2 = 2;

            return tF / (A2 * Math.Sqrt(n));
        }

        private double ComputeK1A(double R)
        {
            double k1 = 1;
            if (R <= 0.5)
            {
                k1 = 4.5 + R;
            }
            else if (R > 0.5)
            {
                k1 = 6 - 2 * R;
            }

            return k1;
        }
        private double ComputeK2A(double tF, double n)
        {
            return tF / (2 * Math.Sqrt(n));
        }

        public double GetAvgS()
        {
            return atcs.SetupManager.AvgS(atcs.ResourceGroup.Id);
        }
        public double GetAvgP()
        {
            List<Workorder> taskToBeScheduled = atcs.TaskManager.Tasks_ToBeScheduled;
            int n = taskToBeScheduled.Count;
            double totalProcessingTime = 0;
            for (int j = 1; j <= n; j++)
            {
                totalProcessingTime += taskToBeScheduled[j - 1].ProcessingTime;
            }

            if (n > 0)
                return totalProcessingTime / (double)n;
            else
                return 1;
        }

        public DateTime GetDmin()
        {
            List<Workorder> taskToBeScheduled = atcs.TaskManager.Tasks_ToBeScheduled;

            if (taskToBeScheduled.Count > 0)
            {
                return taskToBeScheduled.Min(o => o.DueDate);
            }
            return atcs.CalcManager.CalculationDateStart;
        }
        public DateTime GetDmax()
        {
            List<Workorder> taskToBeScheduled = atcs.TaskManager.Tasks_ToBeScheduled;

            if (taskToBeScheduled.Count > 0)
            {
                return taskToBeScheduled.Max(o => o.DueDate);
            }
            return atcs.CalcManager.CalculationDateStart;
        }

        public double computeRankingIndex(double Wj, double Pj, DateTime rj, DateTime dj, DateTime t, double pAVG, double SLj, double sAVG, bool forward)
        {            
            if(K1 == 0)
            {
                NotificationManager.Instance.AddNotificationLog("WARNING! Param K1 = 0", receiver: atcs.CalcManager.Guid);
            }
            if(K2 == 0)
            {
                NotificationManager.Instance.AddNotificationLog("WARNING! Param K2 = 0", receiver: atcs.CalcManager.Guid);
            }
            if(pAVG == 0)
            {
                NotificationManager.Instance.AddNotificationLog("WARNING! Param pAVG = 0", receiver: atcs.CalcManager.Guid);
            }
            if(sAVG == 0)
            {
                NotificationManager.Instance.AddNotificationLog("WARNING! Param sAVG = 0", receiver: atcs.CalcManager.Guid);
            }

            //index = (Wj / Pj) * Math.Exp(((rj + Pj + t) / (K1 * pAVG))) * Math.Exp(-(m * SLj) / (K2 * sAVG));
            double index = 0;
            TimeSpan ts;

            if (forward)
            {
                ts = dj.AddSeconds(-Pj) - t;
                index = (Wj / Pj) * Math.Exp((Math.Max(ts.TotalSeconds,0) / (K1 * pAVG))) * Math.Exp(-(SLj) / (K2 * sAVG));
            }
            else
            {
                ts = t - dj.AddSeconds(Pj);
                index = (Math.Exp(-(Math.Max(ts.TotalSeconds, 0) / (K1 * pAVG)))) * (Math.Exp(-(SLj) / (K2 * sAVG)));
            }
            
            if(double.IsNaN(index))
            {
                NotificationManager.Instance.AddNotificationLog("WARNING! Index is NaN", receiver: atcs.CalcManager.Guid);
                return IndexDefaultVal;
            }

            return index;
        }
    }
}
