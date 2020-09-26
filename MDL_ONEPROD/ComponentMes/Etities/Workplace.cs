using MDL_BASE.Interfaces;
using MDL_ONEPROD.ComponentMes.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("MES_Workplace", Schema = "ONEPROD")]
    public class Workplace : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        public virtual ResourceOP Machine { get; set; }
        public int MachineId { get; set; }

        public virtual Workorder SelectedTask { get; set; }
        public int? SelectedTaskId { get; set; }

        [MaxLength(25), Index(IsUnique = true)]
        public string ComputerHostName { get; set; }

        [MaxLength(20)]
        public string PrinterIPv4 { get; set; }

        [MaxLength(40)]
        public string LoggedUserName { get; set; }

        [MaxLength(50)]
        public string LabelANC { get; set; }

        [MaxLength(15)]
        public string LabelName { get; set; }

        public bool PrintLabel { get; set; }

        public SerialNumberType SerialNumberType { get; set; }
        public PrinterType PrinterType { get; set; }
        public WorkplaceTypeEnum Type { get; set; }

        public int LabelLayoutNo { get; set; }

        public bool IsTraceability { get; set; }
        public bool IsReportOnline { get; set; }

        public bool Deleted { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}