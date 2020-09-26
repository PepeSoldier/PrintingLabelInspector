using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PRD.Entity;
using MDL_PRD.Model;
using System.Data.Entity;

namespace MDL_PRD.Interface
{
    public interface IDbContextPRD : IDbContextCore
    {
        DbSet<OrderModel> PA_Order { get; set; }
        DbSet<OrderArchiveModel> PA_OrderArch { get; set; }
        DbSet<ReasonModel> PA_Reason { get; set; }

        DbSet<ProdOrderSequence> ProdOrderSequence { get; set; }
        DbSet<ProdOrderStatus> ProdOrderStatuses { get; set; }
        DbSet<Prodorder20> ProdOrder20 { get; set; }

        DbSet<Calendar> Calendar { get; set; }
    }
}