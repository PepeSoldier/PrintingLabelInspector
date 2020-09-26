namespace _MPPL_WEB_START.Migrations.ElectroluxPLS
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        SubDirectory = c.String(maxLength: 100),
                        FileNamePrefix = c.String(maxLength: 50),
                        FileNameSuffix = c.String(maxLength: 50),
                        PackingCardUrl = c.String(maxLength: 150),
                        Extension = c.String(maxLength: 10),
                        ParentObjectId = c.Int(nullable: false),
                        ParentType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "CORE.ChangeLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectName = c.String(maxLength: 70),
                        ObjectDescription = c.String(maxLength: 100),
                        FieldName = c.String(maxLength: 70),
                        FieldDisplayName = c.String(maxLength: 140),
                        NewValue = c.String(maxLength: 255),
                        OldValue = c.String(maxLength: 255),
                        ObjectId = c.Int(nullable: false),
                        ParentObjectId = c.Int(nullable: false),
                        ParentObjectName = c.String(maxLength: 70),
                        ParentObjectDescription = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.IDENTITY_User",
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
                .ForeignKey("_MPPL.MASTERDATA_Department", t => t.DepartmentId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.SuperVisorUserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SuperVisorUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "_MPPL.IDENTITY_UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
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
                "_MPPL.IDENTITY_UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.IDENTITY_UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("_MPPL.IDENTITY_Role", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Contractor",
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
            
            CreateTable(
                "_MPPL.MASTERDATA_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        OriginalName = c.String(maxLength: 100),
                        Name = c.String(maxLength: 100),
                        Comment = c.String(maxLength: 50),
                        Color1 = c.String(maxLength: 25),
                        Color2 = c.String(maxLength: 25),
                        Specific1 = c.String(maxLength: 25),
                        Specific2 = c.String(maxLength: 25),
                        Specific3 = c.String(maxLength: 25),
                        Specific4 = c.String(maxLength: 25),
                        DEF = c.String(maxLength: 25),
                        BC = c.String(maxLength: 25),
                        PREFIX = c.String(maxLength: 25),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        ItemGroupId = c.Int(),
                        ResourceGroupId = c.Int(),
                        UnitOfMeasure = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Id_old = c.Int(),
                        Color = c.String(maxLength: 25),
                        New = c.Boolean(nullable: false),
                        Lenght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        IsCommon = c.Boolean(nullable: false),
                        Old_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemGroupId)
                .Index(t => t.ItemGroupId);
            
            CreateTable(
                "iLOGIS.CONFIG_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        H = c.Int(nullable: false),
                        PickerNo = c.Int(nullable: false),
                        TrainNo = c.Int(nullable: false),
                        ABC = c.Int(nullable: false),
                        XYZ = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
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
            
            CreateTable(
                "iLOGIS.CONFIG_PackageItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        ItemWMSId = c.Int(nullable: false),
                        WarehouseId = c.Int(),
                        WarehouseLocationTypeId = c.Int(nullable: false),
                        PickingStrategy = c.Int(nullable: false),
                        QtyPerPackage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackagesPerPallet = c.Int(nullable: false),
                        PalletW = c.Int(nullable: false),
                        PalletD = c.Int(nullable: false),
                        PalletH = c.Int(nullable: false),
                        WeightGross = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .Index(t => t.PackageId)
                .Index(t => t.ItemWMSId);
            
            CreateTable(
                "iLOGIS.CONFIG_Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackagesPerPallet = c.Int(nullable: false),
                        FullPalletHeight = c.Int(nullable: false),
                        Returnable = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "_MPPL.IDENTITY_Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
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
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "iLOGIS.WHDOC_WhDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContractorId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 12),
                        ReferrenceDocument = c.String(maxLength: 40),
                        CostPayer = c.String(maxLength: 128),
                        DocumentDate = c.DateTime(nullable: false),
                        StampTime = c.DateTime(nullable: false),
                        CostCenter = c.String(maxLength: 32),
                        Reason = c.String(maxLength: 128),
                        MeansOfTransport = c.String(maxLength: 128),
                        CreatorId = c.String(maxLength: 128),
                        IssuerId = c.String(maxLength: 128),
                        IssueDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        isSigned = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ForwadrerComments = c.String(),
                        ApproverId = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.ContractorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.CreatorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.IssuerId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.ApproverId)
                .Index(t => t.ContractorId)
                .Index(t => t.CreatorId)
                .Index(t => t.IssuerId)
                .Index(t => t.ApproverId);
            
            CreateTable(
                "iLOGIS.WHDOC_WhDocumentItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        WhDocumentId = c.Int(nullable: false),
                        ItemWMSId = c.Int(),
                        PackageId = c.Int(),
                        ItemCode = c.String(maxLength: 100),
                        ItemName = c.String(maxLength: 100),
                        DisposedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssuedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("iLOGIS.WHDOC_WhDocument", t => t.WhDocumentId)
                .Index(t => t.UserId)
                .Index(t => t.WhDocumentId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.PackageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "ApproverId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "WhDocumentId", "iLOGIS.WHDOC_WhDocument");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "IssuerId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "CreatorId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "ContractorId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("CORE.SystemVariables", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "RoleId", "_MPPL.IDENTITY_Role");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "SuperVisorUserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserLogin", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("_MPPL.IDENTITY_UserClaim", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "PackageId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "WhDocumentId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "UserId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ApproverId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "IssuerId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "CreatorId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            DropIndex("CORE.SystemVariables", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_Role", "RoleNameIndex");
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "PackageId" });
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "ItemId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ItemGroupId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_User", "UserNameIndex");
            DropIndex("_MPPL.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.IDENTITY_User", new[] { "DepartmentId" });
            DropIndex("CORE.ChangeLog", new[] { "UserId" });
            DropTable("iLOGIS.WHDOC_WhDocumentItem");
            DropTable("iLOGIS.WHDOC_WhDocument");
            DropTable("CORE.SystemVariables");
            DropTable("_MPPL.IDENTITY_Role");
            DropTable("CORE.Printer");
            DropTable("iLOGIS.CONFIG_Package");
            DropTable("iLOGIS.CONFIG_PackageItem");
            DropTable("CORE.NotificationDevice");
            DropTable("iLOGIS.CONFIG_Item");
            DropTable("_MPPL.MASTERDATA_Item");
            DropTable("_MPPL.MASTERDATA_Contractor");
            DropTable("_MPPL.IDENTITY_UserRole");
            DropTable("_MPPL.IDENTITY_UserLogin");
            DropTable("_MPPL.MASTERDATA_Department");
            DropTable("_MPPL.IDENTITY_UserClaim");
            DropTable("_MPPL.IDENTITY_User");
            DropTable("CORE.ChangeLog");
            DropTable("CORE.Attachment");
        }
    }
}
