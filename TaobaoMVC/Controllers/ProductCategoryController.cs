using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaobaoMVC.Models;

namespace TaobaoMVC.Controllers
{
    public class ProductCategoryController : Controller
    {
        private TaobaoMVCContext db = new TaobaoMVCContext();

        /// <summary>
        /// 获取商品类别
        /// </summary>
        /// <returns>json</returns>
        /// <example>GET: /ProductCategory/</example>
        public ActionResult Index()
        {
            var data = db.ProductCategories.ToList();
            var collect = data.Select(x => new
            {
                id = x.Id,
                name = x.Name
            });
            return Json(collect,JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /ProductCategory/Details/5

        public ActionResult Details(int id = 0)
        {
            ProductCategory productcategory = db.ProductCategories.Find(id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }
            return View(productcategory);
        }

        //
        // GET: /ProductCategory/Create

        public ActionResult Create()
        {
            return View();
        }

       
        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="token">身份验证token</param>
        /// <param name="productcategory">表单：Name</param>
        /// <returns>json</returns>
        /// <example>POST: /ProductCategory/Create</example>
        [HttpPost]
        public ActionResult Create(string token, ProductCategory productcategory)
        {
            if (ValidMember(token))
            {

                var pc = db.ProductCategories.Where(p => p.Name == productcategory.Name).FirstOrDefault();
                if (pc != null)
                {
                    return Json("已有该类别");
                }
                if (ModelState.IsValid)
                {
                    db.ProductCategories.Add(productcategory);
                    db.SaveChanges();
                    return Json(true);
                }
            }
            return Json("没有权限");
            
        }

        //
        // GET: /ProductCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProductCategory productcategory = db.ProductCategories.Find(id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }
            return View(productcategory);
        }

        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="token">用户验证token</param>
        /// <param name="productcategory">表单：Name</param>
        /// <returns>json</returns>
        /// <example>POST: /ProductCategory/Edit/5(最后的数字为商品id)</example>
        [HttpPost]
        public ActionResult Edit(string token,ProductCategory productcategory)
        {
            if (ValidMember(token))
            {

                if (ModelState.IsValid)
                {
                    db.Entry(productcategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(true);
                }
            }
            return Json("没有权限");
        }

        //
        // GET: /ProductCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProductCategory productcategory = db.ProductCategories.Find(id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }
            return View(productcategory);
        }

        /// <summary>
        /// 删除一个商品分类
        /// </summary>
        /// <param name="token">身份验证token</param>
        /// <param name="id">商品类别IDid</param>
        /// <returns>json</returns>
        /// <example>POST: /ProductCategory/Delete/5</example>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string token, int id)
        {
            try
            {
                if (ValidMember(token))
                {
                    ProductCategory productcategory = db.ProductCategories.Find(id);
                    db.ProductCategories.Remove(productcategory);
                    db.SaveChanges();
                    return Json(true);
                }
                return Json("没有权限");
            }
            catch (Exception ex) {
                return Json(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 验证用户token的权限
        /// </summary>
        /// <param name="token">传回来的token</param>
        /// <returns>true ，false</returns>
        private bool ValidMember(string token)
        {
            var member = (Member)HttpContext.Application[token];
            if (member == null || member.IsAdmin == false)
            {
                return false;
            }
            return true;
        }
    }
}