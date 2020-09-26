namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.OEE_ReasonType", "NameEnglish", c => c.String(maxLength: 100));
            Sql("UPDATE [MPPL_WRP].[ONEPROD].[OEE_ReasonType] SET [NameEnglish] = [Name]");
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.OEE_ReasonType", "NameEnglish");
        }
    }
}
