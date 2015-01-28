using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaobaoMVC.Models
{
    [DisplayName("商品類別")]
    [DisplayColumn("Name")]
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("商品類別名稱")]
        [Required(ErrorMessage = "請輸入商品類別名稱")]
        [MaxLength(20, ErrorMessage = "類別名稱不可超過20個字")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}