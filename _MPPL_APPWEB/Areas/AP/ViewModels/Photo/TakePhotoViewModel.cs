using MDL_BASE.Models.Base;

namespace _MPPL_WEB_START.Areas.AP.ViewModel.Photo
{
    public class TakePhotoViewModel
    {
        public int ParentObjectId {get;set;}
        public AttachmentParentTypeEnum PhotoType { get; set; }
    }
}