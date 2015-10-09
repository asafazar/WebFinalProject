using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;


namespace IntroToMVC.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Related Post ID")]
        public int RelatedPostID { get; set; }
        [DisplayName("Comment Title")]
        public string Title { get; set; }
        [DisplayName("Comment Writer")]
        public string Writer { get; set; }
        [DisplayName("Comment Writer WebSite")]
        public string WriterWebSite { get; set; }
        [DisplayName("Content")]
        public string Content { get; set; }
        [DisplayName("Comment to")]
        public Post RelatedPost { get; set; }
    }
}