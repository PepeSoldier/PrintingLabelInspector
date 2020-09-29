namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_PackingLabel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(maxLength: 10),
                        PncId = c.Int(nullable: false),
                        SerialNumber = c.String(maxLength: 50),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.PncId)
                .Index(t => t.PncId);
            
            CreateTable(
                "_MPPL.MASTERDATA_PackingLabelTest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackingLabelId = c.Int(nullable: false),
                        TestName = c.String(maxLength: 50),
                        ExpectedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LabelType = c.Int(nullable: false),
                        Result = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_PackingLabel", t => t.PackingLabelId)
                .Index(t => t.PackingLabelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.MASTERDATA_PackingLabelTest", "PackingLabelId", "_MPPL.MASTERDATA_PackingLabel");
            DropForeignKey("_MPPL.MASTERDATA_PackingLabel", "PncId", "_MPPL.MASTERDATA_Item");
            DropIndex("_MPPL.MASTERDATA_PackingLabelTest", new[] { "PackingLabelId" });
            DropIndex("_MPPL.MASTERDATA_PackingLabel", new[] { "PncId" });
            DropTable("_MPPL.MASTERDATA_PackingLabelTest");
            DropTable("_MPPL.MASTERDATA_PackingLabel");
        }
    }
}
