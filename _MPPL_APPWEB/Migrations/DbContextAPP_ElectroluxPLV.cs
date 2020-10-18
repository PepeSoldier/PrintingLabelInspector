using System;
using System.Data.Entity;

namespace _LABELINSP_APPWEB.Migrations
{
    //Add-Migration -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.ElectroluxPLV.Configuration 1K
    //Update-Database -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.ElectroluxPLV.Configuration

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
            string clientName = _LABELINSP_APPWEB.Properties.Settings.Default.Client;
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


