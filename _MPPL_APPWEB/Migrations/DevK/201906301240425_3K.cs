namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.MES_Workplace", "PrintLabel", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "SerialNumberType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "PrinterType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "LabelLayoutNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.MES_Workplace", "LabelLayoutNo");
            DropColumn("ONEPROD.MES_Workplace", "PrinterType");
            DropColumn("ONEPROD.MES_Workplace", "SerialNumberType");
            DropColumn("ONEPROD.MES_Workplace", "PrintLabel");
        }
    }
}
