using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaobaoMVC.Models
{
    [DisplayName("评论")]
    [DisplayColumn("Name")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("内容")]
        [Required]
        public string Content { get; set; }

        [DisplayName("日期")]
        public DateTime Date { get; set; }

        public virtual Member Member { get; set; }

        public virtual Product Product { get; set; } 
    }
}