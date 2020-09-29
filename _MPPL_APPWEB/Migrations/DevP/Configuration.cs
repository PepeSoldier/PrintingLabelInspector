namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal class Configuration : DbMigrationsConfiguration<DbContextAPP_DevP>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\DevP";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevP -MigrationsDirectory:Migrations\DevP
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration 1P
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration
        }

        protected override void Seed(DbContextAPP_DevP context)
        {
            CnfigurationHelper.Seed(context);
        }
    }

    //internal sealed class Configuration : Configuration_
    //{
    //    public Configuration() : base(@"Migrations\DevP")
    //    {
    //    }

    //    //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Dev -MigrationsDirectory:Migrations\DevP
    //    //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration 1P
    //    //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration
    //    //
    //}
}
