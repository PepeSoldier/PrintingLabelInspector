namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _160K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId", c => c.Int());
            CreateIndex("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            AddForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ParentWorkorderId" });
            DropColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId");
        }
    }
}
