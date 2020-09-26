using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class DeliveryRepo : RepoGenericAbstract<Delivery>
    {
        protected new IDbContextiLOGIS db;

        public DeliveryRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }

        public Delivery Get(string documentNumber, string supplierCode, string externalId)
        {
            return db.Deliveries.FirstOrDefault(x => 
                (documentNumber == null || x.DocumentNumber == documentNumber) && 
                (supplierCode == null || x.Supplier.Code == supplierCode) &&
                (externalId == null || x.ExternalId == externalId)
            );
        }
        
        public IQueryable<Delivery> GetFilterList(IDeliveryFilter filter)
        {
            DateTime dateDefault = new DateTime();
            ItemWMS itemWMS = filter.ItemCode == null ? null : db.ItemWMS.Where(x => x.Item.Code == filter.ItemCode).FirstOrDefault();
            List<int> delivIds = new List<int>();
            if (itemWMS != null)
            {
                delivIds = db.DeliveryItems.Where(x => x.ItemWMSId == itemWMS.Id).Select(x => x.DeliveryId).Distinct().ToList();
            }
            return db.Deliveries
                .Include(x=>x.Supplier)
                .Include(y=>y.DeliveryItems)
                .Where(x =>
                    (filter.SupplierCode == null || x.Supplier.Code == filter.SupplierCode) &&
                    (filter.SupplierName == null || x.Supplier.Name.Contains(filter.SupplierName)) &&
                    (filter.ItemCode == null || delivIds.Contains(x.Id)) &&
                    (filter.DocumentNumber == null || x.DocumentNumber.Contains(filter.DocumentNumber)) &&
                    (filter.DocumentDate == dateDefault || x.DocumentDate == filter.DocumentDate) &&
                    (filter.StampTime == dateDefault || x.StampTime == filter.StampTime) &&
                    (filter.UserName == null || x.User.Name == filter.UserName) &&
                    (filter.Guid == "" || x.Guid == filter.Guid) &&
                    (filter.Id == 0 || x.Id == filter.Id) &&
                    (x.Deleted == false)
                )
                .OrderBy(x => x.DocumentDate)
                .ThenBy(x => x.DocumentNumber);
        }

        public IQueryable<Delivery> GetFilterListForInspection(IDeliveryFilter filter)
        {
            DateTime dateDefault = new DateTime();
            DateTime dateFrom = DateTime.Now.AddDays(-5);

            ItemWMS itemWMS = filter.ItemCode == null ? null : db.ItemWMS.Where(x => x.Item.Code == filter.ItemCode).FirstOrDefault();
            List<int> delivIds = new List<int>();
            if (itemWMS != null)
            {
                delivIds = db.DeliveryItems.Where(x => x.ItemWMSId == itemWMS.Id).Select(x => x.DeliveryId).Distinct().ToList();
            }
            return db.Deliveries
                .Include(x => x.Supplier)
                .Include(y => y.DeliveryItems)
                .Where(x =>
                    (filter.SupplierCode == null || x.Supplier.Code == filter.SupplierCode) &&
                    (filter.SupplierName == null || x.Supplier.Name.Contains(filter.SupplierName)) &&
                    (filter.ItemCode == null || delivIds.Contains(x.Id)) &&
                    (filter.DocumentNumber == null || x.DocumentNumber.Contains(filter.DocumentNumber)) &&
                    (filter.DocumentDate == dateDefault || x.DocumentDate == filter.DocumentDate) &&
                    (filter.StampTime == dateDefault || x.StampTime == filter.StampTime) &&
                    (filter.UserName == null || x.User.Name == filter.UserName) &&
                    (filter.Guid == "" || x.Guid == filter.Guid) &&
                    (filter.Id == 0 || x.Id == filter.Id) &&
                    (x.Deleted == false)
                )
                .Where(x => x.EnumDeliveryStatus != Enums.EnumDeliveryStatus.Finished || (x.EnumDeliveryStatus == Enums.EnumDeliveryStatus.Finished && x.DocumentDate > dateFrom))
                .OrderBy(x => x.DocumentDate)
                .ThenBy(x => x.DocumentNumber);
        }


        public List<Delivery> GetGroupByGuid(string deliveryItemListGroupGuid)
        {
            return db.Deliveries.Where(x => x.Guid == deliveryItemListGroupGuid).ToList();
        }

    }
}
