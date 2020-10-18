namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7K : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("_LABELINSP.IDENTITY_User", "DepartmentId", "_LABELINSP.MASTERDATA_Department");
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("_LABELINSP.IDENTITY_User", new[] { "DepartmentId" });
            DropColumn("_LABELINSP.IDENTITY_User", "DepartmentId");
            DropTable("CORE.Printer");
            DropTable("_LABELINSP.MASTERDATA_Department");
        }
        
        public override void Down()
        {
            CreateTable(
                "_LABELINSP.MASTERDATA_Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("_LABELINSP.IDENTITY_User", "DepartmentId", c => c.Int());
            CreateIndex("_LABELINSP.IDENTITY_User", "DepartmentId");
            CreateIndex("CORE.Printer", "IpAdress", unique: true);
            AddForeignKey("_LABELINSP.IDENTITY_User", "DepartmentId", "_LABELINSP.MASTERDATA_Department", "Id");
        }
    }
}
