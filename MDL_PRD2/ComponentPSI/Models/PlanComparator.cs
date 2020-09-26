using MDL_PRD.Repo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MDL_PRD.Model
{
    public class PlanComparator
    {
        public KPI kpi;

        List<OrderArchiveModel> orderCurrent;
        List<OrderArchiveModel> orderArchive;

        public PlanComparator()
        {
            kpi = new KPI();
        }
        public PlanComparator(List<Model.OrderArchiveModel> orderArch, List<Model.OrderArchiveModel> orderCurnt)
        {
            this.orderArchive = orderArch;
            this.orderCurrent = orderCurnt;

            kpi = new KPI();
        }

        public void ReplaceData(List<Model.OrderArchiveModel> orderArch, List<Model.OrderArchiveModel> orderCurnt)
        {
            this.orderArchive = orderArch;
            this.orderCurrent = orderCurnt;
        }


        public void ResetKPIData()
        {
            kpi = new KPI();
        }
        public void CompareOrdersSequence()
        {
            if (orderCurrent.Count > 1)
            {
                for (int i = 1; i < orderCurrent.Count; i++)
                {
                    //dla kazdego orderu
                    //znajdz poprzednika
                    string actOrder = orderCurrent[i].OrderNo;
                    string prevOrder = orderCurrent[i - 1].OrderNo;
                    string prevOrderArch = "";

                    //znajdz korespondujacy order w archiwum 
                    //oraz znajdz poprzednika
                    Model.OrderArchiveModel actOrderArch = orderArchive.FirstOrDefault(a => a.OrderNo == actOrder);
                    int indexOf = orderArchive.IndexOf(actOrderArch);

                    if (indexOf > 0)
                    {
                        prevOrderArch = orderArchive[indexOf - 1].OrderNo;
                    }

                    //porownaj poprzedników 
                    if (prevOrder != prevOrderArch)
                    {
                        // oznacz (nie)prawidlowosc sekwecji
                        this.MarkPlanStatus(i, EnumStatus.BadSequence);
                        //frm.dgvOrders.Rows[i].Cells["Seq"].Value = "0";
                        //frm.dgvOrders.Rows[i].Cells["Seq"].Style.BackColor = Color.Orange;
                        kpi.AddBadSequence(orderCurrent[i]);
                    }
                    else
                    {
                        this.MarkPlanStatus(i, EnumStatus.GoodSequence);
                        //frm.dgvOrders.Rows[i].Cells["Seq"].Value = "1";
                        //frm.dgvOrders.Rows[i].Cells["Seq"].Style.BackColor = Color.LimeGreen;
                    }
                }
            }
        }
        public void CheckAdditionalOrders()
        {
            OrderArchiveModel ordArch = null;
            for (int i = 0; i < orderCurrent.Count; i++)
            {
                ordArch = orderArchive.FirstOrDefault(a => a.OrderNo == orderCurrent[i].OrderNo);

                if (ordArch == null)
                {
                    MarkPlanStatus(i, EnumStatus.Added);
                    //frm.dgvOrders.Rows[i].Cells["W"].Value = 1;
                    //frm.dgvOrders.Rows[i].Cells["W"].Style.BackColor = Color.Yellow;
                    kpi.AddOrderInserted(orderCurrent[i]);
                }
                else
                {
                    MarkPlanStatus(i, EnumStatus.OK);
                    //frm.dgvOrders.Rows[i].Cells["W"].Value = 0;
                    //frm.dgvOrders.Rows[i].Cells["W"].Style.BackColor = Color.LimeGreen;
                }
            }
        }
        public void CheckRemovedOrders()
        {
            string actOrderArch;
            for (int i = 0; i < orderArchive.Count; i++)
            {
                actOrderArch = orderArchive[i].OrderNo;
                kpi.AddOrder(orderArchive[i]);

                Model.OrderArchiveModel actOrder = orderCurrent.FirstOrDefault(a => a.OrderNo == actOrderArch);

                if (actOrder == null)
                {
                    this.MarkArchPlanStatus(i, EnumStatus.Removed);
                    //frm.dgvOrdersArch.Rows[i].Cells["WArch"].Value = 1;
                    //frm.dgvOrdersArch.Rows[i].Cells["WArch"].Style.BackColor = Color.OrangeRed;
                    kpi.AddOrderRemoved(orderArchive[i]);
                }
                else
                {
                    this.MarkArchPlanStatus(i, EnumStatus.OK);
                    //frm.dgvOrdersArch.Rows[i].Cells["WArch"].Value = 0;
                    //frm.dgvOrdersArch.Rows[i].Cells["WArch"].Style.BackColor = Color.LimeGreen;
                }
            }
        }
        public void CheckProductionSequence()
        {
            List<OrderArchiveModel> laaa = orderCurrent;
            List<OrderArchiveModel> laaa2 = laaa.OrderBy(o => o.FirstScanTime).ToList<OrderArchiveModel>();

            string orderNo = "";
            int seq = 0;
            int prevSeq = 0;
            OrderArchiveModel order;

            for (int i = 0; i < laaa.Count; i++)
            {
                laaa[i].ProdSeq = -1;
                orderNo = laaa[i].OrderNo;
                order = laaa2.FirstOrDefault(o => o.OrderNo == orderNo);

                if (order != null)
                {
                    if (order.FirstScanTime.Year > 2015)
                    {
                        seq = laaa2.IndexOf(order) + 1;
                        laaa[i].ProdSeqTmp = seq;

                        try
                        {
                            prevSeq = laaa[i - 1].ProdSeqTmp;
                        }
                        catch (Exception e)
                        {
                             Console.WriteLine(e.Message + ". " + e.InnerException != null? e.InnerException.Message : string.Empty);
                        }

                        if (prevSeq == seq - 1)
                            laaa[i].ProdSeq = 0;
                        else
                            laaa[i].ProdSeq = 4;

                    }
                }
            }
        }

        public void MarkArchPlanStatus(int row, EnumStatus status)
        {
            if (status == EnumStatus.Removed)
            {
                orderArchive[row].W = 1;
                //la[row].WArchBackColor = Color.OrangeRed.Name;
            }
            else if (status == EnumStatus.OK)
            {
                orderArchive[row].W = 0;
                //la[row].WArchBackColor = Color.LimeGreen.Name;
            }
        }
        public void MarkPlanStatus(int row, EnumStatus status)
        {
            if (status == EnumStatus.OK)
            {
                orderCurrent[row].W = 0;
                //l[row].WBackColor = Color.LimeGreen.Name;
            }
            else if (status == EnumStatus.BadSequence)
            {
                orderCurrent[row].Seq = 3;
                //l[row].SeqBackColor = Color.Orange.Name;
            }
            else if (status == EnumStatus.GoodSequence)
            {
                orderCurrent[row].Seq = 0;
                //l[row].SeqBackColor = Color.LimeGreen.Name;
            }
            else if (status == EnumStatus.Added)
            {
                orderCurrent[row].W = 2;
                //l[row].WBackColor = Color.Yellow.Name;
            }

        }
    }
}
