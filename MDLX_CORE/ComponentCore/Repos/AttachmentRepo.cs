using XLIB_COMMON.Repo;
using System.Linq;
using MDL_BASE.Interfaces;
using System.IO;
using System.Web.Hosting;
using MDL_BASE.Models.Base;

namespace XLIB_COMMON.Repo.Base
{
    public class AttachmentRepo : RepoGenericAbstract<Attachment>
    {
        protected new IDbContextCore db;
        
        public AttachmentRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        public override Attachment GetById(int parentObjectId)
        {
            return db.Attachments.FirstOrDefault(d => d.ParentObjectId == parentObjectId);
        }
        public override IQueryable<Attachment> GetList()
        {
            return db.Attachments.OrderByDescending(x => x.Id);
        }

        public void DeleteFile(Attachment attachment)
        {
            if(attachment.Extension == "jpg")
            {
                File.Delete(Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), attachment.Id + "-" + attachment.ParentObjectId + "B.jpg"));
                File.Delete(Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), attachment.Id + "-" + attachment.ParentObjectId + "M.jpg"));
            }
            else
            {
                File.Delete(Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), attachment.Id + "-" + attachment.ParentObjectId + "." + attachment.Extension));
                File.Delete(Path.Combine(HostingEnvironment.MapPath("/Uploads/"), attachment.ParentObjectId + "-" + (int)attachment.ParentType + "." + attachment.Extension));
            }
            
            Delete(attachment);
        }
    }
}