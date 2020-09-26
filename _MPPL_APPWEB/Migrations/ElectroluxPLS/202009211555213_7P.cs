namespace _MPPL_WEB_START.Migrations.ElectroluxPLS
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WHDOC_WhDocument", "SecurityApproverId", c => c.String(maxLength: 128));
            CreateIndex("iLOGIS.WHDOC_WhDocument", "SecurityApproverId");
            AddForeignKey("iLOGIS.WHDOC_WhDocument", "SecurityApproverId", "_MPPL.IDENTITY_User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "SecurityApproverId", "_MPPL.IDENTITY_User");
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "SecurityApproverId" });
            DropColumn("iLOGIS.WHDOC_WhDocument", "SecurityApproverId");
        }
    }
}
