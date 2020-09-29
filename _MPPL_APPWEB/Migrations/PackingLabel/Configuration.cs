namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using _MPPL_WEB_START.Migrations;
    using MDL_BASE.Models.IDENTITY;
    using System.Data.Entity.Migrations;
    using XLIB_COMMON.Repo.IDENTITY;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContextAPP_PackingLabel>
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_PackingLabel -MigrationsDirectory:Migrations.PackingLabel
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.PackingLabel.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.PackingLabel.Configuration

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\PackingLabel";
        }

        protected override void Seed(DbContextAPP_PackingLabel context)
        {
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);

            //_roleManager.AddRole(DefRoles.ADMIN);
            //_roleManager.AddRole(DefRoles.USER);

            //_userManager.AddUser("Admin", DefRoles.ADMIN, "admin123$");
        }
    }
}