using System;
using System.ComponentModel;
using System.Data.Entity;

namespace IntroToMVC.Models
{
    public enum Permissions
    {
        User = 1,
        Admin = 2
    };

    public class Fan
    {
        public int ID { get; set; }
        public int Permission { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
        [DisplayName("Number Of Years In The Club")]
        public int NumOfYearsInClub { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }

    }

    public class FanDBContext : DbContext
    {
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public FanDBContext()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<FanDBContext>());
        }

    }
}