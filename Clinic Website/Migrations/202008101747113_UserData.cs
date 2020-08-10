namespace Clinic_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Height", c => c.Short(nullable: false));
            AddColumn("dbo.AspNetUsers", "Weight", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Weight");
            DropColumn("dbo.AspNetUsers", "Height");
        }
    }
}
