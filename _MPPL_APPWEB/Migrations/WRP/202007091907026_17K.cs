namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.CORE_CycleTime", "ProgramName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.CORE_CycleTime", "ProgramName");
        }
    }
}
