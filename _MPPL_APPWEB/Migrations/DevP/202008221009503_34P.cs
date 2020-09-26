namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _34P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WHDOC_WhDocument", "Notice", c => c.String(maxLength: 128));
            AddColumn("iLOGIS.WHDOC_WhDocument", "TruckPlateNumbers", c => c.String(maxLength: 128));
            AddColumn("iLOGIS.WHDOC_WhDocument", "TrailerPlateNumbers", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WHDOC_WhDocument", "TrailerPlateNumbers");
            DropColumn("iLOGIS.WHDOC_WhDocument", "TruckPlateNumbers");
            DropColumn("iLOGIS.WHDOC_WhDocument", "Notice");
        }
    }
}
