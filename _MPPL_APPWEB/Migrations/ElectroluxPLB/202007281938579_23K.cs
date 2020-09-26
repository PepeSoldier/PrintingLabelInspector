namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_Movement", "FreeText1", c => c.String(maxLength: 200));
            AddColumn("iLOGIS.WMS_Movement", "FreeText2", c => c.String(maxLength: 200));
            AlterColumn("iLOGIS.WMS_Movement", "ExternalId", c => c.String(maxLength: 150));
            AlterColumn("iLOGIS.WMS_Movement", "ExternalUserName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.WMS_Movement", "ExternalUserName", c => c.String());
            AlterColumn("iLOGIS.WMS_Movement", "ExternalId", c => c.String());
            DropColumn("iLOGIS.WMS_Movement", "FreeText2");
            DropColumn("iLOGIS.WMS_Movement", "FreeText1");
        }
    }
}
