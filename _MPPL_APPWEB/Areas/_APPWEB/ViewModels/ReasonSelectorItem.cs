using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _LABELINSP_APPWEB.Areas._APPWEB.ViewModels
{
    public class ReasonSelectorItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int ParentId { get; set; }
        public string Color { get; set; }
        public string ColorGroup { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SubreasonsCount { get; set; }
        public int ReasonTypeId { get; set; }
        public string ReasonTypeName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}