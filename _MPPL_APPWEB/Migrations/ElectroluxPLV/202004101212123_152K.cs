namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _152K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.Printer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        User = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        IpAdress = c.String(nullable: false, maxLength: 50),
                        Model = c.String(maxLength: 150),
                        SerialNumber = c.String(maxLength: 150),
                        WorkOrderNumber = c.String(maxLength: 150),
                        PrinterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IpAdress, unique: true);
            
            AddColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "WarehouseType", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "LabelLayoutFileName", c => c.String());
            CreateIndex("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");

            Sql("UPDATE iLOGIS.CONFIG_Warehouse SET AccountingWarehouseId = 1, WarehouseType = 0");

            AddForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropColumn("iLOGIS.CONFIG_Warehouse", "LabelLayoutFileName");
            DropColumn("iLOGIS.CONFIG_Warehouse", "WarehouseType");
            DropColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
            DropTable("CORE.Printer");
        }
    }
}
