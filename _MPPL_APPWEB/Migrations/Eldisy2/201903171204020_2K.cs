namespace _MPPL_WEB_START.Migrations.Eldisy2
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.PREPROD_ClientOrder", "ClientId", c => c.Int());
            AddColumn("ONEPROD.PREPROD_ClientOrder", "InsertDate", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.PREPROD_ClientOrder", "LastUpdateDate", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.PREPROD_ClientOrder", "Deleted", c => c.Boolean(nullable: false));
            CreateIndex("ONEPROD.PREPROD_ClientOrder", "ClientId");
            AddForeignKey("ONEPROD.PREPROD_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.PREPROD_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client");
            DropIndex("ONEPROD.PREPROD_ClientOrder", new[] { "ClientId" });
            DropColumn("ONEPROD.PREPROD_ClientOrder", "Deleted");
            DropColumn("ONEPROD.PREPROD_ClientOrder", "LastUpdateDate");
            DropColumn("ONEPROD.PREPROD_ClientOrder", "InsertDate");
            DropColumn("ONEPROD.PREPROD_ClientOrder", "ClientId");
        }
    }
}
