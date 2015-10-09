namespace IntroToMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _try : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RelatedPostID = c.Int(nullable: false),
                        Title = c.String(),
                        Writer = c.String(),
                        WriterWebSite = c.String(),
                        Content = c.Int(nullable: false),
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
                        Content = c.Int(nullable: false),
                        PostImage = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Fans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        NumOfYearsInClub = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "RelatedPostID", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "RelatedPostID" });
            DropTable("dbo.Fans");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
