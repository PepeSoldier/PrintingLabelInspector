namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10P : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.Delivery", new[] { "SupplierId" });
            AlterColumn("iLOGIS.Delivery", "SupplierId", c => c.Int());
            CreateIndex("iLOGIS.Delivery", "SupplierId");
        }
        
        public override void Down()
        {
            DropIndex("iLOGIS.Delivery", new[] { "SupplierId" });
            AlterColumn("iLOGIS.Delivery", "SupplierId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.Delivery", "SupplierId");
        }
    }
}
