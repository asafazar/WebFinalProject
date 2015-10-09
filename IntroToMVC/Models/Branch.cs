using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntroToMVC.Models
{
    public class Branch
    {
        public int ID { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Latitude")]
        public string lat { get; set; }
        [DisplayName("Longitude")]
        public string lng { get; set; }
        [DisplayName("Branch Manager")]
        public Fan Manager { get; set; }
    }
}