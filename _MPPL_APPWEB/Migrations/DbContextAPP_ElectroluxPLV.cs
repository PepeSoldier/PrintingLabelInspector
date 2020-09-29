using System;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_ElectroluxPLV : DbContextAPP_
    {

        //public DbSet<Model> Models { get; set; }


        public DbContextAPP_ElectroluxPLV() : base("ElectroluxPLVConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbContextAPP_ElectroluxPLV(string connectionName) : base(connectionName)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string clientName = _MPPL_WEB_START.Properties.Settings.Default.Client;
            return clientName + "Connection";
        }
        public static new DbContextAPP_ElectroluxPLV Create()
        {
            return new DbContextAPP_ElectroluxPLV(GetConnectionName());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}


