using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDLX_CORE.ComponentCore.Repos
{
    public class NotificationDeviceRepo : RepoGenericAbstract<NotificationDevice>
    {
        protected new IDbContextCore db;

        public NotificationDeviceRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        //public override NotificationDevice GetById(int id)
        //{
        //    return db.NotificationDevices.FirstOrDefault(d => d.Id == id && d.Deleted == false);
        //}

        //public override IQueryable<NotificationDevice> GetList()
        //{
        //    return db.NotificationDevices.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        //}

        //public List<NotificationDevice> GetByUserId(string userId)
        //{
        //    return db.NotificationDevices.Where(x => x.UserId == userId && x.Deleted == false).OrderByDescending(x => x.Id).Take(3).ToList();
        //}

        //public NotificationDevice GetByUserIdAndPushEndpoint(string userId, string pushEndpoint)
        //{
        //    return db.NotificationDevices.Where(x => x.Deleted == false && x.UserId == userId && x.PushEndpoint == pushEndpoint).FirstOrDefault();
        //}
    }
}