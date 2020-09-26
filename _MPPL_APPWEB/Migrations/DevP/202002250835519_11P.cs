namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11P : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.Delivery", new[] { "SupplierId" });
            AddColumn("iLOGIS.Delivery", "StampTime", c => c.DateTime(nullable: false));
            AlterColumn("iLOGIS.Delivery", "SupplierId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.Delivery", "SupplierId");
            DropColumn("iLOGIS.Delivery", "DespatchTime");
            DropColumn("iLOGIS.Delivery", "ReferenceNumber");
            DropColumn("iLOGIS.Delivery", "ReferenceDate");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.Delivery", "ReferenceDate", c => c.DateTime());
            AddColumn("iLOGIS.Delivery", "ReferenceNumber", c => c.String());
            AddColumn("iLOGIS.Delivery", "DespatchTime", c => c.DateTime());
            DropIndex("iLOGIS.Delivery", new[] { "SupplierId" });
            AlterColumn("iLOGIS.Delivery", "SupplierId", c => c.Int());
            DropColumn("iLOGIS.Delivery", "StampTime");
            CreateIndex("iLOGIS.Delivery", "SupplierId");
        }
    }
}
