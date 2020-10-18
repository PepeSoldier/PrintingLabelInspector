using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDLX_CORE.Models
{
    public class MenuItem : IMenuItem
    {
        public MenuItem()
        {
            Children = new IMenuItem[0];
        }
        public int Id { get; set; }
        public int AccessCode { get; set; }

        public string Name { get; set; }
        public string NameEnglish { get; set; }
        public string HrefArea { get; set; }
        public string HrefController { get; set; }
        public string HrefAction { get; set; }
        public string PictureURL { get; set; }
        public string RequiredRole { get; set; }
        public string HashSuffix { get; set; }
        public string PictureName { get; set; }
        public string ElementId { get; set; }
        public string Category { get; set; }

        public string Class1 { get; set; }
        public string Class2 { get; set; }
        public string Class3 { get; set; }
        public string Class4 { get; set; }
        public string Class5 { get; set; }

        public IMenuItem[] Children { get; set; }
    }

    public interface IMenuItem
    {
        int Id { get; set; }
        int AccessCode { get; set; }

        string Name { get; set; }
        string NameEnglish { get; set; }
        string HrefArea { get; set; }
        string HrefController { get; set; }
        string HrefAction { get; set; }
        string PictureURL { get; set; }
        string RequiredRole { get; set; }
        string HashSuffix { get; set; }
        string PictureName { get; set; }
        string ElementId { get; set; }
        string Category { get; set; }

        string Class1 { get; set; }
        string Class2 { get; set; }
        string Class3 { get; set; }
        string Class4 { get; set; }
        string Class5 { get; set; }

        IMenuItem[] Children { get; set; }
    }
}