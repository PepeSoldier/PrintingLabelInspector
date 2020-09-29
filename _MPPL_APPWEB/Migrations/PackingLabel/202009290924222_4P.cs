namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4P : DbMigration
    {
        public override void Up()
        {
            DropIndex("_MPPL.MASTERDATA_PackingLabel", new[] { "PncId" });
            AddColumn("_MPPL.MASTERDATA_PackingLabel", "ItemCode", c => c.String(maxLength: 50));
            AddColumn("_MPPL.MASTERDATA_PackingLabel", "ItemName", c => c.String(maxLength: 50));
            AlterColumn("_MPPL.MASTERDATA_PackingLabel", "PncId", c => c.Int());
            CreateIndex("_MPPL.MASTERDATA_PackingLabel", "PncId");
        }
        
        public override void Down()
        {
            DropIndex("_MPPL.MASTERDATA_PackingLabel", new[] { "PncId" });
            AlterColumn("_MPPL.MASTERDATA_PackingLabel", "PncId", c => c.Int(nullable: false));
            DropColumn("_MPPL.MASTERDATA_PackingLabel", "ItemName");
            DropColumn("_MPPL.MASTERDATA_PackingLabel", "ItemCode");
            CreateIndex("_MPPL.MASTERDATA_PackingLabel", "PncId");
        }
    }
}
