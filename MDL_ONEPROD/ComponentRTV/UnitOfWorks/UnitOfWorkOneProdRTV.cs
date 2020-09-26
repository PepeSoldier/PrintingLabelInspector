using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.ComponentRTV.Entities;
using MDL_ONEPROD.ComponentRTV.Repos;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo.OEERepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkOneProdRTV : UnitOfWorkOneprod
    {
        public UnitOfWorkOneProdRTV(IDbContextOneProdRTV dbContext) : base(dbContext)
        {
            this.db = dbContext;
        }

        IDbContextOneProdRTV db;
        public IDbContextOneProdRTV Db
        {
            get
            {
                return db;
            }
        }

        private RTVOEEProductionDataRepo rTVOEEProductionDataRepo;
        private RTVOEEProductionDataDetailsRepo rTVOEEProductionDataDetailsRepo;
        private RTVOEEProductionDataParameterRepo rTVOEEProductionDataParameterRepo;
        private RTVOEEParameterRepo rTVOEEParameterRepo;
        private ReasonTypeRepo reasonTypeRepo;

        public RTVOEEProductionDataRepo RTVOEEProductionDataRepo
        {
            get
            {
                rTVOEEProductionDataRepo = (rTVOEEProductionDataRepo != null) ? rTVOEEProductionDataRepo : new RTVOEEProductionDataRepo(db);
                return rTVOEEProductionDataRepo;
            }
        }
        public RTVOEEProductionDataDetailsRepo RTVOEEProductionDataDetailsRepo
        {
            get
            {
                rTVOEEProductionDataDetailsRepo = (rTVOEEProductionDataDetailsRepo != null) ? rTVOEEProductionDataDetailsRepo : new RTVOEEProductionDataDetailsRepo(db);
                return rTVOEEProductionDataDetailsRepo;
            }
        }
        public RTVOEEProductionDataParameterRepo RTVOEEProductionDataParameterRepo
        {
            get
            {
                rTVOEEProductionDataParameterRepo = (rTVOEEProductionDataParameterRepo != null) ? rTVOEEProductionDataParameterRepo : new RTVOEEProductionDataParameterRepo(db);
                return rTVOEEProductionDataParameterRepo;
            }
        }
        public RTVOEEParameterRepo RTVOEEParameterRepo
        {
            get
            {
                rTVOEEParameterRepo = (rTVOEEParameterRepo != null) ? rTVOEEParameterRepo : new RTVOEEParameterRepo(db);
                return rTVOEEParameterRepo;
            }
        }
        public ReasonTypeRepo ReasonTypeRepo
        {
            get
            {
                reasonTypeRepo = (reasonTypeRepo != null) ? reasonTypeRepo : new ReasonTypeRepo(db);
                return reasonTypeRepo;
            }
        }
    }
}