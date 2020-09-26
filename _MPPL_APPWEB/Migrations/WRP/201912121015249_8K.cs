namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ONEPROD.ENERGY_EnergyConsumptionData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnergyMeterId = c.Int(nullable: false),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        ImportDate = c.DateTime(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalValue = c.Decimal(precision: 18, scale: 2),
                        ProductionQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalStopTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.ENERGY_EnergyMeter", t => t.EnergyMeterId)
                .Index(t => t.EnergyMeterId);
            
            CreateTable(
                "ONEPROD.ENERGY_EnergyMeter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MarkedName = c.String(nullable: false),
                        Description = c.String(),
                        ResourceId = c.Int(),
                        EnergyType = c.Int(nullable: false),
                        UnitOfMeasure = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceId)
                .Index(t => t.ResourceId);
            
            CreateTable(
                "ONEPROD.ENERGY_EnergyCost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        EnergyType = c.Int(nullable: false),
                        UnitOfMeasure = c.Int(nullable: false),
                        PricePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.ENERGY_EnergyConsumptionData", "EnergyMeterId", "ONEPROD.ENERGY_EnergyMeter");
            DropForeignKey("ONEPROD.ENERGY_EnergyMeter", "ResourceId", "ONEPROD.CORE_Resource");
            DropIndex("ONEPROD.ENERGY_EnergyMeter", new[] { "ResourceId" });
            DropIndex("ONEPROD.ENERGY_EnergyConsumptionData", new[] { "EnergyMeterId" });
            DropTable("ONEPROD.ENERGY_EnergyCost");
            DropTable("ONEPROD.ENERGY_EnergyMeter");
            DropTable("ONEPROD.ENERGY_EnergyConsumptionData");
        }
    }
}