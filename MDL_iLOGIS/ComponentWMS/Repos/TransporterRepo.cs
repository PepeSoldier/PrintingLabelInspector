using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class TransporterRepo : RepoGenericAbstract<Transporter>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public TransporterRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override Transporter GetById(int id)
        {
            return db.Transporters.FirstOrDefault(x => x.Id == id);
        }

        public Transporter GetById_AsNoTracking(int id)
        {
            return db.Transporters.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public List<Transporter> GetPickers()
        {
            return db.Transporters.Where(x => x.Type == Enums.EnumTransporterType.Picker && x.Deleted == false).ToList();
        }
        public List<Transporter> GetLineFeeders()
        {
            return db.Transporters.Where(x => x.Type == Enums.EnumTransporterType.LineFeeder).ToList();
        }

        //public IQueryable<Transporter> GetList(Transporter filter)
        //{

        //    return query;
        //}

    }
}
