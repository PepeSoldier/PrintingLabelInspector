namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.Correction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackingInstructionId = c.Int(nullable: false),
                        ApplicationStart = c.DateTime(nullable: false),
                        Applicationfinished = c.DateTime(nullable: false),
                        Finished = c.Boolean(nullable: false),
                        CorrectionText = c.String(),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.Applicant_Id)
                .ForeignKey("PFEP.PackingInstruction", t => t.PackingInstructionId)
                .Index(t => t.PackingInstructionId)
                .Index(t => t.Applicant_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.Correction", "PackingInstructionId", "PFEP.PackingInstruction");
            DropForeignKey("_MPPL.Correction", "Applicant_Id", "_MPPL.IDENTITY_User");
            DropIndex("_MPPL.Correction", new[] { "Applicant_Id" });
            DropIndex("_MPPL.Correction", new[] { "PackingInstructionId" });
            DropTable("_MPPL.Correction");
        }
    }
}
