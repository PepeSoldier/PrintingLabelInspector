namespace _LABELINSP_APPWEB.Migrations.PackingLabel
{
    using _LABELINSP_APPWEB.Migrations;
    using MDLX_CORE.Models.IDENTITY;
    using System.Data.Entity.Migrations;
    using XLIB_COMMON.Repo.IDENTITY;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContextAPP_PackingLabel>
    {
        //enable-migrations -ContextTypeName  _LABELINSP_APPWEB.Migrations.DbContextAPP_PackingLabel -MigrationsDirectory:Migrations.PackingLabel
        //Add-Migration -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.PackingLabel.Configuration 1K
        //Update-Database -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.PackingLabel.Configuration

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