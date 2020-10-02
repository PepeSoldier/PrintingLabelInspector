using MDL_BASE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("LABELINSP_ExpectedValues")]
    public class ExpectedValues : IModelEntity
    {
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(50)]
        public string ItemVersion { get; set; }

        [MaxLength(50)]
        public string ExpectedName { get; set; }

        [MaxLength(50)]
        public string ExpectedProductCode { get; set; }

        [MaxLength(50)]
        public string ExpectedWeightKG { get; set; }

        [MaxLength(50)]
        public string ExpectedWeightLBS { get; set; }

    }
}