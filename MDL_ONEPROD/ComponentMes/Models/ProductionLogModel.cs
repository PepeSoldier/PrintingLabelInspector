using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponetMes.Models
{
    public class ProductionLogModel
    {
        private UnitOfWorkOneProdMes uow;


        public ProductionLogModel(UnitOfWorkOneProdMes uowork)
        {
            this.uow = uowork;
        }


        public List<ProductionLogRawMaterial> ShowUsedMaterials(int productionLogId,string partCode, string serialNumber)
        {
            return uow.ProductionLogRawMaterialRepo.GetBy_PartCodeSerialNumber(productionLogId,partCode, serialNumber);
        }
        public List<ProductionLog> ShowProductionLogByWorkOrder(string workOrder)
        {
            return uow.ProductionLogRepo.GetListByWorkorderNumber(workOrder).ToList();
        }
        public List<ProductionLog> ShowProductionLogBySerialNumber(string serialNumber)
        {
            return uow.ProductionLogRepo.GetListByInternalWorkOrderNumber(serialNumber).ToList();
        }

        public void CreateProductionLogInitial(OEEReportProductionData oEEReport, Workplace workplace, string userName)
        {
            ProductionLog pl = new ProductionLog();
            pl.Workplace = workplace;
            pl.OEEReportProductionData = oEEReport;
            pl.UserName = userName;
            pl.TimeStamp = DateTime.Now;
            uow.ProductionLogRepo.AddOrUpdate(pl);
        }
        public void UpdateProductionLog(int productionLogId, string serialNumber, decimal declaredQty)
        {
            ProductionLog pL = uow.ProductionLogRepo.GetById(productionLogId);
            pL.TimeStamp = DateTime.Now;
            pL.InternalWorkOrderNumber = serialNumber;
            pL.DeclaredQty = declaredQty;
            
            uow.ProductionLogRepo.AddOrUpdate(pL);
        }
        public void AssignRawMaterialToProductionLog(int productionLogId, string partCode, string bachSerialNo)
        {
            ProductionLogRawMaterial plrm = new ProductionLogRawMaterial();
            plrm.ProductionLogId = productionLogId;
            plrm.PartCode = partCode;
            plrm.BatchSerialNo = bachSerialNo;

            uow.ProductionLogRawMaterialRepo.AddOrUpdate(plrm);
        }
    }
}