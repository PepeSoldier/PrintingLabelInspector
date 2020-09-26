using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_PRD.Model
{
    public class KPI
    {
        //private double plannedOrderCount;
        //private double plannedOrderSum;
        //private double orderErrorSum;
        //private double orderBadSequenceCounter;
        //private double OrderBadSequenceSum;

        private List<OrderArchiveModel> OrdersPlanned;
        private List<OrderArchiveModel> ordersRemoved;
        private List<OrderArchiveModel> ordersInserted;
        private List<OrderArchiveModel> ordersBadSequence;
        private List<OrderArchiveModel> OrdersBadSeqAndInserted;
        private List<OrderArchiveModel> orderWithErrorTotal;
        public List<OrderArchiveModel> OrdersWithErrorTotal { get { return orderWithErrorTotal; } }

        public double Dsa 
        { 
            get 
            {
                return CalculateDSA();
            } 
        }
        public double CounterAccuracy { 
            get 
            {
                return CalculateBadSeqCnt();
            } 
        }
        public double SumAccuracy
        {
            get
            {
                return CalculateBadSeqSum();
            }
        }
        public double PSI
        {
            get
            {
                return CalculatePSI();
            }
        }

        public KPI()
        {
            orderWithErrorTotal = new List<OrderArchiveModel>();
            OrdersPlanned = new List<OrderArchiveModel>();
            ordersRemoved = new List<OrderArchiveModel>();
            ordersInserted = new List<OrderArchiveModel>();
            ordersBadSequence = new List<OrderArchiveModel>();
            OrdersBadSeqAndInserted = null;

            //plannedOrderCount = 0;
            //plannedOrderSum = 0;
            //orderErrorSum = 0;
            //orderBadSequenceCounter = 0;
            //OrderBadSequenceSum = 0;


            //liczymy DSA
            //liczymy zgodność sekwencji pod kątem ilościowym 
            //liczymy zgodność sekwencji pod kątem sumarycznym
            //zgodnosc sekwencji:
            // zlecenie bylo planowane ale zniknęło z planu: błąd = qty
            // zlecenie było planowane ale jego sekrencja sie nie zgadza: błąd = qty 
            // zlecenie było planowane, jest w planie i sekwencja sie zgadza: błąd = 0
            // zlecenie nie było ale się pojawiło: błąd = qty
        }

        public void AddOrder(OrderArchiveModel order)
        {
            //plannedOrderCount++;
            //plannedOrderSum += orderQty;
            OrdersPlanned.Add(order);
        }
        public void AddOrderRemoved(OrderArchiveModel order)
        {
            //OrderBadSequenceSum += orderQty;
            //orderBadSequenceCounter++;
            //orderErrorSum += orderQty;
            ordersRemoved.Add(order);
            //orderWithErrorTotal.Add(order);
            AddErrorOrder(order);
        }
        public void AddOrderInserted(OrderArchiveModel order)
        {
            //OrderBadSequenceSum += orderQty;
            //orderBadSequenceCounter++;
            //orderErrorSum += orderQty;
            ordersInserted.Add(order);
            //orderWithErrorTotal.Add(order);
            AddErrorOrder(order);
        }
        public void AddBadSequence(OrderArchiveModel order)
        { 
            //orderBadSequenceCounter++;
            //OrderBadSequenceSum += orderQty;
            ordersBadSequence.Add(order);
            //orderWithErrorTotal.Add(order);
            AddErrorOrder(order);
        }

        public void PrepareReport()
        {
            var results = from p in orderWithErrorTotal
                          group p by p.Reason into g
                          select new { ReasonName = g.Key.Name, Sum = g.Sum(m => m.QtyRemain) };

        }

        private void AddErrorOrder(OrderArchiveModel order)
        {
            if(!orderWithErrorTotal.Contains(order))
            {
                orderWithErrorTotal.Add(order);
            }
        }

        public double CalculateDSA()
        {
            double plannedQty = OrdersPlanned.Sum(o => o.QtyPlanned);
            double errorsQty = ordersRemoved.Sum(o => o.QtyPlanned);
                   errorsQty += ordersInserted.Sum(o => o.QtyPlanned);
            
            return (plannedQty - errorsQty) / plannedQty;

        }
        public double CalculateBadSeqCnt()
        {
            if(OrdersBadSeqAndInserted == null)
            OrdersBadSeqAndInserted = ordersBadSequence.Union(ordersInserted).Union(ordersRemoved).ToList<OrderArchiveModel>();
            
            double plannedCounter = OrdersPlanned.Count();
            double badSeqCounter = OrdersBadSeqAndInserted.Count();

            return (plannedCounter - badSeqCounter) / plannedCounter;
        }
        public double CalculateBadSeqSum()
        {
            if(OrdersBadSeqAndInserted == null)
            OrdersBadSeqAndInserted = ordersBadSequence.Union(ordersInserted).Union(ordersRemoved).ToList<OrderArchiveModel>();

            double plannedQty = OrdersPlanned.Sum(o=>o.QtyPlanned);
            double badSeqSum = OrdersBadSeqAndInserted.Sum(o=>o.QtyPlanned);

            return (plannedQty - badSeqSum) / plannedQty;
        }
        public double CalculatePSI()
        {
            return Dsa * 0.3 + CounterAccuracy * 0.3 + SumAccuracy * 0.4;
        }

    }
}
