using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaobaoMVC.Models;

namespace TaobaoMVC.Controllers
{
    public class HomeController : Controller
    {
        TaobaoMVCContext db = new TaobaoMVCContext();

        /// <summary>
        /// 获得商品列表
        /// </summary>
        /// <returns></returns>
        /// <example>GET: /Home/</example>
        public ActionResult Index(int id=0)
        {
            //try
            //{
            //    HttpContext.Application["1"] = db.Members.Find(1);
            //}
            //catch (Exception ex) { }
            List<ProductCategory> data = new List<ProductCategory>();
            if (id == 0)
            {
                data = db.ProductCategories.ToList();
            }
            else
            {
                data = db.ProductCategories.Where(x => x.Id == id).ToList();
            }
            if (data.Count == 0)
            {
                db.ProductCategories.Add(new ProductCategory() { Id = 1, Name = "文具" });
                db.ProductCategories.Add(new ProductCategory() { Id = 2, Name = "礼品" });
                db.ProductCategories.Add(new ProductCategory() { Id = 3, Name = "书籍" });
                db.ProductCategories.Add(new ProductCategory() { Id = 4, Name = "美劳用品" });
                db.SaveChanges();
                //data = db.ProductCategories.ToList();
            }
            var data_p = db.Products.ToList();
            if (data_p.Count == 0)
            {
                foreach (var item in data)
                {
                    item.Products.Add(new Product() { Name = item.Name + "商品1", Picture = "aaa", Price = 1.2M, ProductCategory = item });
                    item.Products.Add(new Product() { Name = item.Name + "商品2", Picture = "bbb", Price = 2.2M, ProductCategory = item });
                    db.SaveChanges();
                }
                data_p = db.Products.ToList();
            }
            var collect_category = data.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                products = x.Products.Select(y => new
                {
                    id = y.Id,
                    name = y.Name,
                    picture = y.Picture,
                    price = y.Price,
                    category = y.ProductCategory.Name
                })
            });
            //var collect = data_p.Select(x => new
            //    {
            //        id = x.Id,
            //        name = x.Name,
            //        picture = x.Picture,
            //        price = x.Price,
            //        caregory = x.ProductCategory.Name
            //    });
            return Json(collect_category, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductList(int id)
        {
            return View();
        }

        /// <summary>
        /// 获取商品详细信息
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns>json</returns>
        /// <example>GET: /Home/ProductDetail/11</example>
        public ActionResult ProductDetail(int id)
        {
            var data = db.Products.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var collect = new
            {
                id = data.Id,
                name = data.Name,
                picture = data.Picture,
                price = data.Price,
                category = data.ProductCategory.Name,
                comments = data.Comments.Select(x => new
                {
                    content=x.Content
                })
            };
            return Json(collect, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="str">查询字符串，空格隔开</param>
        /// <returns></returns>
        /// <example>POST /Home/Search/</example>
        [HttpPost]
        public ActionResult Search(string str)
        {
            string[] keys = str.Trim().Split(' ');
            var pl = db.Products.ToList();
            foreach (var item in keys)
            {
                pl = pl.Where(x => x.Name.Contains(item) || x.ProductCategory.Name.Contains(item)).ToList();
            }
            var collect = pl.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                picture = x.Picture,
                price = x.Price,
                caregory = x.ProductCategory.Name
            });
            return Json(collect);
        }
    }
}
