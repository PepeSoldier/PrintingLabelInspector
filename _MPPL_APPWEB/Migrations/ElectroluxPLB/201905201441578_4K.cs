namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("_MPPL.IDENTITY_User", "DepartmentId");
            AddForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropIndex("_MPPL.IDENTITY_User", new[] { "DepartmentId" });
            DropTable("_MPPL.MASTERDATA_Department");
        }
    }
}
