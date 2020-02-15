namespace Clinic_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "UserType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserType", c => c.String());
        }
    }
}
