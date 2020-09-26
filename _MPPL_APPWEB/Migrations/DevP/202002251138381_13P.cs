namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.DeliveryItem", "Quantity", c => c.Int(nullable: false));
            DropColumn("iLOGIS.DeliveryItem", "DespatchedQuantity");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.DeliveryItem", "DespatchedQuantity", c => c.Int(nullable: false));
            DropColumn("iLOGIS.DeliveryItem", "Quantity");
        }
    }
}
