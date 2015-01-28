using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaobaoMVC.Models
{
    [DisplayName("訂單明細")]
    [DisplayColumn("Name")]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("訂單主檔")]
        public virtual OrderHeader OrderHeader { get; set; }

        [DisplayName("訂購商品")]
        [Required]
        public Product Product { get; set; }

        [DisplayName("选购数量")]
        [Required]
        public int Amount { get; set; }
    }
}