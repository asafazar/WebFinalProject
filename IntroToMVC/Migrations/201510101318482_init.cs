namespace IntroToMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Posts", "Image", c => c.String());
            //AddColumn("dbo.Posts", "Video", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Posts", "Video");
            //DropColumn("dbo.Posts", "Image");
        }
    }
}
