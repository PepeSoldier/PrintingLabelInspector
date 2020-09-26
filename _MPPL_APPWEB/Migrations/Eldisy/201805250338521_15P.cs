namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.Calculation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackingInstructionId = c.Int(nullable: false),
                        PackingInstructionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedInstructionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PFEP.PackingInstruction", t => t.PackingInstructionId)
                .Index(t => t.PackingInstructionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.Calculation", "PackingInstructionId", "PFEP.PackingInstruction");
            DropIndex("_MPPL.Calculation", new[] { "PackingInstructionId" });
            DropTable("_MPPL.Calculation");
        }
    }
}
