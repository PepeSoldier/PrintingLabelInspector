namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17P : DbMigration
    {
        public override void Up()
        {
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ParentId" });
            AddColumn("PFEP.PackingInstruction", "TempPhoto1", c => c.String(maxLength: 64));
            AddColumn("PFEP.PackingInstruction", "TempPhoto2", c => c.String(maxLength: 64));
            AddColumn("PFEP.PackingInstruction", "TempPhoto3", c => c.String(maxLength: 64));
            AddColumn("PFEP.PackingInstruction", "TempPhoto4", c => c.String(maxLength: 64));
            AddColumn("PFEP.PackingInstruction", "TmpConfirmName", c => c.String());
            AddColumn("PFEP.PackingInstruction", "TmpExaminerName", c => c.String());
            AddColumn("PFEP.PackingInstruction", "TmpCreatorName", c => c.String());
            AlterColumn("_MPPL.BASE_Attachment", "Name", c => c.String(maxLength: 250));
            AlterColumn("PFEP.PackingInstruction_Item", "Amount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("_MPPL.MASTERDATA_Resource", "ParentId", c => c.Int());
            CreateIndex("_MPPL.MASTERDATA_Resource", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ParentId" });
            AlterColumn("_MPPL.MASTERDATA_Resource", "ParentId", c => c.Int(nullable: false));
            AlterColumn("PFEP.PackingInstruction_Item", "Amount", c => c.Int());
            AlterColumn("_MPPL.BASE_Attachment", "Name", c => c.String(maxLength: 50));
            DropColumn("PFEP.PackingInstruction", "TmpCreatorName");
            DropColumn("PFEP.PackingInstruction", "TmpExaminerName");
            DropColumn("PFEP.PackingInstruction", "TmpConfirmName");
            DropColumn("PFEP.PackingInstruction", "TempPhoto4");
            DropColumn("PFEP.PackingInstruction", "TempPhoto3");
            DropColumn("PFEP.PackingInstruction", "TempPhoto2");
            DropColumn("PFEP.PackingInstruction", "TempPhoto1");
            CreateIndex("_MPPL.MASTERDATA_Resource", "ParentId");
        }
    }
}
