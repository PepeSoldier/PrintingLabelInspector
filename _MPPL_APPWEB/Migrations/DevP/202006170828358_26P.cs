namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _26P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.CORE_NotificationDevice", newName: "NotificationDevice");
            MoveTable(name: "_MPPL.NotificationDevice", newSchema: "CORE");
        }
        
        public override void Down()
        {
            MoveTable(name: "CORE.NotificationDevice", newSchema: "_MPPL");
            RenameTable(name: "_MPPL.NotificationDevice", newName: "CORE_NotificationDevice");
        }
    }
}
