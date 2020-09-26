namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.AccCross",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                .ForeignKey("_MPPL.IDENTITY_User", t => t.SuperVisorUserId)
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
                "_MPPL.IDENTITY_Role",
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.IDENTITY_UserRole", "RoleId", "_MPPL.IDENTITY_Role");
            DropForeignKey("_MPPL.AccCross", "User_Id", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "SuperVisorUserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserLogin", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserClaim", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("_MPPL.IDENTITY_Role", "RoleNameIndex");
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_User", "UserNameIndex");
            DropIndex("_MPPL.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.AccCross", new[] { "User_Id" });
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("_MPPL.IDENTITY_Role");
            DropTable("_MPPL.MASTERDATA_LabourBrigade");
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("_MPPL.IDENTITY_UserRole");
            DropTable("_MPPL.IDENTITY_UserLogin");
            DropTable("_MPPL.IDENTITY_UserClaim");
            DropTable("_MPPL.IDENTITY_User");
            DropTable("_MPPL.AccCross");
        }
    }
}
