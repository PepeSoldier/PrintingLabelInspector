namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _161K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.APS_Calendar", "Date", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Hours", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxCycleTime", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Efficiency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.APS_Calendar", "Efficiency");
            DropColumn("ONEPROD.APS_Calendar", "MaxCycleTime");
            DropColumn("ONEPROD.APS_Calendar", "MaxQty");
            DropColumn("ONEPROD.APS_Calendar", "Hours");
            DropColumn("ONEPROD.APS_Calendar", "Date");
        }
    }
}
