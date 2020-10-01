using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_CORE.ComponentCore.Entities
{
    [Table("MASTERDATA_PackingLabelTest")]
    public class PackingLabelTest : IModelEntity
    {
        public int Id { get; set; }

        public PackingLabel PackingLabel { get; set; }
        public int PackingLabelId { get; set; }

        [MaxLength(50)]
        public string TestName { get; set; }

        public decimal ExpectedValue { get; set; }

        public decimal ActualValue { get; set; }

        public decimal Tolerance { get; set; }

        public EnumLabelType LabelType { get; set; }

        public bool Result { get; set; }
    }
}