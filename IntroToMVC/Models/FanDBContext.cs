using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntroToMVC.Models
{
    public class FanDBContext : DbContext
    {
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public FanDBContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<FanDBContext>());
        }

    }
}