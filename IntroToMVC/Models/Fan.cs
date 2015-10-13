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
        Fan() { }
        public Fan(int nPermission,
                   string stFirstName,
                   string stLastName,
                   string stGender,
                   DateTime dtBirthDate,
                   int nNumOfYearsInClub,
                   string stUserName,
                   string stPassword
            ){
            this.Permission = nPermission;
            this.FirstName = stFirstName;
            this.LastName = stLastName;
            this.Gender = stGender;
            this.BirthDate = dtBirthDate;
            this.NumOfYearsInClub = nNumOfYearsInClub;
            this.UserName = stUserName;
            this.Password = stPassword;
        }
       
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
}