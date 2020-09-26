using System.Linq;
using MDL_BASE.Interfaces;
using System.IO;
using System.Web.Hosting;
using MDL_BASE.Models.Base;
using MDL_AP.Models.ActionPlan;
using System;
using MDL_iLOGIS.ComponentPFEP.Models;

namespace MDL_AP.Repo.ActionPlan
{
    public class RepoAttachment : XLIB_COMMON.Repo.Base.AttachmentRepo
    {
        protected new IDbContextCore db;
        
        public RepoAttachment(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<Attachment> GetListByActivity(ActionActivity actionActivity)
        {
            int parentObjectId = (actionActivity != null) ? actionActivity.Id : 0;
            return db.Attachments.Where(x => x.ParentObjectId == parentObjectId && 
										x.ParentType == AttachmentParentTypeEnum.ActivityAttachment)
							.OrderByDescending(x => x.Id);
        }
        public IQueryable<Attachment> GetListByAction(ActionModel action)
        {
            int parentObjectId = (action != null) ? action.Id : 0;
            return db.Attachments.Where(x => x.ParentObjectId == parentObjectId &&
                                        x.ParentType == AttachmentParentTypeEnum.ActionAttachment)
                            .OrderByDescending(x => x.Id);
        }

        public void DeleteByActivity(ActionActivity actionActivity)
        {
            IQueryable<Attachment> photoList = GetListByActivity(actionActivity);
            foreach (Attachment photo in photoList)
            {
                DeleteFile(photo);
            }
        }
        public void DeleteByAction(ActionModel action)
        {
            IQueryable<Attachment> photoList = GetListByAction(action);
            foreach (Attachment photo in photoList)
            {
                DeleteFile(photo);
            }
        }

        public string GetByPFEPData(PFEPData item)
        {
            var attach = db.Attachments.Where(x => x.ParentObjectId == item.ItemId).FirstOrDefault();
            if(attach == null)
            {
                return "";
            }
            else
            {
                return attach.PackingCardUrl;
            }
        }
    }
}