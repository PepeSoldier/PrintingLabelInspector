using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System;
using System.Linq;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Repos
{
    public class WorkorderLabelRepo : RepoGenericAbstract<WorkorderLabel>
    {
        protected new IDbContextLabelInsp db;

        public WorkorderLabelRepo(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        public override WorkorderLabel GetById(int id)
        {
            return db.WorkorderLabels.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<WorkorderLabel> GetList()
        {
            return db.WorkorderLabels.OrderBy(x => x.Id);
        }

        public WorkorderLabel GetOrCreate(Workorder wo, string serialNumber)
        {
            WorkorderLabel workorderLabel = null;

            try
            {
                workorderLabel = db.WorkorderLabels.FirstOrDefault(x => x.SerialNumber == serialNumber);

                if (workorderLabel == null)
                {
                    workorderLabel = new WorkorderLabel()
                    {
                        Id = 0,
                        SerialNumber = serialNumber,
                        Workorder = wo,
                        WorkorderId = wo.Id,
                        TimeStamp = DateTime.Now
                    };
                    Add(workorderLabel);
                }
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("WorkorderLabelRepo.Create problem. Model: "
                    + workorderLabel.Id + ", "
                    + workorderLabel.SerialNumber + ", "
                    + wo?.WorkorderNumber ?? "1600000000" + ", "
                    + workorderLabel.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")
                );
                //Logger2FileSingleton.Instance.SaveLog("ItemData: " + itemData.ExpectedName + ", " + itemData.ExpectedProductCode);
            }

            return workorderLabel;
        }

        public WorkorderLabel GetBySerialNumber(string serialNumber)
        {
            return db.WorkorderLabels.Where(x => x.SerialNumber == serialNumber).FirstOrDefault();
        }
    }
}