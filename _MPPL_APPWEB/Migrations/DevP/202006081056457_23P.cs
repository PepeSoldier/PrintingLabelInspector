namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "iLOGIS.WHDOC_WhDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContractorId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 12),
                        ReferrenceDocument = c.String(maxLength: 40),
                        CostPayer = c.String(maxLength: 128),
                        DocumentDate = c.DateTime(nullable: false),
                        StampTime = c.DateTime(nullable: false),
                        CostCenter = c.String(maxLength: 32),
                        Reason = c.String(maxLength: 128),
                        MeansOfTransport = c.String(maxLength: 128),
                        CreatorId = c.String(maxLength: 128),
                        IssuerId = c.String(maxLength: 128),
                        IssueDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        isSigned = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ForwadrerComments = c.String(),
                        ApproverId = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.ContractorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.CreatorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.IssuerId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.ApproverId)
                .Index(t => t.ContractorId)
                .Index(t => t.CreatorId)
                .Index(t => t.IssuerId)
                .Index(t => t.ApproverId);
            
            CreateTable(
                "iLOGIS.WHDOC_WhDocumentItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        WhDocumentId = c.Int(nullable: false),
                        ItemWMSId = c.Int(),
                        PackageId = c.Int(),
                        ItemCode = c.String(maxLength: 100),
                        ItemName = c.String(maxLength: 100),
                        DisposedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssuedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        AdminEntry = c.Boolean(nullable: false),
                        OperatorEntry = c.Boolean(nullable: false),
                        WasPrinted = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("iLOGIS.WHDOC_WhDocument", t => t.WhDocumentId)
                .Index(t => t.UserId)
                .Index(t => t.WhDocumentId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.PackageId);
            
            AddColumn("ONEPROD.APS_Calendar", "Date", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Hours", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxCycleTime", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Efficiency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId", c => c.Int());
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyRequested", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyDelivered", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyUsed", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.CONFIG_PackageItem", "QtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            CreateIndex("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            AddForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "ApproverId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "WhDocumentId", "iLOGIS.WHDOC_WhDocument");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "IssuerId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "CreatorId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "ContractorId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "PackageId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "WhDocumentId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "UserId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ApproverId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "IssuerId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "CreatorId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ParentWorkorderId" });
            AlterColumn("iLOGIS.CONFIG_PackageItem", "QtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyUsed", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyDelivered", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyRequested", c => c.Int(nullable: false));
            DropColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            DropColumn("ONEPROD.APS_Calendar", "Efficiency");
            DropColumn("ONEPROD.APS_Calendar", "MaxCycleTime");
            DropColumn("ONEPROD.APS_Calendar", "MaxQty");
            DropColumn("ONEPROD.APS_Calendar", "Hours");
            DropColumn("ONEPROD.APS_Calendar", "Date");
            DropTable("iLOGIS.WHDOC_WhDocumentItem");
            DropTable("iLOGIS.WHDOC_WhDocument");
        }
    }
}
