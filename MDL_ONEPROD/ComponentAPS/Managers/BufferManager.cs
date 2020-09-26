
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class BufferManager
    {
        UnitOfWorkOneprodWMS uow;
        UnitOfWork_iLogis uow_iLogis;
        List<Warehouse> warehouses;
        List<WarehouseItem> warehouseItems;

        List<int> overflowAprove = new List<int>();
        List<int> overflowRefuse = new List<int>();
        bool considerBuffers = false;

        public BufferManager(IDbContextOneprodWMS dbWMS) : base()
        {
            uow = new UnitOfWorkOneprodWMS(dbWMS);
            uow_iLogis = new UnitOfWork_iLogis(dbWMS);
            warehouses = uow_iLogis.WarehouseRepo.GetListAsNoTracking().ToList();
            //TODO: 20200327 obsłużyć nowe podejśćie iLOGIS w ONEPROD
            warehouseItems = new List<WarehouseItem>();//uow_iLogis.WarehouseItemRepo.GetListAsNoTracking().ToList();

            //TODO: zmapowac w bazie box. (20180723 - kurwa o co tu chodziło?)
            //foreach(Buffor_BoxItemGroup bbpc in dtBoxIdItemGroupId)
            //{
            //    bbpc.BufforBox = uow.Buffor_BoxRepo.GetById(bbpc.BufforBoxID);
            //}
        }

        public bool CheckBoxAvailibility(int partCategoryId, int qty, DateTime time)
        {
            if (considerBuffers)
            {
                WarehouseItem bpc = warehouseItems.FirstOrDefault(b => b.ItemGroupId == partCategoryId);

                if (bpc != null)
                {
                    int boxId = bpc.WarehouseId;
                    int bpcId = bpc.Id;
                    double neededBoxes = (double)qty / Convert.ToDouble(bpc.QtyPerLocation);

                    if (neededBoxes < 1)
                    {
                        //sprawdz czy jest miejsce w obecnym boxie
                        if (checkForPlaceInCurrentBox(bpc, qty))
                        {
                            return true;
                        }
                        else
                        {
                            //NO - Sprawdz czy sa wolne boxy
                            if (checkFreeBoxesQty(boxId) >= neededBoxes)
                                return true;
                            else
                                return checkOverflow(boxId, time);
                        }
                    }
                    else
                    {
                        if (checkFreeBoxesQty(boxId) >= neededBoxes)
                        {
                            return true;
                        }
                        else
                        {
                            return checkOverflow(boxId, time);
                        }
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ReserveBox(int partCateogryId, int qty, DateTime time)
        {
            return true;
        }
        public bool ReleaseBox(int partCateogryId, int qty, DateTime time)
        {
            return true;
        }
        public void ReleaseBoxes(DateTime time)
        {
            //recalculate buffers considering all orders between last t and current t

            //DateTime orderDD;
            ////załozenie: zlecenia posortowane sa wg DD malejaco.
            //int r = 1;
            //if (time < bufforLastT)
            //{
            //    while (r < listTasksToBeScheduled.Count)
            //    {
            //        orderDD = listTasksToBeScheduled[r].DueDate; //poprawic

            //        //jezeli  nowT < DD  <= lastT 
            //        if (orderDD <= bufforLastT)
            //        {
            //            bufforControl.releaseBox(listTasksToBeScheduled[r].ANC, listTasksToBeScheduled[r].Qty, orderDD);
            //        }

            //        if (orderDD < time)
            //        {
            //            bufforLastT = time;
            //            return;
            //        }
            //        r++;
            //    }

            //    bufforLastT = time;
            //}
        }

        //---private----------------------------------------------------------------
        //--------------------------------------------------------------------------

        private bool checkForPlaceInCurrentBox(WarehouseItem bpc, int qtyToByAdded)
        {
            //int qtyPerBox = Convert.ToInt32(dtBuffor2.Rows[bpc]["QtyPerBox"]);
            //int qty = Convert.ToInt32(dtBuffor2.Rows[bpc]["Qty"]);
            //int maxStore = Convert.ToInt32(dtBuffor2.Rows[bpc]["MaxStore"]);

            //if (bpc.Qty < bpc.MaxStore)
            //{
            //    int rest = bpc.QtyPerLocation - (bpc.Qty % bpc.QtyPerLocation);

            //    if (qtyToByAdded <= rest)
            //        return true;
            //    else
            //        return false;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
        private double checkFreeBoxesQty(int boxID)
        {
            double totalBoxAvailable = TotalBoxexAvailable(boxID);
            double totalBoxInUsage = CheckTotalBoxInUsage(boxID);

            return (double)(totalBoxAvailable - totalBoxInUsage);
        }
        private double CheckTotalBoxInUsage(int boxID)
        {
            double totalBoxInUsage = 0;

            for (int i = 0; i < warehouseItems.Count; i++)
            {
                //TO DO obsluzyc brak przypisanego boxa
                if(warehouseItems[i].WarehouseId == boxID)
                {
                    totalBoxInUsage += Math.Ceiling(Convert.ToDouble(warehouseItems[i].Qty) / Convert.ToDouble(warehouseItems[i].QtyPerLocation));
                }
            }

            return totalBoxInUsage;
        }
        private int TotalBoxexAvailable(int boxID)
        {
            return warehouses.FirstOrDefault(x => x.Id == boxID).QtyOfSubLocations;   
        }
        private void UpdateTotalBoxUsage(int boxId)
        {
            for (int i = 0; i < warehouses.Count; i++)
            {
                if (warehouses[i].Id == boxId)
                {
                    warehouses[i].CurrentUsage = CheckTotalBoxInUsage(boxId);
                }
            }
        }
        private double getTotalBoxUsage(int boxId)
        {
            for (int i = 0; i < warehouses.Count; i++)
            {
                if (warehouses[i].Id == boxId)
                {
                    return warehouses[i].CurrentUsage;
                }
            }
            return 0;
        }
        private void updateLog(int bpcId, DateTime t)
        {
            WarehouseItem bpc = warehouseItems.FirstOrDefault(b=>b.Id == bpcId);
            if(bpc != null)
            {
                BufforLog bufforLog = new BufforLog();
                bufforLog.ANC = "";
                bufforLog.BoxId = bpc.Warehouse.Id;
                //bufforLog.MaxStore = bpc.MaxStore;
                bufforLog.ItemGroupId = bpc.ItemGroupId;
                bufforLog.Qty = bpc.Qty;
                bufforLog.UsedBoxes = Convert.ToInt32(bpc.Warehouse.CurrentUsage);
                bufforLog.TotalBoxes = bpc.Warehouse.QtyOfSubLocations;
                bufforLog.Time = t;
                //uow_iLogis.WarehouseItemRepo.Add(bufforLog);
            }
        }
        private bool overflowQuestion(int boxId)
        {
            //if (System.Windows.Forms.MessageBox.Show("Brakuje miejsca, chcesz przepelnic bufor? boxID: " + boxId, "Pytanie", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    overflowAprove.Add(boxId);
            //    return true;
            //}
            //else
            //{
            //    overflowRefuse.Add(boxId);
            //    return false;
            //}
            overflowAprove.Add(boxId);
            return true;
        }
        private bool checkOverflow(int boxId, DateTime t)
        {
            DateTime prodStartDate = new DateTime(); //trzeba podac parametr z atcs calc

            DayOfWeek dd = t.DayOfWeek;
            if (dd == DayOfWeek.Sunday || dd == DayOfWeek.Saturday || t < prodStartDate)
            {
                if (overflowRefuse.Contains(boxId))
                    return false;
                else if (overflowAprove.Contains(boxId))
                    return true;
                else
                    return overflowQuestion(boxId);
            }

            return false;
        }

    }
}
