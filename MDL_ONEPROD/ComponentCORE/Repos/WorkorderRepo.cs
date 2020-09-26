using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class WorkorderRepo : RepoGenericAbstract<Workorder>
    {
        protected new IDbContextOneprod db;
        
        public WorkorderRepo(IDbContextOneprod db) : base(db)
        {
            this.db = db;
        }

        public override Workorder GetById(int id)
        {
            return db.Workorders.FirstOrDefault(t => t.Id == id);
        }
        public Workorder GetFirstNotCoveredTask(int partId)
        {
            List<Workorder> list = db.Workorders.Where(
                x => x.ItemId == partId && 
                x.Qty_Produced < x.Qty_Total &&
                DbFunctions.AddSeconds(x.DueDate, x.ProcessingTime) >= DateTime.Now)
            .OrderBy(x => x.DueDate).Take(1).ToList();

            if(list.Count > 0)
            {
                return list.FirstOrDefault();
            }
            else
            {
                return new Workorder { DueDate = DateTime.Now.AddDays(7), Qty_Total = 1, Qty_Produced = 0, Qty_Used = 0 };
            }
        }
        
        public override IQueryable<Workorder> GetList()
        {
            return db.Workorders.OrderByDescending(x => x.Id);
        }
        public IQueryable<Workorder> GetWorkorderOfResourceGroup(int resourceGroupId, DateTime from, DateTime to)
        {
            return db.Workorders.AsNoTracking()
                .Where(o => o.Item.ItemGroupOP.ResourceGroupOP.Id == resourceGroupId && (from <= o.ClientOrder.EndDate && o.ClientOrder.StartDate <= to))
                .OrderBy(t => t.ClientOrder.StartDate);
        }
        public IQueryable<Workorder> GetWorkorderOfResource_LimitedStatusProd(int resourceId, DateTime from, DateTime to)
        {
            return db.Workorders.AsNoTracking()
                            .Where(o => o.ResourceId == resourceId && (from <= o.EndTime && o.StartTime <= to) && (o.Status < TaskScheduleStatus.produced && o.Qty_Produced < o.Qty_Total))
                            .OrderBy(o => o.StartTime);
        }
        public IQueryable<Workorder> GetWorkordersOfResource(int resourceId, DateTime from, DateTime to)
        {
            return db.Workorders.AsNoTracking()
                    .Where(o => o.ResourceId == resourceId && 
                            (from <= o.EndTime && o.StartTime <= to) 
                            //&& o.Status < TaskScheduleStatus.used
                    )
                    .OrderBy(o => o.StartTime);
        }
        public IQueryable<Workorder> GetTasksbyAreaIdAndBatchNumber(int resourceGroupId, int batchNumber)
        {
            return db.Workorders.AsNoTracking()
                        .Where(o =>
                            o.Resource.ResourceGroupId == resourceGroupId &&
                            o.BatchNumber == batchNumber &&
                            //o.Status < TaskScheduleStatus.used &&
                            (o.Item != null || o.UniqueNumber == "SETUP"))
                        .OrderBy(o => o.StartTime);
        }
        public IQueryable<Workorder> GetWorkordersByBatchNumbers(int resourceGroupId, List<int> batchNumbers)
        {
            var query = db.Workorders.Where(x =>
                    x.Resource.ResourceGroupId == resourceGroupId &&
                    batchNumbers.Contains(x.BatchNumber));

            return query;
        }
        public IQueryable<Workorder> GetworkorderChildrens(Workorder wo)
        {
            IQueryable<Workorder> children = null;

            if (wo != null && wo.ClientOrderId != null && wo.Item.ItemGroupOP != null && wo.Item.ItemGroupOP.ProcessId != null)
            {
                children = db.Workorders
                               .Where(t =>
                                   (t.ClientOrderId == wo.ClientOrderId) &&
                                   (t.Item.ItemGroupOP.Process.ParentId == wo.Item.ItemGroupOP.ProcessId)
                                );
            }
            else
            {
                children = db.Workorders.Where(t => t.Id < 0);
            }

            return children;
        }

        public List<WorkorderBatch> GetBatchesOfResourceGroup(ResourceOP resourceGroup)
        {
            List<int> BatchNumbers = db.Workorders.AsNoTracking()
                            .Where(o =>
                                o.Item.ItemGroupOP.ResourceGroupId == resourceGroup.Id &&
                                //o.Status < TaskScheduleStatus.used && 
                                o.BatchNumber > 0 &&
                                o.Item != null)
                            .OrderBy(o => o.StartTime).Select(x => x.BatchNumber).Distinct().ToList();

            List<WorkorderBatch> Batches = new List<WorkorderBatch>();

            foreach (int batchNo in BatchNumbers)
            {
                Batches.Add(GetBatchByNumberAndResourceGroupId((int)resourceGroup.Id, batchNo));
            }

            return Batches;
        }
        public List<WorkorderBatch> GetBatchesOfResource(ResourceOP machine, DateTime from, DateTime to)
        {
            List<int> BatchNumbers = GetBatchNumbers(machine.Id, from, to).ToList();

            List<WorkorderBatch> Batches = new List<WorkorderBatch>();

            foreach (int batchNo in BatchNumbers)
            {
                Batches.Add(GetBatchByNumberAndResourceGroupId((int)machine.ResourceGroupId, batchNo));
            }

            return Batches;
        }
        public WorkorderBatch GetBatchByNumberAndResourceGroupId(int areaId, int batchNumber)
        {
            List<Workorder> batchWorkorders = GetTasksbyAreaIdAndBatchNumber(areaId, batchNumber).ToList();
            WorkorderBatch batch = new WorkorderBatch { Number = batchNumber, Workorders = new List<Workorder>() };

            if (batchWorkorders != null && batchWorkorders.Count > 0)
            {
                Workorder tmpTask = batchWorkorders.FirstOrDefault(x => x.Item != null);

                batch.Workorders = batchWorkorders;
                batch.Qty = batch.Workorders.Sum(x => x.Qty_Total);
                batch.StartTime = batchWorkorders.Min(t => t.StartTime);
                batch.EndTime = batchWorkorders.Max(t => t.EndTime);
                batch.SetupTime = batch.Workorders.Where(x => x.Item == null).Sum(x => x.ProcessingTime);
                batch.ProcessingTime = (int)(batch.EndTime - batch.StartTime).TotalSeconds;
                batch.Color = (tmpTask != null && tmpTask.Item.ItemGroupOP.Color != null) ? tmpTask.Item.ItemGroupOP.Color : "gray";
                batch.ToolId = tmpTask != null ? tmpTask.ToolId : null;
                batch.ItemId = tmpTask != null ? tmpTask.ItemId : null;
                batch.ResourceId = tmpTask != null ? (int)tmpTask.ResourceId : 0;
            }

            return batch;
        }
        public IQueryable<int> GetBatchNumbers(int resourceId, DateTime from, DateTime to)
        {
            IQueryable<int> BatchNumbers = db.Workorders.AsNoTracking()
                                        .Where(o =>
                                            o.ResourceId == resourceId &&
                                            (from <= o.EndTime && o.StartTime <= to) &&
                                            //o.Status < TaskScheduleStatus.used && 
                                            o.Item != null)
                                        .OrderBy(o => o.StartTime).Select(x => x.BatchNumber).Distinct();
            return BatchNumbers;
        }

        public List<int> ConfirmWorkorderByTrolleyQty(int workorderId, int qty = -1)
        {
            Workorder wo = GetById(workorderId);
            int limit = 100;
            int partId = 0;
            List<int> updatedTaksIds = new List<int>();
            DateTime startTime = DateTime.Now;

            while (qty > 0 && limit > 0)
            {                
                if (wo == null)
                {
                    wo = db.Workorders.Where(x => x.StartTime > startTime && x.ItemId == partId).OrderBy(o=>o.StartTime).Take(1).FirstOrDefault();
                }
                else
                {
                    qty = WorkorderQty_Declare(wo, qty);

                    partId = Convert.ToInt32(wo.ItemId);
                    startTime = wo.StartTime;
                    updatedTaksIds.Add(wo.Id);
                    wo = null;
                }
                limit--;
            }

            return updatedTaksIds;
        }
        public void ConfirmWorkorder(Workorder wo, int qty = -1)
        {
            if (wo != null)
            {
                wo.Qty_Produced = Math.Min(qty, wo.Qty_Total);
                wo.Status = (wo.Qty_Remain == 0) ? TaskScheduleStatus.produced : (wo.Qty_Total == wo.Qty_Remain) ? TaskScheduleStatus.planned : TaskScheduleStatus.inProduction;
                Update(wo);
                WorkorderQty_UpdateChildrens(wo, wo.Qty_Produced);
            }
        }
        public int WorkorderQty_Declare(Workorder wo, int qty)
        {
            //Coś tu jest skopane. Powinny być funkcje dodeklarowujące x sztuk oraz ustawiające konkretną wyprodukowaną ilość
            if (wo != null)
            {
                int qtyToDeclare = Math.Min(qty, wo.Qty_Remain); //(qty > wo.Qty_Remain) ? wo.Qty_Remain : qty;
                int totalReadyQty = wo.Qty_Produced + qtyToDeclare;
                int restQty = qty - qtyToDeclare;
                WorkorderQty_SetReady(wo, totalReadyQty);
                return Math.Max(restQty, 0);
            }
            else
            {
                return qty;
            }
        }
        public decimal WorkorderQty_Undeclare(Workorder wo, decimal qty)
        {
            if (wo != null)
            {
                decimal qtyToUndeclare = Math.Min(qty, wo.Qty_Produced); //(qty > wo.Qty_Produced) ? wo.Qty_Produced : qty;
                decimal totalReadyQty = wo.Qty_Produced - qtyToUndeclare;
                decimal restQty = qty - qtyToUndeclare;
                WorkorderQty_SetReady(wo, totalReadyQty);
                return Math.Max(restQty, 0);
            }
            else
            {
                return qty;
            }
        }
        public void WorkorderQty_SetReady(Workorder wo, decimal readyQty)
        {
            //TODO: 20200624 przerzucić int na decimal
            if (wo != null)
            {
                wo.Qty_Produced = Math.Min((int)readyQty, wo.Qty_Total);
                wo.Status = StatusVerification(wo);
                Update(wo);
                WorkorderQty_UpdateChildrens(wo, wo.Qty_Produced);
            }
        }
        public void WorkorderQty_UpdateChildrens(Workorder parentWo, int parentReadyQty)
        {
            List<Workorder> childrenWorkorders = GetworkorderChildrens(parentWo).ToList();
            
            if (childrenWorkorders == null || !(childrenWorkorders.Count > 0)) return;

            foreach (Workorder wo in childrenWorkorders)
            {
                //Zaktualizuj zużytą ilość
                wo.Qty_Used = Math.Min(wo.Qty_Total, parentReadyQty);
                wo.Status = StatusVerification(wo);
                Update(wo);

                //20200624 - nie bedzie automatycznego dodeklarowania. Na wszystkich maszynach jest ekran operatora wiec moze to powielać deklaracje.
                ////dodeklaruj jeżeli trzeba
                //if(wo.Qty_Used > wo.Qty_Produced)
                //    WorkorderQty_SetReady(wo, wo.Qty_Used);
            }
        }
        public TaskScheduleStatus StatusVerification(Workorder wo, TaskScheduleStatus? defaultStatus = null)
        {
            if (wo.Qty_Used == wo.Qty_Total)
                return TaskScheduleStatus.used;
            if (wo.Qty_Used > 0)
                return TaskScheduleStatus.inUse;
            if (wo.Qty_Remain == 0)
                return TaskScheduleStatus.produced;
            if (wo.Qty_Remain < wo.Qty_Total)
                return TaskScheduleStatus.inProduction;
            if (wo.Qty_Produced == 0 && wo.StartTime.Year > 2000)
                return TaskScheduleStatus.planned;
            if (wo.Qty_Produced == 0 && wo.StartTime.Year < 2000)
                return TaskScheduleStatus.initial;

            return (defaultStatus != null)? (TaskScheduleStatus)defaultStatus : wo.Status;
        }


        //FROM ABSTRACT REPO
        public int Add(Workorder entity)
        {
            return base.Add(entity);
            //db.Entry(entity).State = System.Data.Entity.EntityState.Added;
            //db.SaveChanges();
            //return entity.Id;
            
        }
        public int Update(Workorder entity)
        {
            return base.Update(entity);
        }
        public int AddOrUpdate(Workorder entity)
        {
            if (entity.Id > 0)
            {
                return Update(entity);
            }
            else
            {
                return Add(entity);
            }
        }
        public void Delete(Workorder entity)
        {
            base.Delete(entity);

            //if (entity == null) return;
            //db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //}
        }

        public void ClearTasks(ResourceOP area)
        {
            db.Database.ExecuteSqlCommand(
                "UPDATE t " +
                    "SET t.ResourceId = null, " +
                    "t.StartTime = '1900-01-01 00:00', " +
                    "t.EndTime = '1900-01-01 00:00', " +
                    "t.[Status] = 0, " +
                    "t.ToolId = null, " +
                    "t.[OrderSeq] = 0, " +
                    "t.[BatchNumber] = 0, " +
                    "t.[Index] = -9999.99 " +
                  "FROM [ONEPROD].[CORE_Workorder] t " +
                  "LEFT JOIN [_MPPL].[MASTERDATA_Item] p ON t.ItemId = p.Id " +
                  "LEFT JOIN [_MPPL].[MASTERDATA_Item] pc ON p.ItemGroupId = pc.Id " +
                  //"WHERE t.[Status] < " + TaskScheduleStatus.produced + " AND pc.AreaId = " + area.Id + ""
                  "WHERE pc.ResourceGroupId = " + area.Id + " AND ClientOrderId IS NOT NULL"
                  );

            db.Database.ExecuteSqlCommand(
                "DELETE t FROM [ONEPROD].[CORE_Workorder] t " +
                    "LEFT JOIN [_MPPL].[MASTERDATA_Resource] m ON t.ResourceId = m.Id " +
                    "WHERE UniqueNumber = 'SETUP' AND m.ResourceGroupId = " + area.Id + "");

            if (area.Name == "Setup Line")
            {
                db.Database.ExecuteSqlCommand(
                    "DELETE t FROM [ONEPROD].[CORE_Workorder] t " +
                    "LEFT JOIN [_MPPL].[MASTERDATA_Resource] m ON t.ResourceId = m.Id " +
                    "WHERE m.ResourceGroupId = " + area.Id);

            }
        }
        public void UpdateByFastQuery(Workorder task)
        {
            string sql = "UPDATE ONEPROD.CORE_Workorder SET " +
                    "StartTime = '" + task.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    ",EndTime = '" + task.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    ",ResourceId = " + task.ResourceId +
                    ",OrderSeq = " + task.OrderSeq +
                    ",BatchNumber = " + task.BatchNumber +
                    ",Status = " + Convert.ToInt32(task.Status) +
                    (task.ToolId != null? ",ToolId = " + task.ToolId.ToString() : "") +
                    ",Qty_Produced = " + task.Qty_Produced +
                    ",DueDate = '" + task.DueDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    ",ReleaseDate = '" + task.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    ",ProcessingTime = " + task.ProcessingTime +
                " WHERE Id = " + task.Id;

            if (task.Id > 0)
            {
                db.Database.ExecuteSqlCommand(sql);
            }
        }
        public string GenerateNewWONumber()
        {
            string name = "WorkorderNumber";
            string tblname = "[CORE].[SystemVariables]";

            string queryU = "UPDATE t SET " +
                "t.[Value] = (SELECT ISNULL((SELECT CONVERT(int, [Value]) FROM " + tblname + " WHERE [Name] = '" + name + "'),0) +1) " +
                "FROM " + tblname + " t " +
                "WHERE t.[Name] = '" + name + "'";

            string queryS = "SELECT ISNULL((SELECT CONVERT(int, [Value]) FROM " + tblname + " WHERE [Name] = '" + name + "'),0) as val";
            string query = queryU + ";" + queryS;

            int serialNumber = 0;

            try
            {
                serialNumber = db.Database.SqlQuery<int>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return DateTime.Now.Year.ToString() + (1000000000 + serialNumber).ToString().Substring(1); //2019 000 000 000
        }
    }
}