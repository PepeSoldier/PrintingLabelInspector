using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PRD.Entity;
using MDL_PRD.Interface;
using MDL_PRD.Repo;
using System;
using System.Collections.Generic;

namespace MDL_PRD.Model
{
    public class ProdOrderScheduler
    {
        private CapacityRepo capacityRepo;
        //private List<Calendar> Capacities;
        private Calendar Capacity;
        private string lineName;
        
        public ProdOrderScheduler(IDbContextPRD context, string lineName)
        {
            this.lineName = lineName;
            capacityRepo = new CapacityRepo(context);
        }

        public List<ProductionOrder> CalculateNewStartTime(List<ProductionOrder> prodOrders, DateTime? firstOrderStartTime = null)
        {
            if (!(prodOrders.Count > 0)) return prodOrders;

            prodOrders.RemoveAll(x => x == null);
            DateTime time = CalcInitialTime(prodOrders, firstOrderStartTime);
            Capacity = capacityRepo.GetNextOpenedTimeWindow(time.AddHours(-6).Date, lineName);

            foreach (ProductionOrder po in prodOrders)
            {
                po.StartDate = time;
                time = CalculateEndDate(time, po.QtyRemain);
                po.EndDate = time;
            }

            return prodOrders;
        }
        private DateTime CalcInitialTime(List<ProductionOrder> prodOrders, DateTime? firstOrderStartTime)
        {
            //TODO: wysypał się przy cofaniu przeniesienia na inną linię
            //TODO: automatyczna notka przy zmianie sekwencji i nformacja dodatkowa przy przesunieciu na inną linię.

            if(firstOrderStartTime != null)
            {
                return firstOrderStartTime.Value;
            }
            else
            {
                for(int i = 0; i< prodOrders.Count; i++)
                {
                    if (prodOrders[i] != null)
                    {
                        return prodOrders[i].StartDate;
                    }
                }
            }
            return DateTime.Now;
        }
        private DateTime CalculateEndDate(DateTime currentTime, int orderQty)
        {
            int processingTimeSec = 0;
            DateTime startTime = currentTime;
            DateTime endDate = currentTime;
            
            processingTimeSec = CalcProcessingTime(orderQty);
            endDate = endDate.AddSeconds(processingTimeSec);

            while(endDate > Capacity.EndTime)
            {
                orderQty = orderQty - CountQtyToEndOfSlot(startTime);
                //processingTimeSec = (int)(tempEndTime - Capacity.EndTime).TotalSeconds;
                Capacity = capacityRepo.GetNextOpenedTimeWindow(Capacity.EndTime, lineName);
                processingTimeSec = CalcProcessingTime(orderQty);
                startTime = Capacity.StartTime;
                //TODO: capacity może byc null. trzeba to obsłużyć
                endDate = Capacity.StartTime.AddSeconds(processingTimeSec);
            }

            return endDate;
        }
        private int CountQtyToEndOfSlot(DateTime startTime)
        {
            return (int)((int)(Capacity.EndTime - startTime).TotalSeconds / Capacity.CycleTime1);
        }
        private int CalcProcessingTime(int qty)
        {
            return (int)(qty * Capacity.CycleTime1);
        }

        public List<ProductionOrder> ShiftOrders(List<ProductionOrder> prodOrders, int shiftSeconds)
        {
            if (!(prodOrders.Count > 0)) return prodOrders;

            prodOrders.RemoveAll(x => x == null);

            foreach (ProductionOrder po in prodOrders)
            {
                po.StartDate = po.StartDate.AddSeconds(shiftSeconds);
                po.EndDate = po.EndDate.AddSeconds(shiftSeconds);
            }

            return prodOrders;
        }
    }
}