namespace IntroToMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Last : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        lat = c.String(),
                        lng = c.String(),
                        Manager_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Fans", t => t.Manager_ID)
                .Index(t => t.Manager_ID);
            
            CreateTable(
                "dbo.Fans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Permission = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        NumOfYearsInClub = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RelatedPostID = c.Int(nullable: false),
                        Title = c.String(),
                        Writer = c.String(),
                        WriterWebSite = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posts", t => t.RelatedPostID, cascadeDelete: true)
                .Index(t => t.RelatedPostID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Writer = c.String(),
                        WebSite = c.String(),
                        PostingDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        Image = c.String(nullable: true),
                        Video = c.String(nullable: true)
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "RelatedPostID", "dbo.Posts");
            DropForeignKey("dbo.Branches", "Manager_ID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "RelatedPostID" });
            DropIndex("dbo.Branches", new[] { "Manager_ID" });
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Fans");
            DropTable("dbo.Branches");
        }
    }
}
