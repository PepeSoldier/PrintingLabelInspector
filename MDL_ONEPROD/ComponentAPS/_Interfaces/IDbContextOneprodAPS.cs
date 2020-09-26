using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Interface
{
    public interface IDbContextOneprodAPS : IDbContextOneprod
    {
        DbSet<Calendar2> Calendar2 { get; set; }
        DbSet<ChangeOver> ChangeOvers { get; set; }

        DbSet<ItemGroupTool> ItemGroupTools { get; set; }
        DbSet<Tool> Tools { get; set; }
        DbSet<ToolChangeOver> ToolChangeOvers { get; set; }
        DbSet<ToolGroup> ToolGroups { get; set; }
        DbSet<ToolMachine> ToolMachines { get; set; }
    }
}