namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7K : DbMigration
    {
        public override void Up()
        {
            DropIndex("ONEPROD.MES_Workplace", new[] { "MachineId" });
            AddColumn("ONEPROD.MES_ProductionLog", "ItemId", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLog", "ClientWorkOrderNumber", c => c.String(maxLength: 20));
            AddColumn("ONEPROD.MES_ProductionLog", "WorkorderTotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.MES_ProductionLog", "SerialNo", c => c.String(maxLength: 25));
            AlterColumn("ONEPROD.MES_Workplace", "MachineId", c => c.Int(nullable: false));
            CreateIndex("ONEPROD.MES_ProductionLog", "ItemId");
            CreateIndex("ONEPROD.MES_Workplace", "MachineId");

            Sql("UPDATE t " +
                "SET t.ClientWorkOrderNumber = t.WorkOrderNumber, " +
                "t.WorkorderTotalQty = t.TaskTotalQty, " +
                "t.SerialNo = t.BatchSerialNo, " +
                "t.ItemId = i.Id " +
                "FROM [ONEPROD].[MES_ProductionLog] t " +
                "LEFT JOIN [_MPPL].[MASTERDATA_Item] i ON i.Code = t.PartCode");

            AddForeignKey("ONEPROD.MES_ProductionLog", "ItemId", "ONEPROD.CORE_Item", "Id");

            DropColumn("ONEPROD.MES_ProductionLog", "WorkOrderNumber");
            DropColumn("ONEPROD.MES_ProductionLog", "PartCode");
            DropColumn("ONEPROD.MES_ProductionLog", "BatchSerialNo");
            DropColumn("ONEPROD.MES_ProductionLog", "TaskTotalQty");
        }
        
        public override void Down()
        {
            AddColumn("ONEPROD.MES_ProductionLog", "TaskTotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.MES_ProductionLog", "BatchSerialNo", c => c.String());
            AddColumn("ONEPROD.MES_ProductionLog", "PartCode", c => c.String(maxLength: 50));
            AddColumn("ONEPROD.MES_ProductionLog", "WorkOrderNumber", c => c.String(maxLength: 20));
            DropForeignKey("ONEPROD.MES_ProductionLog", "ItemId", "ONEPROD.CORE_Item");
            DropIndex("ONEPROD.MES_Workplace", new[] { "MachineId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "ItemId" });
            AlterColumn("ONEPROD.MES_Workplace", "MachineId", c => c.Int());
            DropColumn("ONEPROD.MES_ProductionLog", "SerialNo");
            DropColumn("ONEPROD.MES_ProductionLog", "WorkorderTotalQty");
            DropColumn("ONEPROD.MES_ProductionLog", "ClientWorkOrderNumber");
            DropColumn("ONEPROD.MES_ProductionLog", "ItemId");
            CreateIndex("ONEPROD.MES_Workplace", "MachineId");
        }
    }
}
