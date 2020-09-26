using MDL_AP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MDL_BASE.Models.IDENTITY;
using MDL_AP.Models.DEF;
using XLIB_COMMON.Repo;
using XLIB_COMMON.Repo.IDENTITY;
using XLIB_COMMON.Model;
using MDL_AP.Repo.DEF;
using MDL_AP.Repo.ActionPlan;
using MDL_AP.Interfaces;
using MDLX_MASTERDATA.Repos;
using MDL_BASE.ComponentBase.Repos;
using MDLX_CORE.ComponentCore.UnitOfWorks;

namespace MDL_AP.Repo
{
    public class UnitOfWorkActionPlan : UnitOfWorkCore
    {
        IDbContextAP db;
        RepoCommon repoCommon;
        RepoAction repoAction;
        RepoActionActivity repoActionActivity;
        RepoWorkstation repoWorkstation;
        RepoType repoType;
        RepoArea repoArea;
        ResourceRepo repoLine;
        RepoLabourBrigade repoShiftCode;
        RepoAttachment repoPhoto;
        RepoDepartment repoDepartment;
        RepoCategory repoCategory;
        //UserRepo userRepo;
        RepoActionObserver repoActionObserver;
        RepoMeeting repoActionMeeting;
        //RepoExtensionFiles repoExtensionFiles;

        public IDbContextAP DbContext { get { return db; } }
        public RepoCommon RepoCommon
        {
            get
            {
                if (repoCommon == null) { repoCommon = new RepoCommon(db, AlertManager.Instance); }
                return repoCommon;
            }
        }
        public RepoAction RepoAction
        {
            get
            {
                if (repoAction == null) { repoAction = new RepoAction(db, AlertManager.Instance, this); }
                return repoAction;
            }
        }
        public RepoActionActivity RepoActionActivity
        {
            get
            {
                if (repoActionActivity == null) { repoActionActivity = new RepoActionActivity(db, AlertManager.Instance, this); }
                return repoActionActivity;
            }
        }
        public RepoWorkstation RepoWorkstation
        {
            get
            {
                if (repoWorkstation == null) { repoWorkstation = new RepoWorkstation(db); }
                return repoWorkstation;
            }
        }
        public RepoArea RepoArea
        {
            get
            {
                if (repoArea == null) { repoArea = new RepoArea(db); }
                return repoArea;
            }
        }
        public RepoType RepoType
        {
            get
            {
                if (repoType == null) { repoType = new RepoType(db, AlertManager.Instance, this); }
                return repoType;
            }
        }
        public ResourceRepo ResourceRepo
        {
            get
            {
                if (repoLine == null) { repoLine = new ResourceRepo(db); }
                return repoLine;
            }
        }
        public RepoLabourBrigade RepoShiftCode
        {
            get
            {
                if (repoShiftCode == null) { repoShiftCode = new RepoLabourBrigade(db); }
                return repoShiftCode;
            }
        }
        public RepoAttachment RepoAttachment
        {
            get
            {
                if (repoPhoto == null) { repoPhoto = new RepoAttachment(db); }
                return repoPhoto;
            }
        }
        public RepoDepartment RepoDepartment
        {
            get
            {
                if (repoDepartment == null) { repoDepartment = new RepoDepartment(db); }
                return repoDepartment;
            }
        }
        public RepoCategory RepoCategory
        {
            get
            {
                if (repoCategory == null) { repoCategory = new RepoCategory(db, AlertManager.Instance, this); }
                return repoCategory;
            }
        }
        //public UserRepo UserRepo
        //{
        //    get
        //    {
        //        //TODO: user repo db dependency
        //        if (userRepo == null) { userRepo = new UserRepo(new UserStore<User>(db), db); }
        //        return userRepo;
        //    }
        //}

        public RepoActionObserver RepoActionObserver
        {
            get
            {
                if(repoActionObserver == null) { repoActionObserver = new RepoActionObserver(db, AlertManager.Instance, this); }
                return repoActionObserver;
            }
        }

        public RepoMeeting RepoMeeting
        {
            get
            {
                if (repoActionMeeting == null) { repoActionMeeting = new RepoMeeting(db, AlertManager.Instance, this); }
                return repoActionMeeting;
            }
        }

        //public RepoExtensionFiles RepoExtensionFiles
        //{
        //    get
        //    {
        //        if (repoExtensionFiles == null) { repoExtensionFiles = new RepoExtensionFiles(db); }
        //        return repoExtensionFiles;
        //    }
        //}


        public UnitOfWorkActionPlan(IDbContextAP dbContext) : base(dbContext)
        {
            db = dbContext;
        }
    }
}
