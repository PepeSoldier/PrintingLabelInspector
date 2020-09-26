using MDLX_CORE.ComponentCore.Entities;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;

namespace MDL_PFEP.Model
{
    public class ProdOrderBatcher
    {
        int batchSize;
        int partNumber;
        int totalQty;
        int remainQty;
        int tempQtyRemain;
        int currentPartQty;
        int cycleTime;
        bool checkCalendar;
        ProductionOrder order;

        DateTime tempStartDate;
        DateTime breakStartTime;
        DateTime breakEndTime;
        
        public ProdOrderBatcher(ProductionOrder order)
        {
            this.order = order;
            batchSize = 20;
            partNumber = 1;
            totalQty = order.QtyPlanned;
            remainQty = order.QtyRemain;
            tempQtyRemain = 0;
            currentPartQty = 0;
            cycleTime = (int) ((order.QtyRemain > 0)? (order.EndDate - order.StartDate).TotalSeconds / order.QtyRemain : 0);
            checkCalendar = cycleTime > 100;
            cycleTime = cycleTime > 100 ? 38 : cycleTime;
            tempStartDate = order.StartDate;

            if (checkCalendar)
            {
                EstimateBreakTime();
            }
        }

        public List<Prodorder20> BatchNew()
        {
            List<Prodorder20> pb20List = new List<Prodorder20>();

            while (totalQty > 0)
            {
                currentPartQty = (totalQty >= batchSize) ? batchSize : (totalQty > 0) ? totalQty : 0;
                tempQtyRemain = currentPartQty;

                if (totalQty > remainQty) //sztuki zrobione lub cześciowo zrobione z batcha
                {
                    tempQtyRemain = (totalQty - remainQty > currentPartQty) ? 0 : currentPartQty - (totalQty - remainQty);
                }

                pb20List.Add(
                    new Prodorder20
                    {
                        OrderId = order.Id,
                        PartNumber = partNumber,
                        PartQty = currentPartQty,
                        PartQtyRemain = tempQtyRemain,
                        PartStartDate = tempStartDate
                    });

                partNumber++;
                totalQty -= batchSize;
                CalculateNextStartTime();
            }

            return pb20List;
        }
        public List<Prodorder20> BatchExisting(List<Prodorder20> pb20List)
        {
            for (int i = 0; i < pb20List.Count; i++)
            {
                currentPartQty = (totalQty >= batchSize) ? batchSize : (totalQty > 0)? totalQty : 0;
                tempQtyRemain = currentPartQty;

                if (totalQty > remainQty) //sztuki zrobione lub cześciowo zrobione z batcha
                {
                    tempQtyRemain = (totalQty - remainQty > currentPartQty) ? 0 : currentPartQty - (totalQty - remainQty);
                }

                pb20List[i].PartQty = currentPartQty;
                pb20List[i].PartQtyRemain = tempQtyRemain;
                pb20List[i].PartStartDate = tempStartDate;

                partNumber++;
                totalQty -= batchSize;
                CalculateNextStartTime();
            }

            if(totalQty > 0) //If totalQty was increased than additional batches must be created
            {
                pb20List.AddRange(BatchNew());
            }

            return pb20List;
        }

        private void CalculateNextStartTime()
        {
            tempStartDate = tempStartDate.AddSeconds(cycleTime * tempQtyRemain);

            if (checkCalendar)
            {
                if(tempStartDate >= breakStartTime && tempStartDate < breakEndTime)
                {
                    TimeSpan ts = (tempStartDate - breakStartTime);
                    tempStartDate = breakEndTime;
                    tempStartDate = tempStartDate.AddMinutes(ts.TotalMinutes);
                }
            }
        }
        private void EstimateBreakTime()
        {
            int BreakStartHour = (int)(Math.Ceiling((double)order.StartDate.Hour / 8) * 8 - 2);
            int BreakEndHour = (int)(Math.Ceiling((double)order.EndDate.Hour / 8) * 8 - 2);

            breakStartTime = order.StartDate.Date.AddHours(BreakStartHour);
            breakEndTime = order.EndDate.Date.AddHours(BreakEndHour);
        }
    }
}