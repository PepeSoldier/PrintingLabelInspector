using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class DeliveryModel
    {
        IDbContextiLOGIS db;
        UnitOfWork_iLogis uow;
        List<Delivery> deliveries;
        List<DeliveryItem> deliveryItems;
        StockUnitModel stockUnitModel;
        iLogisMovementManager movementManager;

        public DeliveryModel(IDbContextiLOGIS db)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            stockUnitModel = new StockUnitModel(db);
            movementManager = new iLogisMovementManager(db);

            deliveries = new List<Delivery>();
            deliveryItems = new List<DeliveryItem>();
        }

        public void Save()
        {
            using (var transaction = uow.BeginTransaction())
            {
                try
                {
                    uow.DeliveryRepo.AddOrUpdateRange(deliveries);
                    uow.DeliveryItemRepo.AddOrUpdateRange(deliveryItems);
                    stockUnitModel.Save();
                    movementManager.Save();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Debug.WriteLine("Zapis delivery nie powiódł się");
                    throw ex;
                }
            }
        }

        public void DeliveryAdd(Delivery delivery)
        {
            deliveries.Add(delivery);

            foreach (DeliveryItem deliveryItem in delivery.DeliveryItems)
            {
                GenerateStockUnitAndMovement(deliveryItem, delivery);
            }
        }
        public void DeliveryItemAdd(DeliveryItem deliveryItem)
        {
            deliveryItems.Add(deliveryItem);
            GenerateStockUnitAndMovement(deliveryItem);
        }

        private void GenerateStockUnitAndMovement(DeliveryItem deliveryItem, Delivery delivery = null)
        {
            StockUnit su = stockUnitModel.CreateStockUnit_OnIncoming(deliveryItem.ItemWMS, null, null, deliveryItem.TotalQty, deliveryItem.TotalQty, "0", deliveryItem.StockStatus, true);
            //su.Status = deliveryItem.StockStatus;
            Movement movement = movementManager.CreateMovementLogByType(su, null, deliveryItem.TotalQty, deliveryItem.UnitOfMeasure , deliveryItem.MovementType);

            if (deliveryItem.Delivery != null) {
                movement.ExternalId = deliveryItem.Delivery.ExternalId;
                movement.ExternalUserName = deliveryItem.Delivery.ExternalUserName;
            }
            else if (delivery != null)
            {
                movement.ExternalId = delivery.ExternalId;
                movement.ExternalUserName = delivery.ExternalUserName;
            }
        }
    }
}
