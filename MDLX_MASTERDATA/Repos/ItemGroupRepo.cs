using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    public class ItemGroupRepo : RepoGenericAbstract<Item>
    {
        protected new IDbContextMasterData db;

        public ItemGroupRepo(IDbContextMasterData db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        //public override Item GetById(int id)
        //{
        //    return db.Items.FirstOrDefault(x => x.Id == id);
        //}
        //public override IQueryable<Item> GetList()
        //{
        //    return db.Items.Where(x=> x.Deleted == false && x.Type == ItemTypeEnum.ItemGroup).OrderBy(x => x.Name);
        //}
        //public List<Item> GetList(Item Part)
        //{
        //    return db.Items.Where(x =>
        //            (x.Deleted == false) && 
        //            (x.Type == ItemTypeEnum.ItemGroup) &&
        //            (Part.ResourceGroupId == null || Part.ResourceGroupId == -1 || x.ResourceGroupId == Part.ResourceGroupId) &&
        //            //(Part.ProcessId == null || Part.ProcessId == -1 || x.ProcessId == Part.ProcessId) &&
        //            (Part.Name == null || x.Name.StartsWith(Part.Name)))
        //        .OrderBy(x => x.ResourceGroup.Name)//.OrderBy(x => x.ResourceGroup.StageNo)
        //        .ThenBy(x=>x.Name)
        //        .ToList();
        //}
        //public List<Item> GetListAsNoTracking()
        //{
        //    return db.Items
        //        .AsNoTracking()
        //        .Where(x => x.Deleted == false && x.Type == ItemTypeEnum.ItemGroup)
        //        .OrderBy(x => x.Name)
        //        .ToList();
        //}
    }
}