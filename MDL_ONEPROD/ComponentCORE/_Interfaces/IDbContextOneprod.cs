using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Interface
{
    public interface IDbContextOneprod : IDbContextCore
    {
        //DbSet<MDL_ONEPROD.Model.Scheduling.ResourceGroup> Areas2 { get; set; }
        DbSet<ItemOP> ItemsOP { get; set; }
        DbSet<ResourceOP> ResourcesOP { get; set; }

        DbSet<MCycleTime> CycleTimes { get; set; }
        DbSet<Param> Params { get; set; }
        //DbSet<Item> Items { get; set; }
        DbSet<ItemInventory> ItemInventories { get; set; }
        //DbSet<Process> Processes { get; set; }
        DbSet<ClientOrder> ClientOrders { get; set; }
        DbSet<Workorder> Workorders { get; set; }
    }
}