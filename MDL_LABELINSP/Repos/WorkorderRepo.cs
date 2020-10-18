using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Models.Repos
{
    public class WorkorderRepo : RepoGenericAbstract<Workorder>
    {
        protected new IDbContextLabelInsp db;

        public WorkorderRepo(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        public override Workorder GetById(int id)
        {
            return db.Workorders.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<Workorder> GetList()
        {
            return db.Workorders.OrderBy(x => x.Id);
        }

        public Workorder Get(string serialNumber, string pnc)
        {
            Workorder wo = GetBySerialNumber(serialNumber);
            if (wo == null)
            {
                wo = GetUnknownWorkorderByPNC(pnc);
            }

            return wo;
        }
        public Workorder GetUnknownWorkorderByPNC(string pnc)
        {
            Workorder wo = null;
            wo = db.Workorders.FirstOrDefault(x => x.WorkorderNumber == pnc);

            if (wo == null)
            {
                wo = new Workorder()
                {
                    Id = 0,
                    ItemCode = pnc,
                    WorkorderNumber = pnc,
                    SerialNumberFrom = "0",
                    SerialNumberTo = "0",
                    ItemName = "",
                    Qty = 0
                };
                Add(wo);
            }

            return wo;
        }
        public Workorder GetBySerialNumber(string serialNumber)
        {
            bool isOK = int.TryParse(serialNumber, out int serialNumberInt);
            List<Workorder> results = new List<Workorder>();

            if (isOK)
            {
                results = db.Workorders.Where(x => x.SerialNumberFromInt <= serialNumberInt && serialNumberInt <= x.SerialNumberToInt).ToList();

                if (results.Count == 0)
                {
                    results.Add(ImportWorkorderFromExternalDatabase(serialNumberInt));
                }
            }

            return results.FirstOrDefault();
        }
        /// <summary>This method calls stored procedure connected to view in EPSS_2 database</summary>
        private Workorder ImportWorkorderFromExternalDatabase(int serialNumber)
        {
            Workorder wo = null;
            List<Workorder> results = new List<Workorder>();
            var _serialNumber = new SqlParameter("@serialNumber", serialNumber);

            try
            {
                results = db.Database.SqlQuery<Workorder>("EXEC [dbo].[GET_WORKORDER_DETAILS]  @serialNumber", _serialNumber).ToList();
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("WorkorderRepo.GetBySerialNumber Get wo Exception: " + ex.Message);
            }
            
            try
            {
                wo = results.FirstOrDefault();
                //wo.FirstInspectionDate = DateTime.Now;
                //wo.LastInspectionDate = DateTime.Now;

                int.TryParse(wo.SerialNumberFrom, out int sf);
                int.TryParse(wo.SerialNumberTo, out int st);

                wo.SerialNumberFromInt = sf;
                wo.SerialNumberToInt = st;

                Add(wo);
                return wo;
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("WorkorderRepo.GetBySerialNumber Add wo Exception: " + wo?.FirstInspectionDate.ToString() );
                Logger2FileSingleton.Instance.SaveLog("WorkorderRepo.GetBySerialNumber Add wo Exception: " + ex.Message);
                return null;
            }
        }
        
        public void UpdateStats(Workorder wo, bool allTestsPassed)
        {
            try
            {
                if (allTestsPassed)
                {
                    wo.SuccessfullInspections++;
                    wo.LastInspectionDate = DateTime.Now;
                }
                else
                {
                    wo.FailfullInspections++;
                }
                Update(wo);
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("WorkorderRepo.UpdateStats Exception: " + ex.Message);
            }
        }
    }
}