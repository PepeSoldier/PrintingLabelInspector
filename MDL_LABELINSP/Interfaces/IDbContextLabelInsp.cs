﻿using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Entities;
using MDL_LABELINSP.Entities;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA._Interfaces;
using System.Data.Entity;

namespace MDL_LABELINSP.Interfaces
{
    public interface IDbContextLabelInsp : IDbContextMasterData
    {
        //ID
        IDbSet<User> Users { get; set; }
        IDbSet<ApplicationRole> Roles { get; set; }
        IDbSet<UserRole> UserRoles { get; set; }

        //MasterData
        DbSet<ItemData> ItemData { get; set; }
        DbSet<Workorder> Workorders { get; set; }
        DbSet<WorkorderLabel> WorkorderLabels { get; set; }
        DbSet<WorkorderLabelInspection> WorkorderLabelInspections { get; set; }

        DbSet<SystemVariable> SystemVariables { get; set; }
        DbSet<Printer> Printers { get; set; }

        //BASE
        //DbSet<ProductionOrder> ProductionOrders { get; set; }
        //DbSet<Bom> Boms { get; set; }
        //DbSet<BomWorkorder> BomWorkorders { get; set; }
        //DbSet<Attachment> Attachments { get; set; }
        //DbSet<ChangeLog> ChangeLogs { get; set; }
        //DbSet<NotificationDevice> NotificationDevices { get; set; }
    }
}