using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntroToMVC.Models
{
    public class BranchReport
    {
        public Branch Branch { get; set; }
        public Fan Manager { get; set; }
    }
}