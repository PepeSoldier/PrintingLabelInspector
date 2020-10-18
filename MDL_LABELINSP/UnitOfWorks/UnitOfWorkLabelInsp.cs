using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using MDL_LABELINSP.Models.Repos;
using MDL_ONEPROD.Repo;

namespace MDL_LABELINSP.UnitOfWorks
{
    public class UnitOfWorkLabelInsp : UnitOfWorkMasterData
    {
        private IDbContextLabelInsp db;

        public UnitOfWorkLabelInsp(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        private WorkorderRepo workorderRepo;
        private WorkorderLabelRepo workorderLabelRepo;
        private WorkorderLabelInspectionRepo workorderLabelInspectionRepo;
        private ItemDataRepo itemDataRepo;

        public WorkorderRepo WorkorderRepo
        {
            get
            {
                workorderRepo = (workorderRepo != null) ? workorderRepo : new WorkorderRepo(db);
                return workorderRepo;
            }
        }
        public WorkorderLabelRepo WorkorderLabelRepo
        {
            get
            {
                workorderLabelRepo = (workorderLabelRepo != null) ? workorderLabelRepo : new WorkorderLabelRepo(db);
                return workorderLabelRepo;
            }
        }
        public WorkorderLabelInspectionRepo WorkorderLabelInspectionRepo
        {
            get
            {
                workorderLabelInspectionRepo = (workorderLabelInspectionRepo != null) ? workorderLabelInspectionRepo : new WorkorderLabelInspectionRepo(db);
                return workorderLabelInspectionRepo;
            }
        }
        public ItemDataRepo ItemDataRepo
        {
            get
            {
                itemDataRepo = (itemDataRepo != null) ? itemDataRepo : new ItemDataRepo(db);
                return itemDataRepo;
            }
        }
    }
}