namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_Anc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 9),
                        Name = c.String(nullable: false, maxLength: 100),
                        Name2 = c.String(nullable: false, maxLength: 100),
                        Inactive = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        New = c.Boolean(nullable: false),
                        TypeId = c.Int(),
                        IsCommon = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Area",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.BASE_Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Extension = c.String(maxLength: 10),
                        ParentObjectId = c.Int(nullable: false),
                        ParentType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.BASE_ExtensionFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileExtension = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Line",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        AreaId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "PFEP.DEF_Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PFEP.PackagingIntruction_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(),
                        UnitOfMeasure = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        PackingInstructionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PFEP.DEF_Package", t => t.PackageId)
                .ForeignKey("PFEP.PackingInstruction", t => t.PackingInstructionId)
                .Index(t => t.PackageId)
                .Index(t => t.PackingInstructionId);
            
            CreateTable(
                "PFEP.PackingInstruction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstructionNumber = c.Int(nullable: false),
                        InstructionVersion = c.Int(nullable: false),
                        AmountOnBoxId = c.Int(nullable: false),
                        Description = c.String(),
                        AmountOnLayer = c.Int(nullable: false),
                        AmountOnBox = c.Int(nullable: false),
                        AmountOnPallet = c.Int(nullable: false),
                        UnitOfMeasure = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Examined = c.Boolean(nullable: false),
                        ExaminedDate = c.DateTime(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                        ConfirmedDate = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        ExamineId = c.String(),
                        ConfirmId = c.String(maxLength: 128),
                        AreaId = c.Int(nullable: false),
                        ProfileCode = c.String(nullable: false, maxLength: 100),
                        ProfileName = c.String(nullable: false, maxLength: 100),
                        ClientName = c.String(),
                        ClientProfileCode = c.String(),
                        Examiner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.ID_User", t => t.ConfirmId)
                .ForeignKey("_MPPL.ID_User", t => t.CreatorId)
                .ForeignKey("_MPPL.ID_User", t => t.Examiner_Id)
                .Index(t => t.CreatorId)
                .Index(t => t.ConfirmId)
                .Index(t => t.AreaId)
                .Index(t => t.Examiner_Id);
            
            CreateTable(
                "_MPPL.ID_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DepartmentId = c.Int(),
                        SuperVisorUserId = c.String(maxLength: 128),
                        SupervisorUserName = c.String(),
                        Title = c.String(maxLength: 50),
                        Factory = c.String(maxLength: 25),
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
                .ForeignKey("_MPPL.MASTERDATA_Department", t => t.DepartmentId)
                .ForeignKey("_MPPL.ID_User", t => t.SuperVisorUserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SuperVisorUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "_MPPL.ID_UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.ID_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.ID_UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("_MPPL.ID_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.ID_UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("_MPPL.ID_User", t => t.UserId)
                .ForeignKey("_MPPL.ID_Role", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Pnc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.ID_Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
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
                        TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.MASTERDATA_Line", t => t.LineId)
                .Index(t => t.LineId)
                .Index(t => t.AreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "LineId", "_MPPL.MASTERDATA_Line");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("_MPPL.ID_UserRole", "RoleId", "_MPPL.ID_Role");
            DropForeignKey("PFEP.PackagingIntruction_Item", "PackingInstructionId", "PFEP.PackingInstruction");
            DropForeignKey("PFEP.PackingInstruction", "Examiner_Id", "_MPPL.ID_User");
            DropForeignKey("PFEP.PackingInstruction", "CreatorId", "_MPPL.ID_User");
            DropForeignKey("PFEP.PackingInstruction", "ConfirmId", "_MPPL.ID_User");
            DropForeignKey("_MPPL.ID_User", "SuperVisorUserId", "_MPPL.ID_User");
            DropForeignKey("_MPPL.ID_UserRole", "UserId", "_MPPL.ID_User");
            DropForeignKey("_MPPL.ID_UserLogin", "UserId", "_MPPL.ID_User");
            DropForeignKey("_MPPL.ID_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("_MPPL.ID_UserClaim", "UserId", "_MPPL.ID_User");
            DropForeignKey("PFEP.PackingInstruction", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("PFEP.PackagingIntruction_Item", "PackageId", "PFEP.DEF_Package");
            DropForeignKey("_MPPL.MASTERDATA_Line", "AreaId", "_MPPL.MASTERDATA_Area");
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "LineId" });
            DropIndex("_MPPL.ID_Role", "RoleNameIndex");
            DropIndex("_MPPL.ID_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.ID_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.ID_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.ID_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.ID_User", "UserNameIndex");
            DropIndex("_MPPL.ID_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.ID_User", new[] { "DepartmentId" });
            DropIndex("PFEP.PackingInstruction", new[] { "Examiner_Id" });
            DropIndex("PFEP.PackingInstruction", new[] { "AreaId" });
            DropIndex("PFEP.PackingInstruction", new[] { "ConfirmId" });
            DropIndex("PFEP.PackingInstruction", new[] { "CreatorId" });
            DropIndex("PFEP.PackagingIntruction_Item", new[] { "PackingInstructionId" });
            DropIndex("PFEP.PackagingIntruction_Item", new[] { "PackageId" });
            DropIndex("_MPPL.MASTERDATA_Line", new[] { "AreaId" });
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("_MPPL.ID_Role");
            DropTable("_MPPL.MASTERDATA_Pnc");
            DropTable("_MPPL.ID_UserRole");
            DropTable("_MPPL.ID_UserLogin");
            DropTable("_MPPL.ID_UserClaim");
            DropTable("_MPPL.ID_User");
            DropTable("PFEP.PackingInstruction");
            DropTable("PFEP.PackagingIntruction_Item");
            DropTable("PFEP.DEF_Package");
            DropTable("_MPPL.MASTERDATA_Line");
            DropTable("_MPPL.BASE_ExtensionFiles");
            DropTable("_MPPL.MASTERDATA_Department");
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("_MPPL.BASE_Attachment");
            DropTable("_MPPL.MASTERDATA_Area");
            DropTable("_MPPL.MASTERDATA_Anc");
        }
    }
}
