namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13P : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "_MPPL.Correction", name: "Applicant_Id", newName: "ApplicantId");
            RenameIndex(table: "_MPPL.Correction", name: "IX_Applicant_Id", newName: "IX_ApplicantId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "_MPPL.Correction", name: "IX_ApplicantId", newName: "IX_Applicant_Id");
            RenameColumn(table: "_MPPL.Correction", name: "ApplicantId", newName: "Applicant_Id");
        }
    }
}
