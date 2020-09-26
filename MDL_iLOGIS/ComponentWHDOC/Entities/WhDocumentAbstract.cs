using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWHDOC.Enums;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentWHDOC.Entities
{
    [Table("WHDOC_WhDocument", Schema = "iLOGIS")]
    public abstract class WhDocumentAbstract : IModelEntity
    {
        public WhDocumentAbstract()
        {
            DocumentDate = new DateTime(1900,1,1);
            ApproveDate = new DateTime(1900, 1, 1);
            StampTime = DateTime.Now;
        }

        public int Id { get; set; }

        public virtual Contractor Contractor { get; set; }
        public int? ContractorId { get; set; }

        [MaxLength(12)]
        public string DocumentNumber { get; set; }
        [MaxLength(40)]
        public string ReferrenceDocument { get; set; }
        [MaxLength(128)]
        public string CostPayer { get; set; }

        public DateTime DocumentDate { get; set; }
        public DateTime StampTime { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime? IssueDate { get; set; }

        [MaxLength(32)]
        public string CostCenter { get; set; }
        [MaxLength(128)]
        public string Reason { get; set; } //przeznaczenie
        [MaxLength(128)]
        public string MeansOfTransport { get; set; }

        [MaxLength(128)]
        public string Notice { get; set; }

        [MaxLength(128)]
        public string TruckPlateNumbers { get; set; }

        [MaxLength(128)]
        public string TrailerPlateNumbers { get; set; }

        //Ostatni wiersz:
        public virtual User Creator { get; set; }
        [MaxLength(128)]
        public string CreatorId { get; set; }

        public virtual User Issuer { get; set; }
        [MaxLength(128)]
        public string IssuerId { get; set; }


        public EnumWhDocumentStatus Status { get; set; }
        public bool isSigned { get; set; } //podpis ma być składowany na pdf'ie w formie zaszyfrowanej
        public bool Deleted { get; set; }

        public ICollection<WhDocumentItem> WhDocumentItems { get; set; }
    }
}