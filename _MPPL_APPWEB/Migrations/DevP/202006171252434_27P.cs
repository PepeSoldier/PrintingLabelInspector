namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27P : DbMigration
    {
        public override void Up()
        {
            AlterColumn("CORE.NotificationDevice", "UserId", c => c.String(maxLength: 128));
            CreateIndex("CORE.NotificationDevice", "UserId");
            AddForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            AlterColumn("CORE.NotificationDevice", "UserId", c => c.String());
        }
    }
}
