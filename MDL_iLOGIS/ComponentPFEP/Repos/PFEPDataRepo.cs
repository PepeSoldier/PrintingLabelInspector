using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentPFEP.Models;
using System.Data.Entity;
using System;

namespace MDL_ONEPROD.ComponentWMS.Repos
{
    public class PFEPDataRepo
    {
        protected IDbContextiLOGIS db;

        public PFEPDataRepo(IDbContextiLOGIS db)
        {
            this.db = db;
        }

        public IQueryable<PFEPData> GetList(PFEPData filter)
        {
            var query = from itemWMS in db.ItemWMS.Where(x =>
                          //(x.Deleted == false) &&
                          (x.Item.Name.StartsWith("ELC ") == false) &&
                          (x.Item.BC != "42") &&
                          (filter.ItemCode == null || x.Item.Code.Contains(filter.ItemCode)) &&
                          (filter.ItemName == null || x.Item.Name.Contains(filter.ItemName)) &&
                          (filter.ItemDeleted == null || x.Item.Deleted != filter.ItemDeleted.Value) &&
                          (filter.DEF == null || x.Item.DEF == filter.DEF) &&
                          (filter.PREFIX == null || x.Item.PREFIX == filter.PREFIX) &&
                          (filter.BC == null || x.Item.BC == filter.BC) &&
                          (filter.ItemGroupName == null || (x.Item.ItemGroup != null && x.Item.ItemGroup.Name.Contains(filter.ItemGroupName)))
                        )
                        join w in db.WorkstationItems on itemWMS.Id equals w.ItemWMSId into w2
                        join p in db.PackageItems on itemWMS.Id equals p.ItemWMSId into p2
                        join a in db.Attachments.Where(x => x.ParentType == MDL_BASE.Models.Base.AttachmentParentTypeEnum.iLogisData) on itemWMS.Id equals a.ParentObjectId into a2
                        from w3 in w2.DefaultIfEmpty().Where(x2 =>
                            (filter.WorkstationLineName == null || x2.Workstation.Line.Name == filter.WorkstationLineName) &&
                            ((filter.WorkstationName == null || x2.Workstation.Name == filter.WorkstationName) || (filter.WorkstationName == "?" && x2 == null) ) &&
                            (filter.WorkstationSortOrder < 1 || x2.Workstation.SortOrder == filter.WorkstationSortOrder))
                        from p3 in p2.DefaultIfEmpty().Where(x3 =>
                            (filter.PackageReturnable == null || x3.Package.Returnable == filter.PackageReturnable.Value) &&
                            ( (filter.PackageCode == null || x3.Package.Code == filter.PackageCode) || (filter.PackageCode == "?" && x3 == null) ) &&
                            (filter.PackageName == null || x3.Package.Name.Contains(filter.PackageName)) &&
                            (filter.PackageW < 1 || x3.Package.Width == filter.PackageW) &&
                            (filter.PackageH < 1 || x3.Package.Height == filter.PackageH) &&
                            (filter.PackageD < 1 || x3.Package.Depth == filter.PackageD))
                        from a3 in a2.DefaultIfEmpty()
                        select new PFEPData()
                        {
                            ItemId = itemWMS.Id,
                            ItemCode = itemWMS.Item.Code,
                            ItemName = itemWMS.Item.Name,
                            ItemClass = "-",
                            ItemCreatedDate = itemWMS.Item.CreatedDate,
                            ItemDeleted = !itemWMS.Item.Deleted,
                            ItemGroupId = (itemWMS.Item.ItemGroup != null) ? itemWMS.Item.ItemGroup.Id : 0,
                            ItemGroupName = (itemWMS.Item.ItemGroup != null) ? itemWMS.Item.ItemGroup.Name : string.Empty,
                            DEF = itemWMS.Item.DEF,
                            PREFIX = itemWMS.Item.PREFIX,
                            BC = itemWMS.Item.BC,
                            PackageId = (p3 != null && p3.Package != null) ? p3.Package.Id : 0,
                            PackageCode = (p3 != null && p3.Package != null) ? p3.Package.Code : string.Empty,
                            PackageD = (p3 != null && p3.Package != null) ? p3.Package.Depth : 0,
                            PackageW = (p3 != null && p3.Package != null) ? p3.Package.Width : 0,
                            PackageH = (p3 != null && p3.Package != null) ? p3.Package.Height : 0,
                            PackageReturnable = (p3 != null && p3.Package != null) ? p3.Package.Returnable : false,
                            PackageName = (p3 != null && p3.Package != null) ? p3.Package.Name : string.Empty,
                            QtyPerPackage = (p3 != null) ? p3.QtyPerPackage : 0,
                            QtyPerPallet = (p3 != null) ? p3.QtyPerPackage * p3.PackagesPerPallet : 0,
                            WorkstationId = (w3 != null && w3.Workstation != null) ? w3.Workstation.Id : 0,
                            WorkstationName = (w3 != null && w3.Workstation != null) ? w3.Workstation.Name : string.Empty,
                            MaxBomQty = (w3 != null) ? w3.MaxBomQty : 0,
                            MaxPackages = (w3 != null) ? w3.MaxPackages : 0,
                            CheckOnly = (w3 != null) ? w3.CheckOnly : false,
                            WorkstationSortOrder = (w3 != null && w3.Workstation != null) ? w3.Workstation.SortOrder : 0,
                            WorkstationLineId = (w3 != null && w3.Workstation != null && w3.Workstation.Line != null) ? w3.Workstation.Line.Id : 0,
                            WorkstationLineName = (w3 != null && w3.Workstation != null && w3.Workstation.Line != null) ? w3.Workstation.Line.Name : string.Empty,
                            PackingCardUrl = (a3 != null) ? a3.ParentObjectId.ToString() + "-" + ((int)a3.ParentType).ToString() + "." + a3.Extension : string.Empty,
                        };

            return query;
        }
        
    }
}