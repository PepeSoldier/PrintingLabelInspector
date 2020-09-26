using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class PackageRepo : RepoGenericAbstract<Package>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public PackageRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override Package GetById(int id)
        {
            return db.Packages.Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override Package GetByIdAsNoTracking(int id)
        {
            return db.Packages.AsNoTracking().Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<Package> GetList()
        {
            return db.Packages.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
        public IQueryable<Package> GetList(Package filter)
        {
            return db.Packages
                .Where(x => 
                    (x.Deleted == false) &&
                    (filter.Name == null || x.Name.Contains(filter.Name)) &&
                    (filter.Code == null || x.Code.Contains(filter.Code)) &&
                    (filter.UnitOfMeasure == 0 || x.UnitOfMeasure == filter.UnitOfMeasure) &&
                    (filter.Width == 0 || x.Width == filter.Width) &&
                    (filter.Height == 0 || x.Height == filter.Height) &&
                    (filter.Depth  == 0 || x.Depth == filter.Depth) &&
                    (filter.Type  == 0 || x.Type == filter.Type) &&
                    (filter.Weight  == 0 || x.Weight == filter.Weight) &&
                    (filter.Returnable == false || x.Returnable == filter.Returnable)
                )
                .OrderByDescending(x => x.Id);
        }

        public void UpdateMany(Package part, IEnumerable<int> ids)
        {
            var manyPackages = db.Packages.Where(x => ids.Contains(x.Id)).ToList();
            manyPackages.ForEach(x =>
            {
                x.UnitOfMeasure = part.UnitOfMeasure;
                x.Type = part.Type;
                x.Weight = part.Weight;
                x.Width = part.Width;
                x.Depth = part.Depth;
                x.Height = part.Height;
                x.Returnable = part.Returnable;
            });
            db.SaveChanges();
        }
    }
}
