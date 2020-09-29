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
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            //CnfigurationHelper.SeedDataNewRoles(context);

            //RoleRepo _roleManager = new RoleRepo(new RoleStore<ApplicationRole>(context), context);
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore(context), context);
            //UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            //RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();

            UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);

            _roleManager.AddRole(DefRoles.ADMIN);
            _roleManager.AddRole(DefRoles.USER);
            //_roleManager.AddRole(DefRoles.ILOGIS_VIEWER);
            //_roleManager.AddRole(DefRoles.ILOGIS_ADMIN);
            //_roleManager.AddRole(DefRoles.ILOGIS_OPERATOR);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_VIEWER);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_EDITOR);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_APPROVER);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_SECURITY);

            _userManager.AddUser("Admin", DefRoles.ADMIN, "admin123$");
            //_userManager.AddUser("User", DefRoles.ADMIN, "user123$");
            //_userManager.AddUser("iLogisUser", DefRoles.ADMIN, "iLogisUser123$");
            //_userManager.AddUser("iLogisWhDocApprover", DefRoles.ILOGIS_WHDOC_APPROVER, "iLogisWhDocApprover123$");
            //_userManager.AddUser("iLogisWhDocEditor", DefRoles.ILOGIS_WHDOC_EDITOR, "iLogisWhDocEditor123$");
            //_userManager.AddUser("iLogisWhDocViewer", DefRoles.ILOGIS_WHDOC_VIEWER, "iLogisWhDocViewer123$");

            ////SeedData_PLS(context);
            //CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
        }
    }
}