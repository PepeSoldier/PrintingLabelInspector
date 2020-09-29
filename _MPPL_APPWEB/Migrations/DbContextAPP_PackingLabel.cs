using MDL_ONEPROD.ComponentQuality._Interfaces;
using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.Model.Scheduling;

//using MDL_PFEP.Models.PFEP;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_PackingLabel : DbContextAPP_, IDbContextOneprodQuality
    {
        public DbContextAPP_PackingLabel() : base("PackingLabelConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbContextAPP_PackingLabel(string connectionName) : base(connectionName)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string clientName = _MPPL_WEB_START.Properties.Settings.Default.Client;
            return clientName + "Connection";
        }

        public static new DbContextAPP_PackingLabel Create()
        {
            return new DbContextAPP_PackingLabel(GetConnectionName());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ItemParameter> ItemParameters { get; set; }
        public DbSet<ItemMeasurement> ItemMeasurements { get; set; }
        public DbSet<ItemOP> ItemsOP { get; set; }
        public DbSet<ResourceOP> ResourcesOP { get; set; }
        public DbSet<MCycleTime> CycleTimes { get; set; }
        public DbSet<Param> Params { get; set; }
        public DbSet<ItemInventory> ItemInventories { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<Workorder> Workorders { get; set; }
    }
}