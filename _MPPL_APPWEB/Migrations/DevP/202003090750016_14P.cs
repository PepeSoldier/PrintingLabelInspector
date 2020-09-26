namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.DeliveryItem", "PackageQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.DeliveryItem", "PackageQty");
        }
    }
}
