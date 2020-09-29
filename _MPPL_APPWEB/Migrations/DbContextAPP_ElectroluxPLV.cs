using System;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_ElectroluxPLV_Prod : DbContextAPP_ElectroluxPLV
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV -MigrationsDirectory:Migrations.ElectroluxPLV
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration -TargetMigration:"138K"

        //UPDATE [EPSS_2_PROD].[_MPPL].[__MigrationHistory] SET [ContextKey] = '_MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration'
        public DbContextAPP_ElectroluxPLV_Prod() : base("ElectroluxPLVConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }
    public class DbContextAPP_ElectroluxPLV_Staging : DbContextAPP_ElectroluxPLV
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV_Staging -MigrationsDirectory:Migrations.ElectroluxPLV_Staging
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging.Configuration
        public DbContextAPP_ElectroluxPLV_Staging() : base("ElectroluxPLV_StagingConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }

    public class DbContextAPP_ElectroluxPLV : 
                    DbContextAPP_
    {
        
        //public DbSet<Model> Models { get; set; }
        

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


