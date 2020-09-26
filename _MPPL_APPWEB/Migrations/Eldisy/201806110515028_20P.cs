namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.ChangeLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectName = c.String(maxLength: 70),
                        FieldName = c.String(maxLength: 70),
                        NewValue = c.String(maxLength: 255),
                        OldValue = c.String(maxLength: 255),
                        ObjectId = c.Int(nullable: false),
                        ParentObjectId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("_MPPL.ChangeLog", new[] { "UserId" });
            DropTable("_MPPL.ChangeLog");
        }
    }
}
