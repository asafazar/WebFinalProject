using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroToMVC.Models
{
    public class Post
    {
        public int ID { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Writer")]
        public string Writer { get; set; }
        [DisplayName("Writer WebSite")]
        public string WebSite { get; set; }
        [DisplayName("Posting Date")]
        public DateTime PostingDate { get; set; }
        [DisplayName("Post Content")]
        public string Content { get; set; }
        [DisplayName("Post Comments")]
        public virtual ICollection<Comment> Comments { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }
        public string Video { get; set; }
    }
}