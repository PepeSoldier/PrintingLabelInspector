namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.Calculation", "SetInstructionPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("PFEP.Calculation", "SetInstructionPrice");
        }
    }
}
