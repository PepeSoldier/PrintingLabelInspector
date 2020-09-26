namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20K : DbMigration
    {
        public override void Up()
        {
            AddColumn("OTHER.PICKBYLIGHT_InstanceElement", "PickByLightInstanceId", c => c.Int(nullable: false));
            CreateIndex("OTHER.PICKBYLIGHT_InstanceElement", "PickByLightInstanceId");
            AddForeignKey("OTHER.PICKBYLIGHT_InstanceElement", "PickByLightInstanceId", "OTHER.PICKBYLIGHT_Instance", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("OTHER.PICKBYLIGHT_InstanceElement", "PickByLightInstanceId", "OTHER.PICKBYLIGHT_Instance");
            DropIndex("OTHER.PICKBYLIGHT_InstanceElement", new[] { "PickByLightInstanceId" });
            DropColumn("OTHER.PICKBYLIGHT_InstanceElement", "PickByLightInstanceId");
        }
    }
}
