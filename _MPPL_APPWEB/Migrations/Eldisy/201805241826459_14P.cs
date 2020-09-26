namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.DEF_Package", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("PFEP.DEF_Package", "UnitOfMeasue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PFEP.DEF_Package", "UnitOfMeasue");
            DropColumn("PFEP.DEF_Package", "UnitPrice");
        }
    }
}
