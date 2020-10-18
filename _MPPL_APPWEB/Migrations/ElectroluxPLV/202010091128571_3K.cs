namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            AddColumn("LABELINSP.WorkorderLabel", "WorkorderId", c => c.Int(nullable: false));
            AddColumn("LABELINSP.WorkorderLabelInspection", "TimeStamp", c => c.DateTime(nullable: false));
            AddColumn("LABELINSP.Workorder", "ItemName", c => c.String(maxLength: 50));

            Sql("UPDATE [LABELINSP].[WorkorderLabel] SET WorkorderId = 1");

            CreateIndex("LABELINSP.WorkorderLabel", "WorkorderId");
            AddForeignKey("LABELINSP.WorkorderLabel", "WorkorderId", "LABELINSP.Workorder", "Id");
            DropColumn("LABELINSP.WorkorderLabel", "OrderNo");
            DropColumn("LABELINSP.WorkorderLabel", "ItemCode");
            DropColumn("LABELINSP.WorkorderLabel", "ItemName");
        }
        
        public override void Down()
        {
            AddColumn("LABELINSP.WorkorderLabel", "ItemName", c => c.String(maxLength: 50));
            AddColumn("LABELINSP.WorkorderLabel", "ItemCode", c => c.String(maxLength: 50));
            AddColumn("LABELINSP.WorkorderLabel", "OrderNo", c => c.String(maxLength: 10));
            DropForeignKey("LABELINSP.WorkorderLabel", "WorkorderId", "LABELINSP.Workorder");
            DropIndex("LABELINSP.WorkorderLabel", new[] { "WorkorderId" });
            DropColumn("LABELINSP.Workorder", "ItemName");
            DropColumn("LABELINSP.WorkorderLabelInspection", "TimeStamp");
            DropColumn("LABELINSP.WorkorderLabel", "WorkorderId");
        }
    }
}
