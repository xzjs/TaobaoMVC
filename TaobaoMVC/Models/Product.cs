using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaobaoMVC.Models
{
    [DisplayName("商品資訊")]
    [DisplayColumn("Name")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("商品類別")]
        public virtual ProductCategory ProductCategory { get; set; }

        [DisplayName("商品名稱")]
        [Required(ErrorMessage = "請輸入商品名稱")]
        [MaxLength(60, ErrorMessage = "商品名稱不可超過60個字")]
        public string Name { get; set; }

        [DisplayName("商品图片")]
        [Required(ErrorMessage = "商品必须得有图片")]
        public string Picture { get; set; }

        [DisplayName("商品售價")]
        [Required(ErrorMessage = "請輸入商品售價")]
        [Range(1, 10000, ErrorMessage = "商品售價必須介於 1 ~ 10,000 之間")]
        public decimal Price { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}