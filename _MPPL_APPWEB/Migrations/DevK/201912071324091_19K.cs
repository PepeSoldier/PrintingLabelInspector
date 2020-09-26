namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "OTHER.PICKBYLIGHT_InstanceElement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ItemCode = c.String(),
                        PLCMemoryAdress = c.String(),
                        Value = c.Boolean(nullable: false),
                        LastChange = c.DateTime(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "OTHER.PICKBYLIGHT_Instance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PLCDriverIPAdress = c.String(),
                        TCPPort = c.String(),
                        LastChange = c.DateTime(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("OTHER.PICKBYLIGHT_Instance");
            DropTable("OTHER.PICKBYLIGHT_InstanceElement");
        }
    }
}
