using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_OTHER.ComponentPickByLight.Repos;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentPickByLight.UnitOfWorks
{
    public class UnitOfWorkPickByLight : UnitOfWorkCore
    {
        IDbContextPickByLight db;

        public UnitOfWorkPickByLight(IDbContextPickByLight db) : base(db)
        {
            this.db = db;
        }

        private PickByLightInstanceElementRepo pickByLightInstanceElementRepo;
        private PickByLightInstanceRepo pickByLightInstanceRepo;

        public PickByLightInstanceElementRepo PickByLightInstanceElementRepo
        {
            get
            {
                pickByLightInstanceElementRepo = (pickByLightInstanceElementRepo != null) ? pickByLightInstanceElementRepo : new PickByLightInstanceElementRepo(db);
                return pickByLightInstanceElementRepo;
            }
        }
        public PickByLightInstanceRepo PickByLightInstanceRepo
        {
            get
            {
                pickByLightInstanceRepo = (pickByLightInstanceRepo != null) ? pickByLightInstanceRepo : new PickByLightInstanceRepo(db);
                return pickByLightInstanceRepo;
            }
        }

    }
}
