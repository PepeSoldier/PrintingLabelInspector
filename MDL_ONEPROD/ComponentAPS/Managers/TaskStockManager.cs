using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDL_ONEPROD.Repo.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Manager
{
    public class TaskStockManager
    {
        UnitOfWorkOneprod uow;
        TaskManager taskManager;
        PartStockManager partStockManager;
        //ItemGroupManager partCategoryManager;
        string guid;

        public TaskStockManager(TaskManager taskMgr, string guid, UnitOfWorkOneprod unitOfWork)
        {
            this.uow = unitOfWork;
            this.guid = guid;

            taskManager = taskMgr;
            partStockManager = new PartStockManager(uow);
            //partCategoryManager = new ItemGroupManager();
        }

        //------------------------------------------------------------------------------------CONSIDER_STOCK2
        public void ConsiderStock(DateTime stockDate, ResourceOP area, List<Workorder> tasksToBeScheduled)
        {
            int line = NotificationManager.Instance.AddNotificationBlock("Oznaczanie zleceń pokrytych stokiem - " + area.Name, receiver: guid, id: -1);

            int stockQty, declaredQty;
            List<ItemInventory> listStock = uow.ItemInventoryRepo.GetListByDate(stockDate); //uow.PartStockRepo.GetStock(stockDate);

            foreach (ItemInventory partStock in listStock)
            {
                stockQty = partStock.Stock;
                declaredQty = tasksToBeScheduled.Where(x => x.Item.Code == partStock.Item.Code).Sum(x => (x.Qty_Produced - x.Qty_Used));

                ConsiderStock_UndoDeclaration(tasksToBeScheduled, partStock, 0, declaredQty);
                ConsiderStock_AddDeclaration(tasksToBeScheduled, partStock, stockQty);

                //if(declaredQty < stockQty)
                //{
                //    ConsiderStock_AddDeclaration(tasksToBeScheduled, partStock, stockQty);
                //}
                //if(declaredQty > stockQty)
                //{
                //    ConsiderStock_UndoDeclaration(tasksToBeScheduled, partStock, stockQty, declaredQty);
                //}
            }

            NotificationManager.Instance.AddNotificationBlock("Oznaczanie zleceń pokrytych stokiem - " + area.Name, status: "100%", receiver: guid, id: line);
        }
        private void ConsiderStock_UndoDeclaration(List<Workorder> tasksToBeScheduled, ItemInventory partStock, int stockQty, int declaredQty)
        {
            Workorder task;
            List<Workorder> declaredTasks = tasksToBeScheduled.Where(x => x.Item.Code == partStock.Item.Code && (x.Qty_Produced - x.Qty_Used) > 0).OrderBy(x=>x.DueDate).ToList();
            int overDeclaration = declaredQty - stockQty;
            int toUndeclare = 0;
            int j = declaredTasks.Count;

            while (overDeclaration > 0 && j > 0)
            {
                //prod      10  7   80
                //overdecl  6   10  10
                //toundecl  6   7   10
                toUndeclare = Math.Min(overDeclaration, declaredTasks[j-1].Qty_Produced - declaredTasks[j - 1].Qty_Used);
                if (toUndeclare > 0)
                {
                    task = uow.WorkorderRepo.GetById(declaredTasks[j-1].Id);
                    uow.WorkorderRepo.WorkorderQty_SetReady(task, task.Qty_Produced - toUndeclare);
                }
                overDeclaration -= toUndeclare;

                j--;
            }
        }
        private void ConsiderStock_AddDeclaration(List<Workorder> tasksToBeScheduled, ItemInventory partStock, int stockQty)
        {
            int todeclare;
            int j = 0;
            Workorder wo;
            while (stockQty > 0 && j < tasksToBeScheduled.Count)
            {
                if (partStock.Item.Code == tasksToBeScheduled[j].Item.Code)
                {
                    wo = uow.WorkorderRepo.GetById(tasksToBeScheduled[j].Id);
                    if (wo != null)
                    {
                        todeclare = Math.Min(wo.Qty_Total - wo.Qty_Used, stockQty);
                        if (todeclare > 0)
                            uow.WorkorderRepo.WorkorderQty_Declare(wo, todeclare);
                        stockQty = Math.Max(stockQty - (todeclare), 0);
                    }
                }
                j++;
            }
        }

        public void MoveStock(List<Workorder> tasksToBeScheduled)
        {
            int line = NotificationManager.Instance.AddNotificationBlock("Uwzględnianie stocku...", receiver: guid, id: -1);

            List<ItemInventory> listStock = new TaskStockMonitorManager(uow).GetCurrentStock();
            int stockQty, declaredQty;

            foreach (ItemInventory partStock in listStock)
            {
                stockQty = partStock.Stock;
                declaredQty = tasksToBeScheduled.Where(x => x.Item.Code == partStock.Item.Code).Sum(x => (x.Qty_Produced - x.Qty_Used));

                ConsiderStock_UndoDeclaration(tasksToBeScheduled, partStock, 0, stockQty);
                ConsiderStock_AddDeclaration(tasksToBeScheduled, partStock, stockQty);
            }

            NotificationManager.Instance.AddNotificationBlock("Uwzględnianie stocku... 100%", receiver: guid, id: line);
        }

        public void ConsiderInventory(List<Workorder> tasksToBeScheduled, DateTime stockDate)
        {
            //int line = NotificationManager.Instance.AddNotificationBlock("Uwzględnianie stocku...", receiver: guid, id: -1);

            List<ItemInventory> listStock = uow.ItemInventoryRepo.GetListByDate(stockDate);
            int stockQty, declaredQty;

            foreach (ItemInventory partStock in listStock)
            {
                if (!partStock.isStockApplied && partStock.Stock > 0)
                {
                    stockQty = partStock.Stock;
                    declaredQty = tasksToBeScheduled.Where(x => x.Item.Code == partStock.Item.Code).Sum(x => (x.Qty_Produced - x.Qty_Used));

                    ConsiderStock_UndoDeclaration(tasksToBeScheduled, partStock, 0, declaredQty);
                    ConsiderStock_AddDeclaration(tasksToBeScheduled, partStock, stockQty);

                    partStock.isStockApplied = true;
                    uow.ItemInventoryRepo.Update(partStock);
                }
            }
            //NotificationManager.Instance.AddNotificationBlock("Uwzględnianie stocku... 100%", receiver: guid, id: line);
        }
    }
}