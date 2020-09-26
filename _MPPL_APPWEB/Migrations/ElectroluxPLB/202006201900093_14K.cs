namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.NotificationDevice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PushEndpoint = c.String(),
                        PushP256DH = c.String(),
                        PushAuth = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            DropTable("CORE.NotificationDevice");
        }
    }
}
