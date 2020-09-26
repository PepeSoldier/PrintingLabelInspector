namespace _MPPL_WEB_START.Migrations.ElectroluxPLS
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate");
        }
    }
}
