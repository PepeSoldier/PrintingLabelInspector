namespace _MPPL_WEB_START.Migrations.Grandhome
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
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
                "_MPPL.MASTERDATA_Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Country = c.String(),
                        Language = c.String(),
                        NIP = c.String(),
                        ContactPersonName = c.String(),
                        ContactPhoneNumber = c.String(),
                        ContactEmail = c.String(),
                        ContactAdress = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("ONEPROD.CORE_ClientOrder", "ClientId");
            AddForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client");
            DropForeignKey("CORE.BOM", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "AncId", "_MPPL.MASTERDATA_Item");
            DropIndex("ONEPROD.CORE_ClientOrder", new[] { "ClientId" });
            DropIndex("CORE.BOM", new[] { "PncId" });
            DropIndex("CORE.BOM", new[] { "AncId" });
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("CORE.BOM");
        }
    }
}
