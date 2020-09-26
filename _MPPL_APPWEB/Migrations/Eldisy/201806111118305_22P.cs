namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.DEF_Package", "UnitOfMeasure", c => c.Int(nullable: false));
            DropColumn("PFEP.DEF_Package", "UnitOfMeasue");
        }
        
        public override void Down()
        {
            AddColumn("PFEP.DEF_Package", "UnitOfMeasue", c => c.Int(nullable: false));
            DropColumn("PFEP.DEF_Package", "UnitOfMeasure");
        }
    }
}
