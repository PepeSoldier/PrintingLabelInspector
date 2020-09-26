namespace _MPPL_WEB_START.Migrations.Eldisy2
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ParentId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "TypeId", "_MPPL.MASTERDATA_Resource_Type");
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ParentId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "TypeId" });
            AddColumn("_MPPL.MASTERDATA_Client", "Country", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "Language", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "NIP", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "ContactPersonName", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "ContactPhoneNumber", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "ContactEmail", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "ContactAdress", c => c.String());
            AddColumn("_MPPL.MASTERDATA_Client", "Deleted", c => c.Boolean(nullable: false));
            DropTable("_MPPL.MASTERDATA_Resource");
            DropTable("_MPPL.MASTERDATA_Resource_Type");
        }
        
        public override void Down()
        {
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
            
            CreateTable(
                "_MPPL.MASTERDATA_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        NameShort = c.String(nullable: false, maxLength: 25),
                        SortOrder1 = c.Int(nullable: false),
                        SortOrder2 = c.Int(nullable: false),
                        ParentId = c.Int(),
                        TypeId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("_MPPL.MASTERDATA_Client", "Deleted");
            DropColumn("_MPPL.MASTERDATA_Client", "ContactAdress");
            DropColumn("_MPPL.MASTERDATA_Client", "ContactEmail");
            DropColumn("_MPPL.MASTERDATA_Client", "ContactPhoneNumber");
            DropColumn("_MPPL.MASTERDATA_Client", "ContactPersonName");
            DropColumn("_MPPL.MASTERDATA_Client", "NIP");
            DropColumn("_MPPL.MASTERDATA_Client", "Language");
            DropColumn("_MPPL.MASTERDATA_Client", "Country");
            CreateIndex("_MPPL.MASTERDATA_Resource", "TypeId");
            CreateIndex("_MPPL.MASTERDATA_Resource", "ParentId");
            AddForeignKey("_MPPL.MASTERDATA_Resource", "TypeId", "_MPPL.MASTERDATA_Resource_Type", "Id");
            AddForeignKey("_MPPL.MASTERDATA_Resource", "ParentId", "_MPPL.MASTERDATA_Resource", "Id");
        }
    }
}
