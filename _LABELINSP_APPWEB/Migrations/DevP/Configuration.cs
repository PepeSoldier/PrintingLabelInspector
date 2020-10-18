namespace _LABELINSP_APPWEB.Migrations.DevP
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

            //enable-migrations -ContextTypeName  _LABELINSP_APPWEB.Migrations.DbContextAPP_DevP -MigrationsDirectory:Migrations\DevP
            //Add-Migration -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.DevP.Configuration 1P
            //Update-database -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.DevP.Configuration
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

    //    //enable-migrations -ContextTypeName  _LABELINSP_APPWEB.Migrations.DbContextAPP_Dev -MigrationsDirectory:Migrations\DevP
    //    //Add-Migration -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.DevP.Configuration 1P
    //    //Update-database -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.DevP.Configuration
    //    //
    //}
}
