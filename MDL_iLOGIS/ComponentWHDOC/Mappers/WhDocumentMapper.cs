using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWHDOC.ViewModels;
using MDL_iLOGIS.ComponentWMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentWMS.Mappers
{
    public static class WhDocumentMapper
    {

        public static List<WhDocumentAbstractViewModel> ToList<T>(this IQueryable<WhDocumentWZ> source)
        {
            string typeNameOfDocument = source.ElementType.Name;
            IQueryable<WhDocumentAbstractViewModel> q = source.Select(x => new WhDocumentAbstractViewModel()
            {
                Id = x.Id,
                ContractorId = x.ContractorId,
                ContractorName = x.Contractor.Name,
                ContractorAdress = x.Contractor.ContactAdress,
                DocumentNumber = x.DocumentNumber,
                CostCenter = x.CostCenter,
                ReferrenceDocument = x.ReferrenceDocument,
                CreatorName = x.Creator.UserName,
                ApproverName = x.Approver != null ? x.Approver.UserName : "",
                SecurityApproverName = x.SecurityApprover != null ? x.SecurityApprover.UserName : "",
                SecurityApproverId = x.SecurityApproverId,
                ApproverId = x.ApproverId,
                ApproveDate = x.ApproveDate,
                DocumentDate = x.DocumentDate,
                //DocumentDateStr = x.DocumentDate.ToString("yyyy-MM-dd HH:mm"),
                StampTime = x.StampTime,
                DocumentType = typeNameOfDocument,
                Status = x.Status,
                isSigned = x.isSigned,
                ItemsTotalQty = x.WhDocumentItems.DefaultIfEmpty().Sum(s => s != null ? s.IssuedQty : 0),
                ItemsCount = x.WhDocumentItems.Count(),
                Deleted = x.Deleted
            });

            return q.ToList();
        }

        public static WhDocumentAbstractViewModel FirstOrDefault<T>(this WhDocumentWZ source)
        {
            string typeNameOfDocument = source.GetType().Name;
            WhDocumentAbstractViewModel q = new WhDocumentAbstractViewModel()
            {
                Id = source.Id,
                ContractorId = source.ContractorId != null ? source.ContractorId : 0 ,
                ContractorName = source.ContractorId != null ? source.Contractor.Name : "",
                ContractorAdress = source.ContractorId != null ? source.Contractor.ContactAdress : "",
                DocumentNumber = source.DocumentNumber,
                CostCenter = source.CostCenter,
                ReferrenceDocument = source.ReferrenceDocument,
                CreatorName = source.Creator != null? source.Creator.FirstName + " " + source.Creator.LastName : "",
                ApproverName = source.Approver != null? source.Approver.FirstName + " " + source.Approver.LastName : "",
                ApproverId = source.ApproverId,
                SecurityApproverName = source.SecurityApprover != null ? source.SecurityApprover.UserName : "",
                SecurityApproverId = source.SecurityApproverId,
                ApproveDate = source.ApproveDate,
                DocumentDate = source.DocumentDate,
                DocumentDateStr = source.DocumentDate.ToString("yyyy-MM-dd HH:mm"),
                IssueDate = source.IssueDate,
                IssuerName = source.Issuer != null ? source.Issuer.FirstName + " " + source.Issuer.LastName : "",

                StampTime = source.StampTime,
                DocumentType = typeNameOfDocument,
                Status = source.Status,
                isSigned = source.isSigned,
                Notice = source.Notice,
                TrailerPlateNumbers = source.TrailerPlateNumbers,
                TruckPlateNumbers = source.TruckPlateNumbers,
                Reason = source.Reason,
                ItemsTotalQty = source.WhDocumentItems != null ? source.WhDocumentItems.DefaultIfEmpty().Sum(s => s != null ? s.IssuedQty : 0) : 0,
                ItemsCount = source.WhDocumentItems != null ? source.WhDocumentItems.Count() : 0,
                Deleted = source.Deleted
            };

            return q;
        }
    }
}
