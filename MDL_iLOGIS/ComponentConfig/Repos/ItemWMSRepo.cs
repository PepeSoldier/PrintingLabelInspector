using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDLX_MASTERDATA.Enums;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class ItemWMSRepo : RepoGenericAbstract<ItemWMS>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public ItemWMSRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override ItemWMS GetById(int id)
        {
            return db.ItemWMS.Where(x => x.Item.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ItemWMS> GetList()
        {
            return db.ItemWMS.Where(x => x.Item.Deleted == false).OrderBy(x => x.Id);
        }
        public virtual IQueryable<ItemWMS> GetList(ItemWMS filter)
        {
            bool startDateNotSet = filter.Item.StartDate == new DateTime();

            return dbSet.Where(p =>
                    (p.Item.Deleted == false) &&
                    (filter.Item.Name == null || p.Item.Name.Contains(filter.Item.Name)) &&
                    (filter.Item.Code == null || p.Item.Code.StartsWith(filter.Item.Code)) &&
                    (filter.Item.DEF == null || p.Item.DEF == filter.Item.DEF) &&
                    (filter.Item.BC == null || p.Item.BC == filter.Item.BC) &&
                    (filter.Item.PREFIX == null || p.Item.PREFIX.StartsWith(filter.Item.PREFIX)) &&
                    (filter.PickerNo <= 0 || p.PickerNo == filter.PickerNo) &&
                    (filter.TrainNo <= 0 || p.TrainNo == filter.TrainNo) &&
                    (filter.H <= 0 || p.H == filter.H) &&
                    (filter.Item.Type == 0 || p.Item.Type == filter.Item.Type) &&
                    (filter.Item.UnitOfMeasure == 0 || p.Item.UnitOfMeasure == filter.Item.UnitOfMeasure) &&
                    (startDateNotSet || p.Item.StartDate == filter.Item.StartDate) &&
                    (filter.Item.ItemGroupId == null || filter.Item.ItemGroupId == -1 || p.Item.ItemGroupId == filter.Item.ItemGroupId))
                .OrderByDescending(p => p.Item.StartDate);
        }
        public ItemWMS GetByCode(string itemCode)
        {
            return db.ItemWMS
                .FirstOrDefault(x => x.Item.Code == itemCode && x.Item.Deleted == false);
        }
        public ItemWMS GetByCodeWithUnitOfMeasures(string itemCode)
        {
            return db.ItemWMS
                .Include(x => x.Item.UnitOfMeasures)
                .FirstOrDefault(x => x.Item.Code == itemCode && x.Item.Deleted == false);
        }

        public ItemWMS Get(int? itemWMSId, int? itemId, string itemCode)
        {
            if ((itemWMSId == null || itemWMSId <= 0) && (itemId == null || itemId <= 0) && (itemCode == null || itemCode.Length <= 0))
            {
                return null;
            }
            else
            {
                return db.ItemWMS
                    .Where(x =>
                        (x.Item.Deleted == false) &&
                        ((itemWMSId == null || itemWMSId <= 0) || x.Id == itemWMSId) &&
                        ((itemId == null || itemId <= 0) || x.ItemId == itemId) &&
                        ((itemCode == null || itemCode.Length <= 0) || x.Item.Code == itemCode)
                    ).Take(1).FirstOrDefault();
            }
        }
    }
}
