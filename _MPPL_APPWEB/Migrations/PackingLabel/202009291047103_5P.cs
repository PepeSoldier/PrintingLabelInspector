namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5P : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("_MPPL.MASTERDATA_PackingLabel", "PncId", "_MPPL.MASTERDATA_Item");
            DropIndex("_MPPL.MASTERDATA_PackingLabel", new[] { "PncId" });
            DropColumn("_MPPL.MASTERDATA_PackingLabel", "PncId");
        }
        
        public override void Down()
        {
            AddColumn("_MPPL.MASTERDATA_PackingLabel", "PncId", c => c.Int());
            CreateIndex("_MPPL.MASTERDATA_PackingLabel", "PncId");
            AddForeignKey("_MPPL.MASTERDATA_PackingLabel", "PncId", "_MPPL.MASTERDATA_Item", "Id");
        }
    }
}
