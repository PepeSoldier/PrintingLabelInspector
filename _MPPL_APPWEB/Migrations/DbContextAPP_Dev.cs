using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Common;


namespace _MPPL_WEB_START.Migrations
{

    public class DbContextAPP_DevK : DbContextAPP_Dev
    {
        public DbContextAPP_DevK() : base("DevKConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevK -MigrationsDirectory:Migrations.DevK
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration
        }
    }

    public class DbContextAPP_DevP : DbContextAPP_Dev
    {
        public DbContextAPP_DevP() : base("DevPConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevP -MigrationsDirectory:Migrations.DevP
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration
        }
    }

    public class DbContextAPP_DevPE : DbContextAPP_Dev
    {
        public DbContextAPP_DevPE() : base("DevPEConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevPE -MigrationsDirectory:Migrations.DevPE
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevPE.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevPE.Configuration
        }
    }

    public class DbContextAPP_Dev : 
                    DbContextAPP_
    {
        public DbContextAPP_Dev() : base("Dev")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Dev -MigrationsDirectory:Migrations.Dev
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Dev.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Dev.Configuration
        }

        public DbContextAPP_Dev(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbContextAPP_Dev(DbConnection connection) : base(connection)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string HostName = System.Environment.MachineName;
            List<string> HostsDevK = new List<string>(){ "DESKTOP-VTS936D", "SAMKAM10", "IN0001" };
            List<string> HostsDevP = new List<string>(){ "C4AEDF-NB", "IN0003" };

            if (HostsDevK.Contains(HostName))
            {
                return "DevKConnection";
            }
            else if(HostsDevP.Contains(HostName))
            {
                return "DevPConnection";
            }
            else
            {
                return "DevPConnection";
            }
        }

        public static new DbContextAPP_Dev Create()
        {
            return new DbContextAPP_Dev(GetConnectionName());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }   
}


