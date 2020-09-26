namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.NotificationDevice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PushEndpoint = c.String(),
                        PushP256DH = c.String(),
                        PushAuth = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("ONEPROD.APS_Calendar", "Date", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Hours", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxCycleTime", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Efficiency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.ENERGY_EnergyCost", "kWhConverter", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.ENERGY_EnergyCost", "UseConverter", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId", c => c.Int());
            CreateIndex("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            AddForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ParentWorkorderId" });
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            DropColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            DropColumn("ONEPROD.ENERGY_EnergyCost", "UseConverter");
            DropColumn("ONEPROD.ENERGY_EnergyCost", "kWhConverter");
            DropColumn("ONEPROD.APS_Calendar", "Efficiency");
            DropColumn("ONEPROD.APS_Calendar", "MaxCycleTime");
            DropColumn("ONEPROD.APS_Calendar", "MaxQty");
            DropColumn("ONEPROD.APS_Calendar", "Hours");
            DropColumn("ONEPROD.APS_Calendar", "Date");
            DropTable("CORE.NotificationDevice");
        }
    }
}
