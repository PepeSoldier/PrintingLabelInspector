using MDL_AP.Models.ActionPlan;
using _MPPL_WEB_START.Areas.AP.ViewModel.Photo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using MDL_AP.Repo;
using XLIB_COMMON.Interface;
using _MPPL_WEB_START.Migrations;
using _MPPL_WEB_START.Areas.Models;
using MDL_BASE.Models.Base;
using MDL_AP.Interfaces;
using MDL_BASE.Models;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class AttachmentController : BaseController
    {
        IDbContextAP db;
        UnitOfWorkActionPlan unitOfWork;
        List<Attachment> attachmentList;

        public AttachmentController(IDbContextAP db)
        {
            this.db = db;
            unitOfWork = new UnitOfWorkActionPlan(db);
            attachmentList = new List<Attachment>();
        }

        [HttpPost]
        public JsonResult Upload(int parentObjectId, int parentType)
        {
            AttachmentParentTypeEnum parentType1 = (AttachmentParentTypeEnum)parentType;
            HttpPostedFileBase file;
            int uploadCount = 0; 
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (IsExtensionPhotoAllowed(GetPostedFileName(file)))
                {
                    uploadCount += UploadAttachmentPhoto(file, parentObjectId, parentType1);
                }
                else if (IsExtensionFileAllowed(GetPostedFileName(file)))
                {
                    //upload PDF or XLSX
                    uploadCount += UploadAttachmentFile(file, parentObjectId, parentType1);
                }
                else
                {
                    attachmentList.Add(new Attachment
                    {
                        Id = 0,
                        Extension = GetFileExtension(GetPostedFileName(file)),
                        UploadErrorMessage = "Niedozwolony format pliku. Załaduj jeden z formatów: " + String.Join(", ", GetAllowedPhotoExtensions().ToArray()) + "."
                    });
                }
            }

            if (Request.Files.Count <= uploadCount)
                return Json("Nie wszystkie pliki zostały załadowane!");
            else if (Request.Files.Count <= 0)
                return Json("Nie wybrano pliku");
            else
                return Json("Wszystkie pliki zostały załadowane");
        }
        //[HttpPost]
        //public JsonResult UploadPhotoActivity(int parentObjectId)
        //{
        //    AttachmentParentTypeEnum photoType = AttachmentParentTypeEnum.ActivityAttachment;

        //    if (Request.Files.Count > 0)
        //    {
        //        return UploadPhoto(Request.Files, parentObjectId, photoType);
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}
        //[HttpPost]
        //public JsonResult UploadPhotoAction(int parentObjectId)
        //{
        //    AttachmentParentTypeEnum photoType = AttachmentParentTypeEnum.ActionAttachment;

        //    if (Request.Files.Count > 0)
        //    {
        //        return UploadPhoto(Request.Files, parentObjectId, photoType);
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}
        //[HttpPost]
        //public JsonResult UploadPhotoObservation(int parentObjectId)
        //{
        //    AttachmentParentTypeEnum photoType = AttachmentParentTypeEnum.ObservationAttachment;

        //    if (Request.Files.Count > 0)
        //    {
        //        return UploadPhoto(Request.Files, parentObjectId, photoType);
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}
        [HttpGet]
        public ActionResult TakeAPhoto(int parentObjectId, AttachmentParentTypeEnum photoType)
        {
            TakePhotoViewModel vm = new TakePhotoViewModel();
            vm.ParentObjectId = parentObjectId;
            vm.PhotoType = photoType;
            return View(vm);
        }
        [HttpPost]
        public ActionResult SaveTakenPhoto()
        {
            string imgBase64 = Request["imgBase64"];
            int parentObjectId = Convert.ToInt32(Request["parentObjectId"]);
            AttachmentParentTypeEnum photoType = (AttachmentParentTypeEnum)Convert.ToInt32(Request["photoType"]);

            string base64str = imgBase64.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(base64str);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                Attachment photo = new Attachment { Extension = "png", ParentObjectId = parentObjectId, ParentType = photoType };
                unitOfWork.RepoAttachment.Add(photo);
                string fname = Path.Combine(Server.MapPath("~/Uploads/"), photo.Id + "-" + photo.ParentObjectId + "." + photo.Extension);
                image.Save(fname);
            }

            return RedirectToAction("Browse", "Action");
        }
        [HttpPost]
        public JsonResult Delete(int parentObjectId, int parentType)
        {
            Attachment photo = unitOfWork.RepoAttachment.GetById(parentObjectId);
            unitOfWork.RepoAttachment.DeleteFile(photo);
            alertManager.AddAlert(AlertMessageType.info, "Załącznik został usunięty", User.Identity.Name);
            return Json("usunięty");
        }


        //----PRIVATE-FUNCTIONS----------------------------------------------------------------
        private int UploadAttachmentPhoto(HttpPostedFileBase file, int parentObjectId, AttachmentParentTypeEnum parentType)
        {
            int uploadCount = 0;

            try
            {
                if (IsExtensionPhotoAllowed(GetPostedFileName(file)))
                {
                    string newExtension = "jpg";
                    Attachment attachmentPhoto = new Attachment { Extension = newExtension, ParentObjectId = parentObjectId, ParentType = parentType };
                    unitOfWork.RepoAttachment.Add(attachmentPhoto);

                    ImageManager imageManager = new ImageManager();
                    imageManager.SaveBigPhoto(Image.FromStream(file.InputStream), attachmentPhoto);
                    imageManager.SaveMiniPhoto(Image.FromStream(file.InputStream), attachmentPhoto);

                    attachmentList.Add(attachmentPhoto);
                    uploadCount++;
                }
            }
            catch (Exception ex)
            {
                attachmentList.Add(new Attachment { Id = 0, UploadErrorMessage = "Error occurred. Error details: " + ex.Message + "." });
            }
            return uploadCount;
        }
        private int UploadAttachmentFile(HttpPostedFileBase file, int parentObjectId, AttachmentParentTypeEnum parentType)
        {
            int uploadCount = 0;

            try
            {
                string fileName = GetPostedFileName(file);
                string ext = GetFileExtension(fileName);

                if (IsExtensionFileAllowed(GetPostedFileName(file)))
                {
                    Attachment attachment = new Attachment { Extension = ext, ParentObjectId = parentObjectId, ParentType = parentType };
                    attachment.Name = file.FileName;
                    attachment.PackingCardUrl = (Path.Combine(Server.MapPath("~/Uploads/"), parentObjectId + "-" + (int)parentType + "." + ext));
                    unitOfWork.RepoAttachment.Add(attachment);

                    file.SaveAs(attachment.PackingCardUrl);

                    attachmentList.Add(attachment);
                    uploadCount++;
                }
            }
            catch (Exception ex)
            {
                attachmentList.Add(new Attachment { Id = 0, UploadErrorMessage = "Error occurred. Error details: " + ex.Message + "." });
            }

            return uploadCount;
        }

        private string GetPostedFileName(HttpPostedFileBase file)
        {
            string fileName = string.Empty;

            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            {
                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                fileName = testfiles[testfiles.Length - 1];
            }
            else
            {
                fileName = file.FileName;
            }

            return fileName;
        }
        private string GetFileExtension(string fileName)
        {
            return fileName.Split('.').Last().ToLower();
        }
        private bool IsExtensionPhotoAllowed(string fileName)
        {
            return GetAllowedPhotoExtensions().Contains(GetFileExtension(fileName));
        }
        private bool IsExtensionFileAllowed(string fileName)
        {
            return GetAllowedFileExtensions().Contains(GetFileExtension(fileName));
        }
        private List<string> GetAllowedPhotoExtensions()
        {
            return new List<string> { "jpg", "jpeg", "png", "bmp", "gif" };
        }
        private List<string> GetAllowedFileExtensions()
        {
            return new List<string> { "pdf", "xlsx", "xls" };
        }
    }
}