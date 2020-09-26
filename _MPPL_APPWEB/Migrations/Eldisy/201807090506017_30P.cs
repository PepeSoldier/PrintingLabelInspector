namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.PackingInstruction_Item", "PriceForHundredPackages", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("PFEP.PackingInstruction_Item", "PriceForHundredPackages");
        }
    }
}
