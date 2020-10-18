using MDL_BASE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("ItemData", Schema = "LABELINSP")]
    public class ItemData : IModelEntity
    {
        public int Id { get; set; }
        
        /// <summary>itemCode is a PNC</summary>
        [MaxLength(50)]
        public string ItemCode { get; set; }
        /// <summary>itemCode is a ELC</summary>
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
        [NotMapped]
        public string ExpectedBarcodeSmall { get { return ExpectedProductCode.Length > 1 ? ExpectedProductCode + "215529" : ""; } }

        [NotMapped]
        public string ActualName { get; set; }
        [NotMapped]
        public string ActualProductCode { get; set; }
        [NotMapped]
        public string ActualWeightKG { get; set; }
        [NotMapped]
        public string ActualBarcode { get; set; }

        [NotMapped]
        public bool IsDataEmpty { get; set; }

    }
}