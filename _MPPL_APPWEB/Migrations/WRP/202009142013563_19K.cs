﻿namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_ItemUoM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        DefaultUnitOfMeasure = c.Int(nullable: false),
                        QtyForDefaultUnitOfMeasure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AlternativeUnitOfMeasure = c.Int(nullable: false),
                        QtyForAlternativeUnitOfMeasure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.MASTERDATA_ItemUoM", "ItemId", "_MPPL.MASTERDATA_Item");
            DropIndex("_MPPL.MASTERDATA_ItemUoM", new[] { "ItemId" });
            DropTable("_MPPL.MASTERDATA_ItemUoM");
        }
    }
}