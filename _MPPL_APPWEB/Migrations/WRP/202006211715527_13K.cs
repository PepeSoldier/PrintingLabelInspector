namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ONEPROD.OEE_Reason", "GroupId", c => c.Int());
            Sql("UPDATE ONEPROD.OEE_Reason SET GroupId = NULL WHERE GroupId = 0");
            CreateIndex("ONEPROD.OEE_Reason", "GroupId");
            AddForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason");
            DropIndex("ONEPROD.OEE_Reason", new[] { "GroupId" });
            AlterColumn("ONEPROD.OEE_Reason", "GroupId", c => c.Int(nullable: false));
        }
    }
}
