namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.BASE_Attachment", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.BASE_Attachment", "Discriminator");
        }
    }
}
