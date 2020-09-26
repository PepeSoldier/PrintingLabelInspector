using System.Data.Entity;
using MDL_BASE.Models.MasterData;
using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Interfaces;
//using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Entities;

namespace MDLX_MASTERDATA._Interfaces
{
    public interface IDbContextMasterData : IDbContext
    {
        DbSet<Entities.Item> Items { get; set; }
        DbSet<Entities.ItemUoM> ItemUoMs { get; set; }
        DbSet<Entities.Resource2> Resources2 { get; set; }

        DbSet<Department> Departments { get; set; }
        DbSet<Area> Areas { get; set; }
        //DbSet<Resource2> Lines { get; set; }
        DbSet<Workstation> Workstations { get; set; }
        //DbSet<MDL_BASE.Models.MasterData.LabourBrigade> ShiftCodes { get; set; }
        //DbSet<Item> Items { get; set; }
        //DbSet<Item> Items { get; set; }
        DbSet<Contractor> Contractors { get; set; }
        DbSet<LabourBrigade> LabourBrigades { get; set; }
        DbSet<Process> Processes { get; set; }
        
    }
}
