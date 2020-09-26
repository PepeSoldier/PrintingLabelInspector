using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentHSE.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentJobItemConfig.UnitOfWorks
{
    public class UnitOfWorkVisualControl : UnitOfWorkCore
    {
        IDbContextVisualControl db;

        public UnitOfWorkVisualControl(IDbContextVisualControl db) : base (db)
        {
            this.db = db;
        }

        private JobItemConfigRepo jobItemConfigRepo;
        public JobItemConfigRepo JobItemConfigRepo
        {
            get
            {
                jobItemConfigRepo = (jobItemConfigRepo != null) ? jobItemConfigRepo : new JobItemConfigRepo(db);
                return jobItemConfigRepo;
            }
        }
    }
}
