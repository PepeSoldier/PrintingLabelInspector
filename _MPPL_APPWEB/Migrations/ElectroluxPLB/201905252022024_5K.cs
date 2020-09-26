namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.SystemVariables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Value = c.String(),
                        Type = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("CORE.SystemVariables", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("CORE.SystemVariables", new[] { "UserId" });
            DropTable("CORE.SystemVariables");
        }
    }
}
