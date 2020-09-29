namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal class Configuration : DbMigrationsConfiguration<DbContextAPP_DevK>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\DevK";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevK -MigrationsDirectory:Migrations\DevK
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration
        }

        protected override void Seed(DbContextAPP_DevK context)
        {
            CnfigurationHelper.Seed(context);   
        }
    }

    //internal sealed class Configuration : Configuration_ 
    //{
    //    public Configuration() : base(@"Migrations\DevK")
    //    {
    //    }

    //    //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Dev -MigrationsDirectory:Migrations\DevK
    //    //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration 1K
    //    //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration
    //}
}
