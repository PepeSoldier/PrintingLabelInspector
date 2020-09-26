namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.AccCross", newName: "HSE_SafetyCross");
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("_MPPL.MASTERDATA_LabourBrigade");
            DropTable("_MPPL.MASTERDATA_Workstation");
        }
        
        public override void Down()
        {
            CreateTable(
                "_MPPL.MASTERDATA_Workstation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        SortOrder = c.Int(nullable: false),
                        LineId = c.Int(),
                        AreaId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_LabourBrigade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Country = c.String(),
                        Language = c.String(),
                        NIP = c.String(),
                        ContactPersonName = c.String(),
                        ContactPhoneNumber = c.String(),
                        ContactEmail = c.String(),
                        ContactAdress = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            RenameTable(name: "_MPPL.HSE_SafetyCross", newName: "AccCross");
        }
    }
}
