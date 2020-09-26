namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8K : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.ID_User", newName: "IDENTITY_User");
            RenameTable(name: "_MPPL.ID_UserClaim", newName: "IDENTITY_UserClaim");
            RenameTable(name: "_MPPL.ID_UserLogin", newName: "IDENTITY_UserLogin");
            RenameTable(name: "_MPPL.ID_UserRole", newName: "IDENTITY_UserRole");
            RenameTable(name: "_MPPL.ID_Role", newName: "IDENTITY_Role");
            CreateTable(
                "_MPPL.MASTERDATA_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        NameShort = c.String(nullable: false, maxLength: 25),
                        SortOrder1 = c.Int(nullable: false),
                        SortOrder2 = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.ParentId)
                .ForeignKey("_MPPL.MASTERDATA_Resource_Type", t => t.TypeId)
                .Index(t => t.ParentId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Resource_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Color = c.String(maxLength: 25),
                        Icon = c.String(maxLength: 100),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("_MPPL.MASTERDATA_Anc", "TypeId");
            DropColumn("_MPPL.MASTERDATA_Pnc", "TypeId");
            DropColumn("_MPPL.MASTERDATA_Workstation", "TypeId");
            DropTable("_MPPL.BASE_ExtensionFiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "_MPPL.BASE_ExtensionFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileExtension = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("_MPPL.MASTERDATA_Workstation", "TypeId", c => c.Int());
            AddColumn("_MPPL.MASTERDATA_Pnc", "TypeId", c => c.Int());
            AddColumn("_MPPL.MASTERDATA_Anc", "TypeId", c => c.Int());
            DropForeignKey("_MPPL.MASTERDATA_Resource", "TypeId", "_MPPL.MASTERDATA_Resource_Type");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ParentId", "_MPPL.MASTERDATA_Resource");
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "TypeId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ParentId" });
            DropTable("_MPPL.MASTERDATA_Resource_Type");
            DropTable("_MPPL.MASTERDATA_Resource");
            RenameTable(name: "_MPPL.IDENTITY_Role", newName: "ID_Role");
            RenameTable(name: "_MPPL.IDENTITY_UserRole", newName: "ID_UserRole");
            RenameTable(name: "_MPPL.IDENTITY_UserLogin", newName: "ID_UserLogin");
            RenameTable(name: "_MPPL.IDENTITY_UserClaim", newName: "ID_UserClaim");
            RenameTable(name: "_MPPL.IDENTITY_User", newName: "ID_User");
        }
    }
}
