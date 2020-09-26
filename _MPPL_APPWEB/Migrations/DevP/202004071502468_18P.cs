namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18P : DbMigration
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
                        PrinterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IpAdress, unique: true);
            
            AddColumn("iLOGIS.CONFIG_Warehouse", "LabelLayoutFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropColumn("iLOGIS.CONFIG_Warehouse", "LabelLayoutFileName");
            DropTable("CORE.Printer");
        }
    }
}
