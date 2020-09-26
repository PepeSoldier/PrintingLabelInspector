using _MPPL_WEB_START.Migrations;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Repo;
using XLIB_COMMON.Repo;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDL_BASE.Models.Base;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    public class Attachment1Controller : BaseController
    {
        UnitOfWorkActionPlan UnitOfWorkActionPlan;

        public Attachment1Controller()
        {

            UnitOfWorkActionPlan = new UnitOfWorkActionPlan(DbContextAPP_ElectroluxPLV.Create());
        }

        //[HttpPost]
        //public JsonResult UploadAttachmentActivity(int parentObjectId)
        //{
        //    ExtensionTypeEnum extensionType = ExtensionTypeEnum.ActivityExtension;

        //    if (Request.Files.Count > 0)
        //    {
        //        return UploadExtension(Request.Files, parentObjectId, extensionType);
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}
        //[HttpPost]
        //public JsonResult UploadAttachmentAction(int parentObjectId)
        //{
        //    ExtensionTypeEnum extensionType = ExtensionTypeEnum.ActionExtension;

        //    if (Request.Files.Count > 0)
        //    {
        //        return UploadExtension(Request.Files, parentObjectId, extensionType);
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}

        //----PRIVATE-FUNCTIONS----------------------------------------------------------------
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
        private List<string> GetAllowedPhotoExtensions()
        {
            return new List<string> { "pdf" };
        }
        //private JsonResult UploadExtension(HttpFileCollectionBase files, int parentObjectId, ExtensionTypeEnum extensionType)
        //{
        //    HttpPostedFileBase file;
        //    string fileName = string.Empty;
        //    string ext = string.Empty;
        //    string msg = string.Empty;
        //    int attachmentId = 0;

        //    try
        //    {
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            file = files[i];
        //            fileName = GetPostedFileName(file);
        //            ext = GetFileExtension(fileName);

        //            if (IsExtensionPhotoAllowed(GetPostedFileName(file)))
        //            {
        //                ExtensionFile ca = new ExtensionFile();
        //                ca.FileExtension = ext;

        //                attachmentId = 0; //UnitOfWorkActionPlan.RepoExtensionFiles.Add(ca);
        //                UnitOfWorkActionPlan.RepoExtensionFiles.AssignEAttachmentToAction(parentObjectId, attachmentId);

        //                file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), parentObjectId + "-" + attachmentId + "." + ext));
        //                msg = "File Uploaded Successfully!";
        //            }
        //            else
        //            {
        //                msg = "Niedozwolony format pliku. Załaduj jeden z formatów: " + String.Join(", ", GetAllowedPhotoExtensions().ToArray()) + ".";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error occurred. Error details: " + ex.Message + ".";
        //    }
        //    return Json(msg);
        //}
    }
}
