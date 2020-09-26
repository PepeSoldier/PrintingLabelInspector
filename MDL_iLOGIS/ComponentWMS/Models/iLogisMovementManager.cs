using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class iLogisMovementManager
    {
        IDbContextiLOGIS db;
        UnitOfWork_iLogis uow;
        Movement movement;
        List<Movement> movements;
        WarehouseLocation externalWarehouseLocation;
        WarehouseLocation ExternalWarehouseLocation
        {
            get
            {
                if (externalWarehouseLocation == null || externalWarehouseLocation.Id < 1)
                    externalWarehouseLocation = uow.WarehouseLocationRepo.GetExternalWarehouseLocation();
                return externalWarehouseLocation;
            }
        }
        string freeText1; 
        string freeText2;

        public iLogisMovementManager(IDbContextiLOGIS db)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            movement = new Movement();
            movements = new List<Movement>();
            externalWarehouseLocation = new WarehouseLocation();
        }

        public void SetFreeText(string freeText1, string freeText2)
        {
            this.freeText1 = freeText1;
            this.freeText2 = freeText2;
        }
        public Movement CreateMovementLog(StockUnit sourceStockUnit, StockUnit destinationStockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure)
        {
            movement = new Movement();
            AssignMovementValues(sourceStockUnit, destinationStockUnit, userId, qtyMoved, unitOfMeasure);
            movement.Type = GetMovementTypeByLocation(destinationStockUnit.WarehouseLocation);

            return InsertMovement();
        }
        public Movement CreateMovementLog(StockUnit stockUnit, WarehouseLocation destinationLocation, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure)
        {
            movement = new Movement();
            AssignMovementValues(stockUnit, destinationLocation, userId, qtyMoved, unitOfMeasure);
            movement.Type = GetMovementTypeByLocation(destinationLocation);

            return InsertMovement();
        }
        public Movement CreateMovementLogDelivery(StockUnit destinationStockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure, decimal deliveryId)
        {
            movement = new Movement();
            AssignMovementValues(ExternalWarehouseLocation, destinationStockUnit, userId, qtyMoved, unitOfMeasure);
            movement.Type = EnumMovementType.CODE_101;
            movement.SourceStockUnitSerialNumber = deliveryId.ToString();

            return InsertMovement();
        }
        public Movement CreateMovementLogByType(StockUnit stockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure, EnumMovementType type)
        {
            movement = new Movement();
            if (IsMovementTypeToExternal(type) || (IsMovementTypeOfInventory(type) && qtyMoved <= 0 ))
            {
                return CreateMovementLogToExternal(stockUnit, userId, qtyMoved, unitOfMeasure, type);
            }
            else if (IsMovementTypeFromExternal(type) || (IsMovementTypeOfInventory(type) && qtyMoved > 0))
            {
                return CreateMovementLogFromExternal(stockUnit, userId, qtyMoved, unitOfMeasure, type);
            }
            else
            {
                return null;
            }
        }
        public Movement CreateMovementLogByType(StockUnit stockUnit, WarehouseLocation destinationLocation, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure, EnumMovementType type)
        {
            movement = new Movement();
            if (IsMovementTypeToExternal(type) || (IsMovementTypeOfInventory(type) && qtyMoved <= 0))
            {
                return CreateMovementLogToExternal(stockUnit, userId, qtyMoved, unitOfMeasure, type);
            }
            else if (IsMovementTypeFromExternal(type) || (IsMovementTypeOfInventory(type) && qtyMoved > 0))
            {
                return CreateMovementLogFromExternal(stockUnit, userId, qtyMoved, unitOfMeasure, type);
            }
            else if(type == EnumMovementType.CODE_311)
            {
                return CreateMovementLogBetween(stockUnit, destinationLocation, userId, qtyMoved, unitOfMeasure, type);
            }
            else
            {
                return null;
            }
        }

        private Movement CreateMovementLogFromExternal(StockUnit destinationStockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure, EnumMovementType type)
        {
            AssignMovementValues(ExternalWarehouseLocation, destinationStockUnit, userId, qtyMoved, unitOfMeasure);
            movement.Type = type;

            return InsertMovement();
        }
        private Movement CreateMovementLogToExternal(StockUnit sourceStockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure, EnumMovementType type)
        {
            AssignMovementValues(sourceStockUnit, ExternalWarehouseLocation, userId, qtyMoved, unitOfMeasure);
            movement.Type = type;

            return InsertMovement();
        }
        private Movement CreateMovementLogBetween(StockUnit sourceStockUnit, WarehouseLocation destinationLocation, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure, EnumMovementType type)
        {
            AssignMovementValues(sourceStockUnit, destinationLocation, userId, qtyMoved, unitOfMeasure);
            movement.Type = type;
            return InsertMovement();
        }

        private Movement InsertMovement()
        {
            movement.FreeText1 = freeText1;
            movement.FreeText2 = freeText2;

            Movement movementToReturn = movement;
            //uow.MovementRepo.Add(movement);
            movements.Add(movementToReturn);
            //movement = new Movement();
            return movementToReturn;
        }

        public bool Save()
        {
            return uow.MovementRepo.AddOrUpdateRange(movements);
        }

        private EnumMovementType GetMovementTypeByLocation(WarehouseLocation location)
        {
            return location.Warehouse.isProduction == true ? EnumMovementType.CODE_261 : EnumMovementType.CODE_311;
        }
        private static bool IsMovementTypeToExternal(EnumMovementType type)
        {
            List<EnumMovementType> movementsToExternal = new List<EnumMovementType>()
            {
                EnumMovementType.CODE_102,
                EnumMovementType.CODE_122,
                EnumMovementType.CODE_124,
                EnumMovementType.CODE_161,
                EnumMovementType.CODE_201,
                EnumMovementType.CODE_221,
                EnumMovementType.CODE_251,
                EnumMovementType.CODE_261,
                EnumMovementType.CODE_281,
                EnumMovementType.CODE_291,
                EnumMovementType.CODE_301,
                EnumMovementType.CODE_303,
                EnumMovementType.CODE_305,
                EnumMovementType.CODE_351,
                //EnumMovementType.CODE_413,
                EnumMovementType.CODE_551,
                EnumMovementType.CODE_553,
                EnumMovementType.CODE_555,
                EnumMovementType.CODE_557,
                EnumMovementType.CODE_601,
                EnumMovementType.CODE_603,
                EnumMovementType.CODE_621,
                EnumMovementType.CODE_623,
                //EnumMovementType.CODE_631,
                EnumMovementType.CODE_633,
                EnumMovementType.CODE_641,
                EnumMovementType.CODE_643,
                EnumMovementType.CODE_645,
                EnumMovementType.CODE_647,
                EnumMovementType.CODE_661,
                EnumMovementType.CODE_673,
                EnumMovementType.CODE_675,
            };

            return movementsToExternal.Contains(type);
        }
        private static bool IsMovementTypeFromExternal(EnumMovementType type)
        {
            List<EnumMovementType> movementsToExternal = new List<EnumMovementType>()
            {
                EnumMovementType.CODE_101,
                EnumMovementType.CODE_103,
                EnumMovementType.CODE_105,
                EnumMovementType.CODE_121,
                EnumMovementType.CODE_451,
                EnumMovementType.CODE_455,
                EnumMovementType.CODE_501,
                EnumMovementType.CODE_503,
                EnumMovementType.CODE_505,
                EnumMovementType.CODE_521,
                EnumMovementType.CODE_523,
                EnumMovementType.CODE_525,
                EnumMovementType.CODE_531,
                EnumMovementType.CODE_545,
                EnumMovementType.CODE_561,
                EnumMovementType.CODE_563,
                EnumMovementType.CODE_565,
                EnumMovementType.CODE_581,
                EnumMovementType.CODE_605,
                EnumMovementType.CODE_651,
                EnumMovementType.CODE_653,
                EnumMovementType.CODE_655,
                EnumMovementType.CODE_657,
            };

            return movementsToExternal.Contains(type);
        }
        private static bool IsMovementTypeOfInventory(EnumMovementType type)
        {
            List<EnumMovementType> movementsToExternal = new List<EnumMovementType>()
            {
                EnumMovementType.CODE_701,
                EnumMovementType.CODE_703,
                EnumMovementType.CODE_707,
                EnumMovementType.CODE_711,
                EnumMovementType.CODE_713,
                EnumMovementType.CODE_715,
                EnumMovementType.CODE_717,
                EnumMovementType.CODE_561,
                EnumMovementType.CODE_562
            };

            return movementsToExternal.Contains(type);
        }
        
        private void AssignMovementValues(StockUnit sourceStockUnit, StockUnit destinationStockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure)
        {
            movement.Date = DateTime.Now;
            movement.ItemWMSId = sourceStockUnit.ItemWMSId;
            movement.SourceWarehouseId = sourceStockUnit.WarehouseLocation.WarehouseId;
            movement.SourceLocationId = sourceStockUnit.WarehouseLocationId;
            movement.SourceStockUnitSerialNumber = sourceStockUnit.SerialNumber;
            movement.DestinationWarehouseId = destinationStockUnit.WarehouseLocation.WarehouseId;
            movement.DestinationLocationId = destinationStockUnit.WarehouseLocation.Id;
            movement.DestinationStockUnitSerialNumber = destinationStockUnit.SerialNumber;
            movement.QtyMoved = qtyMoved;
            movement.UnitOfMeasure = unitOfMeasure;
            movement.UserId = userId;
        }
        private void AssignMovementValues(StockUnit sourceStockUnit, WarehouseLocation destinationLocation, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure)
        {
            movement.Date = DateTime.Now;
            movement.ItemWMSId = sourceStockUnit.ItemWMSId;
            movement.SourceWarehouseId = sourceStockUnit.WarehouseLocation.WarehouseId;
            movement.SourceLocationId = sourceStockUnit.WarehouseLocationId;
            movement.SourceStockUnitSerialNumber = sourceStockUnit.SerialNumber;
            movement.DestinationWarehouseId = destinationLocation.WarehouseId;
            movement.DestinationLocationId = destinationLocation.Id;
            movement.DestinationStockUnitSerialNumber = "";
            movement.QtyMoved = qtyMoved;
            movement.UnitOfMeasure = unitOfMeasure;
            movement.UserId = userId;
        }
        private void AssignMovementValues(WarehouseLocation sourceWarehouseLocation, StockUnit destinationStockUnit, string userId, decimal qtyMoved, UnitOfMeasure unitOfMeasure)
        {
            movement.Date = DateTime.Now;
            movement.ItemWMSId = destinationStockUnit.ItemWMSId;
            movement.SourceWarehouseId = sourceWarehouseLocation.WarehouseId;
            movement.SourceLocationId = sourceWarehouseLocation.Id;
            movement.SourceStockUnitSerialNumber = "";
            movement.DestinationWarehouseId = destinationStockUnit.WarehouseLocation.WarehouseId;
            movement.DestinationLocationId = destinationStockUnit.WarehouseLocationId;
            movement.DestinationStockUnitSerialNumber = destinationStockUnit.SerialNumber;
            movement.QtyMoved = qtyMoved;
            movement.UnitOfMeasure = unitOfMeasure;
            movement.UserId = userId;
        }
    }
}
