using MDL_AP.Models.ActionPlan;
using MDL_AP.Models.DEF;
using MDL_BASE.Interfaces;
using System.Data.Entity;

namespace MDL_AP.Interfaces
{
    public interface IDbContextAP : IDbContextCore
    {
        DbSet<ActionModel> Actions { get; set; }
        DbSet<ActionActivity> ActionActivities { get; set; }
        DbSet<ActionObserver> ActionObservers { get; set; }

        DbSet<Meeting> Meetings { get; set; }
        DbSet<Models.DEF.Type> Types { get; set; }
        DbSet<Category> Categories { get; set; }
    }
}