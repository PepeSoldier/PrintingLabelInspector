using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_OTHER.ComponentPickByLight.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Repo;

namespace MDL_OTHER.ComponentPickByLight.Repos
{
    public class PickByLightInstanceRepo : RepoGenericAbstract<PickByLightInstance>
    {
        protected new IDbContextPickByLight db;

        public PickByLightInstanceRepo(IDbContextPickByLight db) : base(db)
        {
            this.db = db;
        }

        public override PickByLightInstance GetById(int id)
        {
            return db.PickByLightInstances.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<PickByLightInstance> GetListFiltered(PickByLightInstance filter)
        {
            return db.PickByLightInstances.Where(x =>
                (filter.TCPPort == null || filter.TCPPort == x.TCPPort) &&
                (filter.Name == null || filter.Name == x.Name) &&
                (filter.PLCDriverIPAdress == null || filter.PLCDriverIPAdress == x.PLCDriverIPAdress)
             ).OrderBy(x => x.Name).ThenBy(x => x.TCPPort);
        }
    }
}
