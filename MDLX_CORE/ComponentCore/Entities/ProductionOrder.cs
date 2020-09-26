using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDLX_CORE.ComponentCore.Entities
{
    [Table("ProdOrder", Schema = "PRD")]
    public class ProductionOrder : IModelEntity
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string OrderNumber { get; set; }

        [Display(Name = "Data")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int QtyPlanned { get; set; }
        public int QtyRemain { get; set; }

        public int QtyProducedInPast { get; set; }

        public virtual Item Pnc { get; set; }
        public int PncId { get; set; }
        
        public virtual Resource2 Line { get; set; }
        public int LineId { get; set; }

        //[ForeignKey("Resource")]
        //public virtual Workstation Workst { get; set; }
        //public int WorkstId { get; set; }

        public string Notice { get; set; }
        public int Sequence { get; set; }
        public bool Deleted { get; set; }

        [MaxLength(25)]
        public string SerialNoFrom { get; set; }
        [MaxLength(25)]
        public string SerialNoTo { get; set; }

        public int CounterProductsIn { get; set; }
        public int CounterProductsOut { get; set; }
        public int CounterProductsFGW { get; set; }

        public DateTime? FirstProductIn { get; set; }
        public DateTime? LastProductIn { get; set; }
        public DateTime? FirstProductOut { get; set; }
        public DateTime? LastProductOut { get; set; }

        public DateTime LastUpdate { get; set; }

        [NotMapped]
        public bool Used { get; set; }
        [NotMapped]
        public int QtyToDisplay { get; set; }
        [NotMapped]
        public int QtyRemainCalculated { get { return QtyPlanned - CounterProductsIn; } }
        [NotMapped]
        public int Qty_PlannedOrRemain { get { return StartDate < DateTime.Now ? QtyPlanned : QtyRemain; } }
    }

    public class ProductionOrderFilter
    {
        [Display(Name = "Data")]
        public string Date { get; set; }

        [Display(Name = "Linia")]
        public string Line { get; set; }

        [Display(Name = "Numer Zlecenia")]
        public string OrderNumber { get; set; }

        [Display(Name = "PNC")]
        public string Pnc { get; set; }

        [Display(Name = "Ilość")]
        public string PCS { get; set; }

        [Display(Name = "R4h")]
        public string R4h { get; set; }

        [Display(Name = "R8h")]
        public string R8h { get; set; }

        [Display(Name = "R24h")]
        public string R24h { get; set; }
    }
}