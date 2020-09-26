namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Country = c.String(),
                        Language = c.String(),
                        NIP = c.String(),
                        ContactPersonName = c.String(),
                        ContactPhoneNumber = c.String(),
                        ContactEmail = c.String(),
                        ContactAdress = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("ONEPROD.CORE_ClientOrder", "ClientId");
            AddForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client");
            DropIndex("ONEPROD.CORE_ClientOrder", new[] { "ClientId" });
            DropTable("_MPPL.MASTERDATA_Client");
        }
    }
}
