namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _29P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WHDOC_WhDocument", "ApproveDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WHDOC_WhDocument", "ApproveDate");
        }
    }
}
