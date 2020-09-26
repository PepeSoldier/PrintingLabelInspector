namespace _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging
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
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "MaxPackages", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "SafetyStock", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "MaxBomQty", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "CheckOnly", c => c.Boolean(nullable: false));
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
            
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "CheckOnly");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "MaxBomQty");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "SafetyStock");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "MaxPackages");
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
