using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentVisualControl.Entities
{
    [Table("VISUALCONTROL_JobItemConfig", Schema = "OTHER")]
    public class JobItemConfig : IModelEntity
    {
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        public int ItemId { get; set; }

        public int JobNo { get; set; }
        public int PairNo { get; set; }
        public int CameraNo { get; set; }

        public JobItemTypeEnum Type { get; set; }
        public JobItemLocationEnum Location { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [NotMapped]
        public string ItemCode { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
    }
}
