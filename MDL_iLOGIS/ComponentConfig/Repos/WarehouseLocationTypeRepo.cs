using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System.Linq;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_BASE.Interfaces;
using System.Collections.Generic;
using System;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class WarehouseLocationTypeRepo : RepoGenericAbstract<WarehouseLocationType>
    {
        protected new IDbContextiLOGIS db;
        public WarehouseLocationTypeRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }

        public override WarehouseLocationType GetById(int id)
        {
            return db.WarehouseLocationTypes.FirstOrDefault(x => x.Id == id);
        }
        public WarehouseLocationType GetByName(string locationName)
        {
            var whlocType = db.WarehouseLocationTypes.Where(x => x.Name == locationName).FirstOrDefault();
            return whlocType;
        }
        public WarehouseLocationType GetOrCreate(string name)
        {
            WarehouseLocationType whLocType = db.WarehouseLocationTypes.FirstOrDefault(x => x.Name == name);

            if (whLocType == null)
            {
                whLocType = new WarehouseLocationType();
                whLocType.Name = name;
                whLocType.TypeEnum = WarehouseLocationTypeEnum.Trolley;
                whLocType.Description = "platformy do pickingu";
                AddOrUpdate(whLocType);
            }
            
            return whLocType;
        }
        public WarehouseLocationType GetByNameAndType(string locationName, WarehouseLocationTypeEnum type)
        {
            var whLocType = db.WarehouseLocationTypes
                .Where(x => x.Name == locationName &&
                    (type == WarehouseLocationTypeEnum.Shelf || x.TypeEnum == type))
                .FirstOrDefault();
            return whLocType;
        }

        public override IQueryable<WarehouseLocationType> GetList()
        {
            return db.WarehouseLocationTypes.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
        public IQueryable<WarehouseLocationType> GetBySize(int width, int height, int depth)
        {
            var query = db.WarehouseLocationTypes
                .Where(x =>
                    (width <= 0 || x.Width == width) &&
                    (height <= 0 || x.Height == height) &&
                    (depth <= 0 || x.Depth == depth)
                    );

            return query;
        }
    }
}