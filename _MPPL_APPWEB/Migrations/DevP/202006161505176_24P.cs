namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_NotificationDevice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        PushEndpoint = c.String(),
                        PushP256DH = c.String(),
                        PushAuth = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("iLOGIS.WHDOC_WhDocumentItem", "AdminEntry");
            DropColumn("iLOGIS.WHDOC_WhDocumentItem", "OperatorEntry");
            DropColumn("iLOGIS.WHDOC_WhDocumentItem", "WasPrinted");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.WHDOC_WhDocumentItem", "WasPrinted", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WHDOC_WhDocumentItem", "OperatorEntry", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WHDOC_WhDocumentItem", "AdminEntry", c => c.Boolean(nullable: false));
            DropTable("_MPPL.MASTERDATA_NotificationDevice");
        }
    }
}
