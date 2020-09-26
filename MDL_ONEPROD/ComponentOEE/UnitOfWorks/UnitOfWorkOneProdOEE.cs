using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo.OEERepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkOneProdOEE : UnitOfWorkOneprod
    {
        public UnitOfWorkOneProdOEE(IDbContextOneProdOEE dbContext) : base(dbContext)
        {
            this.db = dbContext;
        }

        IDbContextOneProdOEE db;
        public IDbContextOneProdOEE Db
        {
            get
            {
                return db;
            }
        }

        private OEEReportRepo oeeReportRepo;
        private OEEReportEmployeeRepo oeeReportEmployeeRepo;
        private OEEReportProductionDataRepo oeeReportProductionDataRepo;
        private OEEReportProductionDataDetailsRepo oeeReportProductionDataPressLinesRepo;
        private ReasonRepo reasonRepo;
        private ReasonTypeRepo reasonTypeRepo;
        private MachineReasonRepo machineReasonRepo;
        private MachineTargetRepo machineTargetRepo;
        

        public OEEReportRepo OEEReportRepo
        {
            get
            {
                oeeReportRepo = (oeeReportRepo != null) ? oeeReportRepo : new OEEReportRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return oeeReportRepo;
            }
        }
        public OEEReportEmployeeRepo OEEReportEmployeeRepo
        {
            get
            {
                oeeReportEmployeeRepo = (oeeReportEmployeeRepo != null) ? oeeReportEmployeeRepo : new OEEReportEmployeeRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return oeeReportEmployeeRepo;
            }
        }
        public OEEReportProductionDataRepo OEEReportProductionDataRepo
        {
            get
            {
                oeeReportProductionDataRepo = (oeeReportProductionDataRepo != null) ? oeeReportProductionDataRepo : new OEEReportProductionDataRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return oeeReportProductionDataRepo;
            }
        }
        public OEEReportProductionDataDetailsRepo OEEReportProductionDataDetailsRepo
        {
            get
            {
                oeeReportProductionDataPressLinesRepo = (oeeReportProductionDataPressLinesRepo != null) ? oeeReportProductionDataPressLinesRepo : new OEEReportProductionDataDetailsRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return oeeReportProductionDataPressLinesRepo;
            }
        }
        public ReasonRepo ReasonRepo
        {
            get
            {
                reasonRepo = (reasonRepo != null) ? reasonRepo : new ReasonRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return reasonRepo;
            }
        }
        public ReasonTypeRepo ReasonTypeRepo
        {
            get
            {
                reasonTypeRepo = (reasonTypeRepo != null) ? reasonTypeRepo : new ReasonTypeRepo(db);
                return reasonTypeRepo;
            }
        }
        public MachineReasonRepo MachineReasonRepo
        {
            get
            {
                machineReasonRepo = (machineReasonRepo != null) ? machineReasonRepo : new MachineReasonRepo(db, AlertManager.Instance, this);
                return machineReasonRepo;
            }
        }
        public MachineTargetRepo MachineTargetRepo
        {
            get
            {
                machineTargetRepo = (machineTargetRepo != null) ? machineTargetRepo : new MachineTargetRepo(db);
                return machineTargetRepo;
            }
        }
    }
}