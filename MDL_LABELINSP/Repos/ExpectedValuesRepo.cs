using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Repos
{
    public class ItemDataRepo : RepoGenericAbstract<ItemData>
    {
        protected new IDbContextLabelInsp db;

        public ItemDataRepo(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        public override ItemData GetById(int id)
        {
            return db.ItemData.FirstOrDefault(d => d.Id == id);
        }

        public ItemData GetByItemCodeAndVersion(string ItemCode, string ItemVersion)
        {
            var itemData = db.ItemData.Where(d => 
                (ItemCode == null || ItemCode == d.ItemCode) &&
                (ItemVersion == null || ItemVersion == d.ItemVersion)
            ).OrderByDescending(x => x.ItemVersion).FirstOrDefault();

            //itemData.ExpectedProductCode = itemData.ExpectedProductCode.Replace(".", "");
            return itemData;
        }

        public override IQueryable<ItemData> GetList()
        {
            return db.ItemData.OrderBy(x => x.Id);
        }

    }
}