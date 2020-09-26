namespace _MPPL_WEB_START.Migrations.ElectroluxPLS
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            AddColumn("_MPPL.IDENTITY_UserRole", "Id", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WHDOC_WhDocument", "ApproveDate", c => c.DateTime(nullable: false));
            AlterColumn("iLOGIS.WHDOC_WhDocument", "ContractorId", c => c.Int());
            CreateIndex("iLOGIS.WHDOC_WhDocument", "ContractorId");
            DropColumn("_MPPL.IDENTITY_UserRole", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("_MPPL.IDENTITY_UserRole", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            AlterColumn("iLOGIS.WHDOC_WhDocument", "ContractorId", c => c.Int(nullable: false));
            DropColumn("iLOGIS.WHDOC_WhDocument", "ApproveDate");
            DropColumn("_MPPL.IDENTITY_UserRole", "Id");
            CreateIndex("iLOGIS.WHDOC_WhDocument", "ContractorId");
        }
    }
}
