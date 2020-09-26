namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.MASTERDATA_Client", newName: "MASTERDATA_Contractor");
        }
        
        public override void Down()
        {
            RenameTable(name: "_MPPL.MASTERDATA_Contractor", newName: "MASTERDATA_Client");
        }
    }
}
