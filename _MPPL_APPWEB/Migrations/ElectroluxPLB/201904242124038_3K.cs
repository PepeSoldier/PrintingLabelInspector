namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "_MPPL.HSE_SafetyCross", name: "User_Id", newName: "UserId");
            RenameIndex(table: "_MPPL.HSE_SafetyCross", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "_MPPL.HSE_SafetyCross", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "_MPPL.HSE_SafetyCross", name: "UserId", newName: "User_Id");
        }
    }
}
