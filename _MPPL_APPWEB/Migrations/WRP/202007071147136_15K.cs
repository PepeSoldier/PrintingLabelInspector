namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.BOM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(),
                        PncId = c.Int(),
                        LV = c.Int(nullable: false),
                        PCS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        BC = c.String(maxLength: 3),
                        DEF = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 12),
                        Suffix = c.String(maxLength: 12),
                        IDCO = c.String(maxLength: 12),
                        Task = c.String(maxLength: 50),
                        TaskForce = c.String(maxLength: 50),
                        Formula = c.String(maxLength: 50),
                        Condition = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.AncId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.PncId)
                .Index(t => t.AncId)
                .Index(t => t.PncId);
            
            CreateTable(
                "CORE.BOM_Workorder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(maxLength: 20),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                        BC1 = c.String(maxLength: 3),
                        LV = c.Int(nullable: false),
                        QtyUsed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BC2 = c.String(maxLength: 3),
                        DEF = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 12),
                        Suffix = c.String(maxLength: 12),
                        IDCO = c.String(maxLength: 12),
                        DirPar = c.String(maxLength: 20),
                        UnitOfMeasure = c.Int(nullable: false),
                        InterItm = c.String(),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ChildId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("CORE.BOM_Workorder", "ParentId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM_Workorder", "ChildId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "AncId", "_MPPL.MASTERDATA_Item");
            DropIndex("CORE.BOM_Workorder", new[] { "ChildId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ParentId" });
            DropIndex("CORE.BOM", new[] { "PncId" });
            DropIndex("CORE.BOM", new[] { "AncId" });
            DropTable("CORE.BOM_Workorder");
            DropTable("CORE.BOM");
        }
    }
}
