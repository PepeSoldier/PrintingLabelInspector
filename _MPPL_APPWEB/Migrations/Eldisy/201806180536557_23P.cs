namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23P : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "_MPPL.Calculation", newSchema: "PFEP");
        }
        
        public override void Down()
        {
            MoveTable(name: "PFEP.Calculation", newSchema: "_MPPL");
        }
    }
}
