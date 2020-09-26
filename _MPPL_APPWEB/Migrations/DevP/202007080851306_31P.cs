namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.IDENTITY_UserRole", "Id", c => c.Int(nullable: false));
            DropColumn("_MPPL.IDENTITY_UserRole", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("_MPPL.IDENTITY_UserRole", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("_MPPL.IDENTITY_UserRole", "Id");
        }
    }
}
