namespace _LABELINSP_APPWEB.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_LABELINSP.LABELINSP_ExpectedValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemCode = c.String(maxLength: 50),
                        ItemVersion = c.String(maxLength: 50),
                        ExpectedName = c.String(maxLength: 50),
                        ExpectedProductCode = c.String(maxLength: 50),
                        ExpectedWeightKG = c.String(maxLength: 50),
                        ExpectedWeightLBS = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_LABELINSP.LABELINSP_Workorders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkorderNumber = c.String(maxLength: 50),
                        ItemCode = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SerialNumberFrom = c.String(maxLength: 50),
                        SerialNumberTo = c.String(maxLength: 50),
                        FirstInspectionDate = c.DateTime(nullable: false),
                        LastInspectionDate = c.DateTime(nullable: false),
                        SuccessfullInspections = c.Int(nullable: false),
                        FailfullInspections = c.Int(nullable: false),
                        FailInspectionLabelPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("_LABELINSP.LABELINSP_Workorders");
            DropTable("_LABELINSP.LABELINSP_ExpectedValues");
        }
    }
}
