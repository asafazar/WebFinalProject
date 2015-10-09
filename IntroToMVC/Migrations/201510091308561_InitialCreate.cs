namespace IntroToMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Fans", "UserName", c => c.String());
            //AddColumn("dbo.Fans", "Password", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Fans", "Password");
            //DropColumn("dbo.Fans", "UserName");
        }
    }
}
