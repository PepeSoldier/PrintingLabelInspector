using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Repo.ELDISY_PFEP
{
    public class PackingInstructionPhotoRepo : RepoGenericAbstract<PackingInstructionPhoto>
    {
        protected new IDbContextPFEP_Eldisy db;

        public PackingInstructionPhotoRepo(IDbContextPFEP_Eldisy db) : base(db)
        {
            this.db = db;
        }

        public override PackingInstructionPhoto GetById(int id)
        {
            return db.PackingInstructionPhotos.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<PackingInstructionPhoto> GetList()
        {
            return db.PackingInstructionPhotos.OrderByDescending(x => x.Id);
        }
        public List<PackingInstructionPhoto> GetByPackingInstruction(PackingInstruction packingInst)
        {
            int id = packingInst != null ? packingInst.Id : 0;
            return db.PackingInstructionPhotos.Where(x => x.ParentObjectId == id).ToList();
        }
        public PackingInstructionPhoto Add(int packingInstructionId, string name)
        {
            PackingInstructionPhoto packingInstrPhoto = new PackingInstructionPhoto();
            packingInstrPhoto.Extension = "jpg";
            packingInstrPhoto.Name = name;
            packingInstrPhoto.SubDirectory = "packinginstr";
            packingInstrPhoto.ParentObjectId = packingInstructionId;
            packingInstrPhoto.ParentType = MDL_BASE.Models.Base.AttachmentParentTypeEnum.none;

            this.AddOrUpdate(packingInstrPhoto);

            return packingInstrPhoto;
        }
    }
}