namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1P : DbMigration
    {
        public override void Up()
        {
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
                "CORE.BOM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(),
                        PncId = c.Int(),
                        LV = c.Int(nullable: false),
                        PCS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        BC = c.String(maxLength: 3),
                        DEF = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 12),
                        Suffix = c.String(maxLength: 12),
                        IDCO = c.String(maxLength: 12),
                        Task = c.String(maxLength: 50),
                        TaskForce = c.String(maxLength: 50),
                        Formula = c.String(maxLength: 50),
                        Condition = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.AncId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.PncId)
                .Index(t => t.AncId)
                .Index(t => t.PncId);
            
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
                .ForeignKey("_MPPL.MASTERDATA_Process", t => t.ProcessId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.ResourceGroupId)
                .Index(t => t.ProcessId)
                .Index(t => t.ItemGroupId)
                .Index(t => t.ResourceGroupId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Process",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        ParentId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Type = c.Int(nullable: false),
                        ResourceGroupId = c.Int(),
                        AreaId = c.Int(),
                        Color = c.String(maxLength: 35),
                        FlowTime = c.Int(nullable: false),
                        OldId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.ResourceGroupId)
                .Index(t => t.ResourceGroupId)
                .Index(t => t.AreaId);
            
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
            
            CreateTable(
                "CORE.BOM_Workorder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(maxLength: 20),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                        BC1 = c.String(maxLength: 3),
                        LV = c.Int(nullable: false),
                        QtyUsed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BC2 = c.String(maxLength: 3),
                        DEF = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 12),
                        Suffix = c.String(maxLength: 12),
                        IDCO = c.String(maxLength: 12),
                        DirPar = c.String(maxLength: 20),
                        UnitOfMeasure = c.Int(nullable: false),
                        InterItm = c.String(),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ChildId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
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
                        Id = c.Int(nullable: false),
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
                "_MPPL.MASTERDATA_LabourBrigade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "PRD.ProdOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(maxLength: 20),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        QtyPlanned = c.Int(nullable: false),
                        QtyRemain = c.Int(nullable: false),
                        QtyProducedInPast = c.Int(nullable: false),
                        PncId = c.Int(nullable: false),
                        LineId = c.Int(nullable: false),
                        Notice = c.String(),
                        Sequence = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        SerialNoFrom = c.String(maxLength: 25),
                        SerialNoTo = c.String(maxLength: 25),
                        CounterProductsIn = c.Int(nullable: false),
                        CounterProductsOut = c.Int(nullable: false),
                        CounterProductsFGW = c.Int(nullable: false),
                        FirstProductIn = c.DateTime(),
                        LastProductIn = c.DateTime(),
                        FirstProductOut = c.DateTime(),
                        LastProductOut = c.DateTime(),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.LineId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.PncId)
                .Index(t => t.PncId)
                .Index(t => t.LineId);
            
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
                "_MPPL.MASTERDATA_Workstation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        SortOrder = c.Int(nullable: false),
                        SortOrderTrain = c.Int(nullable: false),
                        FlowRackLOverride = c.Int(nullable: false),
                        LineId = c.Int(),
                        AreaId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        ProductsFromIn = c.Int(nullable: false),
                        ProductsFromOut = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.LineId)
                .Index(t => t.LineId)
                .Index(t => t.AreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("CORE.SystemVariables", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "RoleId", "_MPPL.IDENTITY_Role");
            DropForeignKey("PRD.ProdOrder", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PRD.ProdOrder", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("CORE.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "SuperVisorUserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserLogin", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("_MPPL.IDENTITY_UserClaim", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("CORE.BOM_Workorder", "ParentId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM_Workorder", "ChildId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_ItemUoM", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ProcessId", "_MPPL.MASTERDATA_Process");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "_MPPL.MASTERDATA_Item");
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "LineId" });
            DropIndex("CORE.SystemVariables", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_Role", "RoleNameIndex");
            DropIndex("PRD.ProdOrder", new[] { "LineId" });
            DropIndex("PRD.ProdOrder", new[] { "PncId" });
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_User", "UserNameIndex");
            DropIndex("_MPPL.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.IDENTITY_User", new[] { "DepartmentId" });
            DropIndex("CORE.ChangeLog", new[] { "UserId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ChildId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ParentId" });
            DropIndex("_MPPL.MASTERDATA_ItemUoM", new[] { "ItemId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ItemGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ProcessId" });
            DropIndex("CORE.BOM", new[] { "PncId" });
            DropIndex("CORE.BOM", new[] { "AncId" });
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("CORE.SystemVariables");
            DropTable("_MPPL.IDENTITY_Role");
            DropTable("PRD.ProdOrder");
            DropTable("CORE.Printer");
            DropTable("CORE.NotificationDevice");
            DropTable("_MPPL.MASTERDATA_LabourBrigade");
            DropTable("_MPPL.MASTERDATA_Contractor");
            DropTable("_MPPL.IDENTITY_UserRole");
            DropTable("_MPPL.IDENTITY_UserLogin");
            DropTable("_MPPL.MASTERDATA_Department");
            DropTable("_MPPL.IDENTITY_UserClaim");
            DropTable("_MPPL.IDENTITY_User");
            DropTable("CORE.ChangeLog");
            DropTable("CORE.BOM_Workorder");
            DropTable("_MPPL.MASTERDATA_ItemUoM");
            DropTable("_MPPL.MASTERDATA_Resource");
            DropTable("_MPPL.MASTERDATA_Process");
            DropTable("_MPPL.MASTERDATA_Item");
            DropTable("CORE.BOM");
            DropTable("CORE.Attachment");
            DropTable("_MPPL.MASTERDATA_Area");
        }
    }
}
