namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4K : DbMigration
    {
        public override void Up()
        {
            AddColumn("CORE.ChangeLog", "ObjectDescription", c => c.String(maxLength: 100));
            AddColumn("CORE.ChangeLog", "ParentObjectName", c => c.String(maxLength: 70));
            AddColumn("CORE.ChangeLog", "ParentObjectDescription", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("CORE.ChangeLog", "ParentObjectDescription");
            DropColumn("CORE.ChangeLog", "ParentObjectName");
            DropColumn("CORE.ChangeLog", "ObjectDescription");
        }
    }
}
