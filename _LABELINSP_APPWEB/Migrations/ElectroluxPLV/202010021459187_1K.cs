namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LABELINSP.ItemData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemCode = c.String(maxLength: 50),
                        ItemVersion = c.String(maxLength: 50),
                        ExpectedName = c.String(maxLength: 50),
                        ExpectedProductCode = c.String(maxLength: 50),
                        ExpectedWeightKG = c.String(maxLength: 50),
                        ExpectedWeightLBS = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LABELINSP.WorkorderLabel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(maxLength: 10),
                        ItemCode = c.String(maxLength: 50),
                        ItemName = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LABELINSP.WorkorderLabelInspection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackingLabelId = c.Int(nullable: false),
                        TestName = c.String(maxLength: 50),
                        ExpectedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LabelType = c.Int(nullable: false),
                        Result = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("LABELINSP.WorkorderLabel", t => t.PackingLabelId)
                .Index(t => t.PackingLabelId);
            
            CreateTable(
                "CORE.Printer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        User = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        IpAdress = c.String(nullable: false, maxLength: 50),
                        Model = c.String(maxLength: 150),
                        SerialNumber = c.String(maxLength: 150),
                        PrinterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IpAdress, unique: true);
            
            CreateTable(
                "_LABELINSP.IDENTITY_Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "_LABELINSP.IDENTITY_UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("_LABELINSP.IDENTITY_Role", t => t.RoleId)
                .ForeignKey("_LABELINSP.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "CORE.SystemVariables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Value = c.String(),
                        Type = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_LABELINSP.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_LABELINSP.IDENTITY_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DepartmentId = c.Int(),
                        LastPasswordChangedDate = c.DateTime(nullable: false),
                        SuperVisorUserId = c.String(maxLength: 128),
                        SupervisorUserName = c.String(),
                        Title = c.String(maxLength: 50),
                        Factory = c.String(maxLength: 25),
                        Deleted = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_LABELINSP.MASTERDATA_Department", t => t.DepartmentId)
                .ForeignKey("_LABELINSP.IDENTITY_User", t => t.SuperVisorUserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SuperVisorUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "_LABELINSP.IDENTITY_UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_LABELINSP.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_LABELINSP.MASTERDATA_Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_LABELINSP.IDENTITY_UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("_LABELINSP.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "LABELINSP.Workorder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkorderNumber = c.String(maxLength: 50),
                        ItemCode = c.String(maxLength: 50),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SerialNumberFrom = c.String(maxLength: 50),
                        SerialNumberTo = c.String(maxLength: 50),
                        FirstInspectionDate = c.DateTime(nullable: false),
                        LastInspectionDate = c.DateTime(nullable: false),
                        SuccessfullInspections = c.Int(nullable: false),
                        FailfullInspections = c.Int(nullable: false),
                        FailInspectionLabelPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("CORE.SystemVariables", "UserId", "_LABELINSP.IDENTITY_User");
            DropForeignKey("_LABELINSP.IDENTITY_User", "SuperVisorUserId", "_LABELINSP.IDENTITY_User");
            DropForeignKey("_LABELINSP.IDENTITY_UserRole", "UserId", "_LABELINSP.IDENTITY_User");
            DropForeignKey("_LABELINSP.IDENTITY_UserLogin", "UserId", "_LABELINSP.IDENTITY_User");
            DropForeignKey("_LABELINSP.IDENTITY_User", "DepartmentId", "_LABELINSP.MASTERDATA_Department");
            DropForeignKey("_LABELINSP.IDENTITY_UserClaim", "UserId", "_LABELINSP.IDENTITY_User");
            DropForeignKey("_LABELINSP.IDENTITY_UserRole", "RoleId", "_LABELINSP.IDENTITY_Role");
            DropForeignKey("LABELINSP.WorkorderLabelInspection", "PackingLabelId", "LABELINSP.WorkorderLabel");
            DropIndex("_LABELINSP.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_LABELINSP.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_LABELINSP.IDENTITY_User", "UserNameIndex");
            DropIndex("_LABELINSP.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_LABELINSP.IDENTITY_User", new[] { "DepartmentId" });
            DropIndex("CORE.SystemVariables", new[] { "UserId" });
            DropIndex("_LABELINSP.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_LABELINSP.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_LABELINSP.IDENTITY_Role", "RoleNameIndex");
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("LABELINSP.WorkorderLabelInspection", new[] { "PackingLabelId" });
            DropTable("LABELINSP.Workorder");
            DropTable("_LABELINSP.IDENTITY_UserLogin");
            DropTable("_LABELINSP.MASTERDATA_Department");
            DropTable("_LABELINSP.IDENTITY_UserClaim");
            DropTable("_LABELINSP.IDENTITY_User");
            DropTable("CORE.SystemVariables");
            DropTable("_LABELINSP.IDENTITY_UserRole");
            DropTable("_LABELINSP.IDENTITY_Role");
            DropTable("CORE.Printer");
            DropTable("LABELINSP.WorkorderLabelInspection");
            DropTable("LABELINSP.WorkorderLabel");
            DropTable("LABELINSP.ItemData");
        }
    }
}
