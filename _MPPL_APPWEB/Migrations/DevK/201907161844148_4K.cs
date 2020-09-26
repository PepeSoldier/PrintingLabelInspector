namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4K : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("PFEP.AncPackage", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PFEP.AncPackage", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("PFEP.AncWorkstation", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PFEP.AncWorkstation", "BufferTypeId", "PFEP.DEF_Types");
            DropForeignKey("PFEP.AncWorkstation", "FeederTypeId", "PFEP.DEF_Types");
            DropForeignKey("PFEP.AncWorkstation", "MontageTypeId", "PFEP.DEF_Types");
            DropForeignKey("PFEP.AncWorkstation", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropIndex("PFEP.AncPackage", new[] { "AncId" });
            DropIndex("PFEP.AncPackage", new[] { "PackageId" });
            DropIndex("PFEP.AncWorkstation", new[] { "AncId" });
            DropIndex("PFEP.AncWorkstation", new[] { "WorkstationId" });
            DropIndex("PFEP.AncWorkstation", new[] { "MontageTypeId" });
            DropIndex("PFEP.AncWorkstation", new[] { "FeederTypeId" });
            DropIndex("PFEP.AncWorkstation", new[] { "BufferTypeId" });
            AddColumn("_MPPL.MASTERDATA_Workstation", "ProductsFromIn", c => c.Int(nullable: false));
            AddColumn("_MPPL.MASTERDATA_Workstation", "ProductsFromOut", c => c.Int(nullable: false));
            AddColumn("CORE.BOM", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("CORE.BOM", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("CORE.BOM_Workorder", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("CORE.BOM_Workorder", "InterItm", c => c.String());
            AddColumn("iLOGIS.CONFIG_Transporter", "Type", c => c.Int(nullable: false));
            AddColumn("PRD.ProdOrder", "QtyProducedInPast", c => c.Int(nullable: false));
            DropTable("PFEP.AncPackage");
            DropTable("PFEP.AncWorkstation");
        }
        
        public override void Down()
        {
            CreateTable(
                "PFEP.AncWorkstation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(nullable: false),
                        WorkstationId = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        BomQuantity = c.Int(nullable: false),
                        MontageTypeId = c.Int(nullable: false),
                        FeederTypeId = c.Int(nullable: false),
                        BufferTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PFEP.AncPackage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        Returnable = c.Boolean(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfBoxesOnPallet = c.Int(nullable: false),
                        Stackable = c.Boolean(nullable: false),
                        PackagingCard = c.Boolean(nullable: false),
                        PackagingCardFile = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("PRD.ProdOrder", "QtyProducedInPast");
            DropColumn("iLOGIS.CONFIG_Transporter", "Type");
            DropColumn("CORE.BOM_Workorder", "InterItm");
            DropColumn("CORE.BOM_Workorder", "UnitOfMeasure");
            DropColumn("CORE.BOM", "EndDate");
            DropColumn("CORE.BOM", "StartDate");
            DropColumn("_MPPL.MASTERDATA_Workstation", "ProductsFromOut");
            DropColumn("_MPPL.MASTERDATA_Workstation", "ProductsFromIn");
            CreateIndex("PFEP.AncWorkstation", "BufferTypeId");
            CreateIndex("PFEP.AncWorkstation", "FeederTypeId");
            CreateIndex("PFEP.AncWorkstation", "MontageTypeId");
            CreateIndex("PFEP.AncWorkstation", "WorkstationId");
            CreateIndex("PFEP.AncWorkstation", "AncId");
            CreateIndex("PFEP.AncPackage", "PackageId");
            CreateIndex("PFEP.AncPackage", "AncId");
            AddForeignKey("PFEP.AncWorkstation", "WorkstationId", "_MPPL.MASTERDATA_Workstation", "Id");
            AddForeignKey("PFEP.AncWorkstation", "MontageTypeId", "PFEP.DEF_Types", "Id");
            AddForeignKey("PFEP.AncWorkstation", "FeederTypeId", "PFEP.DEF_Types", "Id");
            AddForeignKey("PFEP.AncWorkstation", "BufferTypeId", "PFEP.DEF_Types", "Id");
            AddForeignKey("PFEP.AncWorkstation", "AncId", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("PFEP.AncPackage", "PackageId", "iLOGIS.CONFIG_Package", "Id");
            AddForeignKey("PFEP.AncPackage", "AncId", "_MPPL.MASTERDATA_Item", "Id");
        }
    }
}
