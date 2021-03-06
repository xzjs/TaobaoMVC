﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaobaoMVC.Models
{
    [DisplayName("會員資料")]
    [DisplayColumn("Name")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入 Email 地址")]
        [Description("我們直接以 Email 當成會員的登入帳號")]
        [MaxLength(250, ErrorMessage = "Email地址長度無法超過250個字元")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [MaxLength(40, ErrorMessage = "請輸入密碼")]
        [Description("密碼將以SHA1進行雜湊運算，透過SHA1雜湊運算後的結果轉為HEX表示法的字串長度皆為40個字元")]
        public string Password { get; set; }

        [DisplayName("中文姓名")]
        [Required(ErrorMessage = "請輸入中文姓名")]
        [MaxLength(5, ErrorMessage = "中文姓名不可超過5個字")]
        [Description("暫不考慮有外國人用英文註冊會員的情況")]
        public string Name { get; set; }

        [DisplayName("網路暱稱")]
        [MaxLength(10, ErrorMessage = "網路暱稱請勿輸入超過10個字")]
        public string Nickname { get; set; }

        [DisplayName("會員註冊時間")]
        public DateTime RegisterOn { get; set; }

        [DisplayName("會員啟用認證碼")]
        [MaxLength(36)]
        [Description("當 AuthCode 等於 null 則代表此會員已經通過Email有效性驗證")]
        public string AuthCode { get; set; }

        [DisplayName("是否是管理员")]
        public bool IsAdmin { get; set; }

        public virtual ICollection<OrderHeader> Orders { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}