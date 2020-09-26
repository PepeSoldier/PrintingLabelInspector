namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.Calculation", "ClientProfileCode", c => c.String());
            AddColumn("PFEP.Calculation", "ProfileCode", c => c.String());
            AddColumn("PFEP.Calculation", "ProfileName", c => c.String());
            DropColumn("PFEP.Calculation", "InstrProfileCode");
            DropColumn("PFEP.Calculation", "InstrName");
        }
        
        public override void Down()
        {
            AddColumn("PFEP.Calculation", "InstrName", c => c.String());
            AddColumn("PFEP.Calculation", "InstrProfileCode", c => c.String());
            DropColumn("PFEP.Calculation", "ProfileName");
            DropColumn("PFEP.Calculation", "ProfileCode");
            DropColumn("PFEP.Calculation", "ClientProfileCode");
        }
    }
}
