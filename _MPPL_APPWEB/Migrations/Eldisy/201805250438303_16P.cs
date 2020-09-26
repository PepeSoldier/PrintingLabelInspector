namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.Calculation", "InstrProfileCode", c => c.String());
            AddColumn("_MPPL.Calculation", "InstrName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.Calculation", "InstrName");
            DropColumn("_MPPL.Calculation", "InstrProfileCode");
        }
    }
}
