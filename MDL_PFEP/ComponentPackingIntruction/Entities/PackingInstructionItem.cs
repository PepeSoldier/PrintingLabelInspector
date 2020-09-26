using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_PFEP.Model.ELDISY_PFEP
{
    [Table("PackingInstruction_Package", Schema = "PFEP")]
    public class PackingInstructionPackage : IModelEntity
    {
        public int Id { get; set; }

        public decimal? Amount { get; set; }

        public UnitOfMeasure UnitOfMeasure{ get; set; }

        public string PriceForHundredPackages { get; set; }

        public virtual Package Package { get; set; }
        public int PackageId { get; set; }

        public virtual PackingInstruction PackingInstruction { get; set; }
        public int PackingInstructionId { get; set; }

        public decimal GetValueOfPackage
        {
            get
            {
                if (this.Amount != null)
                {
                    return (decimal)this.Amount * Package.UnitPrice;
                }
                else
                {
                    return 0;
                }
            }           
        }

    }    
}