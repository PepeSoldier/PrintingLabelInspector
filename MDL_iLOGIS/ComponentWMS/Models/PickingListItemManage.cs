using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class PickingListItemManage
    {
        public PickingListItem PickingListItem { get; set; }
        public int WorkOrderId { get; set; }
        public string DefaultTrolley { get; set; }
        public int TrasporterId { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> CommentList { get; set; }
    }
}
