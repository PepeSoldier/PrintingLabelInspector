using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class MovementRepo : RepoGenericAbstract<Movement>
    {
        protected new IDbContextiLOGIS db;

        public MovementRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<Movement> GetByFilter(IMovementFilter item)
        {
            EnumMovementType enumMovementType = (EnumMovementType)item.MovementType;
            DateTime movementDate = Convert.ToDateTime(item.MovementDate).Date;

            return db.Movements.Where(x => 
                (item.Id == 0 || x.Id == item.Id) &&
                (item.ItemCode == null || x.ItemWMS.Item.Code == item.ItemCode) &&
                (item.ItemName == null || x.ItemWMS.Item.Name.Contains(item.ItemName)) &&
                (item.SourceAccountingWarehouseName == null || x.SourceWarehouse.AccountingWarehouse.Code == item.SourceAccountingWarehouseName) &&
                (item.SourceWarehouseName == null || x.SourceWarehouse.Name == item.SourceWarehouseName) &&
                (item.SourceLocationName == null || x.SourceLocation.Name == item.SourceLocationName) &&
                (item.SourceStockUnitSerialNumber == null || x.SourceStockUnitSerialNumber == item.SourceStockUnitSerialNumber) &&
                (item.DestinationAccountingWarehouseName == null || x.DestinationWarehouse.AccountingWarehouse.Code == item.DestinationAccountingWarehouseName) &&
                (item.DestinationWarehouseName == null || x.DestinationWarehouse.Name == item.DestinationWarehouseName) &&
                (item.DestinationLocationName == null || x.DestinationLocation.Name == item.DestinationLocationName) &&
                (item.DestinationStockUnitSerialNumber == null || x.DestinationStockUnitSerialNumber == item.DestinationStockUnitSerialNumber) &&
                (item.UserName == null || x.User.UserName == item.UserName ) &&
                (item.QtyMoved == 0 || x.QtyMoved == item.QtyMoved) &&
                (item.MovementType == 0 || x.Type == enumMovementType) &&
                (item.MovementDate == null || DbFunctions.TruncateTime(x.Date) == movementDate))
           .OrderByDescending(x => x.Id);
        }
    }
}