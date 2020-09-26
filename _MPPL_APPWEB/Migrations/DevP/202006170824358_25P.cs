namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.CORE_NotificationDevice", newName: "CORE_NotificationDevice");
            AddColumn("_MPPL.CORE_NotificationDevice", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.CORE_NotificationDevice", "RegistrationDate");
            RenameTable(name: "CORE_NotificationDevice", newName: "_MPPL.CORE_NotificationDevice");
        }
    }
}
