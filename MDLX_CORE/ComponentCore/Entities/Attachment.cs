using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MDL_BASE.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Hosting;
using System.Web;

namespace MDL_BASE.Models.Base
{
    [Table("Attachment", Schema = "CORE")]
    public class Attachment : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string SubDirectory { get; set; }
        [MaxLength(50)]
        public string FileNamePrefix { get; set; }
        [MaxLength(50)]
        public string FileNameSuffix { get; set; }
        [MaxLength(150)]
        public string PackingCardUrl { get; set; }
        [MaxLength(10)]
        public string Extension { get; set; }
        public int ParentObjectId { get; set; }
        public AttachmentParentTypeEnum ParentType { get; set; }

        [NotMapped]
        public string UploadErrorMessage { get; set; }
        [NotMapped]
        public string ShortN
        {
            get
            {
                if (this.Name != null && this.Name.Length > 13)
                {
                    return this.Name.Substring(0, 10) + "...";
                }
                else
                {
                    return this.Name;
                }
            }
            set { }
        }

        public static string ConstructFileName(Attachment file, string customSuffix = null)
        {
            string prefix = file.FileNamePrefix != null ? string.Concat(file.FileNamePrefix, "_") : string.Empty;
            string suffix = file.FileNameSuffix != null ? file.FileNameSuffix : string.Empty;
            suffix = customSuffix != null ? string.Concat(suffix, customSuffix) : suffix;

            return string.Concat(prefix, file.Id, "-", file.ParentObjectId, suffix, ".", file.Extension);
        }
        public static string ConstructFullFilePath(Attachment file, string customSuffix = null)
        {
            string subdirectory = file.SubDirectory != null ? string.Concat(file.SubDirectory, "\\") : string.Empty;
            string ffp = System.IO.Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), subdirectory, ConstructFileName(file, customSuffix));
            return ffp;
        }
    }

    public static class AttachmentHelper
    {
        public static string GetPostedFileName(HttpPostedFileBase file, string browserName)
        {
            string fileName = string.Empty;

            if (browserName.ToUpper() == "IE" || browserName.ToUpper() == "INTERNETEXPLORER")
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
        public static string GetFileExtension(string fileName)
        {
            return fileName.Split('.').Last().ToLower();
        }
        public static bool IsExtensionPhotoAllowed(string fileName)
        {
            return GetAllowedPhotoExtensions().Contains(GetFileExtension(fileName));
        }
        public static bool IsExtensionExcelAllowed(string fileName)
        {
            return GetAllowedDocumentExtensionsExcel().Contains(GetFileExtension(fileName));
        }
        public static bool IsExtensionFileAllowed(string fileName)
        {
            return GetAllowedDocumentExtensions().Contains(GetFileExtension(fileName));
        }
        public static List<string> GetAllowedPhotoExtensions()
        {
            return new List<string> { "jpg", "jpeg", "png", "bmp", "gif" };
        }
        public static List<string> GetAllowedDocumentExtensions()
        {
            return new List<string> { "pdf" };
        }

        public static List<string> GetAllowedDocumentExtensionsExcel()
        {
            return new List<string> { "xlsx" };
        }
    }
}