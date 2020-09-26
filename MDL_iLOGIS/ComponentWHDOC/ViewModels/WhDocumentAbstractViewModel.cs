using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWHDOC.Enums;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MDL_iLOGIS.ComponentWHDOC.ViewModels
{
    public class WhDocumentAbstractViewModel
    {
        public int Id { get; set; }
        public int? ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorAdress { get; set; }
        public string DocumentNumber { get; set; }
        public string CostCenter { get; set; }
        public string CostPayer { get; set; }
        public string Reason { get; set; }
        public string ReferrenceDocument { get; set; }

        public string CreatorName { get; set; }
        public string ApproverName { get; set; }
        public string IssuerName { get; set; }
        public string SecurityApproverName { get; set; }
        
        public string Notice { get; set; }
        public string TrailerPlateNumbers { get; set; }
        public string TruckPlateNumbers { get; set; }


        public string ApproverId { get; set; }
        public string CreatorId { get; set; }
        public string IssuerId { get; set; }
        public string SecurityApproverId { get; set; }
        public string DocumentDateStr { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime StampTime { get; set; }
        public DateTime ApproveDate { get; set; }

        public string DocumentType { get; set; }
        public bool isSigned { get; set; }

        public decimal ItemsTotalQty { get; set; }
        public decimal ItemsCount { get; set; }
        public bool EnableEditing { get; set; }

        public EnumWhDocumentStatus Status { get; set; }
        public bool Deleted { get; set; }

        //other
        public string ItemCode { get; set; }
        public List<WhDocumentItemViewModel> WhDocumentItems { get; set; }
        public IEnumerable<object> ApproverList{ get; set; }
        //[AllowHtml]
        public string QrCode { get; set; }

        public override string ToString()
        {
            return Id.ToString() + ". " + ContractorName + "-" + DocumentNumber;
        }

    }
    
}
