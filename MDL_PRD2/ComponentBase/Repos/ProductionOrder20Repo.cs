using XLIB_COMMON.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using MDL_BASE.Models;
using MDL_PRD.Model;
using MDL_PRD.Interface;
using MDL_PFEP.Model;
using System.Diagnostics;
using MDLX_CORE.ComponentCore.Entities;

namespace MDL_PRD.Repo
{
    public class ProdOrder20Repo : RepoGenericAbstract<Prodorder20>
    {
        protected new IDbContextPRD db;

        public ProdOrder20Repo(IDbContextPRD db)
            : base(db)
        {
            this.db = db;
        }

        public override Prodorder20 GetById(int id)
        {
            return db.ProdOrder20.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<Prodorder20> GetList()
        {
            return db.ProdOrder20.OrderByDescending(x => x.Id);
        }
        public List<Prodorder20> GetListByIds(int[] ProdOrders)
        {
            DateTime now = DateTime.Now;

            return db.ProdOrder20.AsNoTracking()
                        .Where(x => ProdOrders.Contains(x.Id) 
                                && (x.PartQtyRemain > 0 || (x.PartQtyRemain <= 0 && x.Order.StartDate < now)))
                        .OrderBy(o => o.PartStartDate).ToList();
        }
        public void AddOrUpdate_Orders20 (ProductionOrder order)
        {
            List<Prodorder20> pb20List =  db.ProdOrder20.Where(x => x.OrderId == order.Id).OrderBy(o=>o.PartNumber).ToList();
            ProdOrderBatcher orderBatcher = new ProdOrderBatcher(order);

            if (!(pb20List.Count > 0))
            {
                pb20List = orderBatcher.BatchNew();   
            }
            else
            {
                pb20List = orderBatcher.BatchExisting(pb20List);
            }

            foreach (Prodorder20 pb20 in pb20List)
            {
                //AddOrUpdate(pb20);
                UpdateFast(pb20);
            }

            DeleteZeroQty();
        }

        public void UpdateFast(Prodorder20 p020)
        {
            //TODO: przydałby się test jednostkowy dla tego updejta na wypadek zmiany nazw kolumn

            if (p020.Id > 0)
            {
                db.Database.ExecuteSqlCommand(
                    "UPDATE PRD.ProdOrder_20 SET " +
                        "OrderId = " + p020.OrderId +
                        ",PartQty = " + p020.PartQty +
                        ",PartQtyRemain = " + p020.PartQtyRemain +
                        ",PartNumber = " + p020.PartNumber +
                        ",PartStartDate = ' " + p020.PartStartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    "WHERE Id = " + p020.Id
                );
            }
            else
            {
                if (p020.PartQty > 0)
                {
                    db.Database.ExecuteSqlCommand(
                    "INSERT INTO PRD.ProdOrder_20 (OrderId,PartQty,PartQtyRemain,PartNumber,PartStartDate) VALUES " +
                    "(" + p020.OrderId + ", " + p020.PartQty + ", " + p020.PartQtyRemain + ", " + p020.PartNumber + ", '" + p020.PartStartDate.ToString("yyyy-MM-dd HH:mm:ss") + "') "
                    );
                }
            }
        }

        public bool DeleteZeroQty()
        {
            int val = 0;
            try
            {
                val = db.Database.ExecuteSqlCommand("DELETE FROM " + Prodorder20.TableName + " WHERE PartQty <= 0");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + ". " + e.InnerException.Message);
                val = -1;
            }

            return (val != -1);
        }
    }
}
