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
    public class ProductController : Controller
    {
        private TaobaoMVCContext db = new TaobaoMVCContext();

        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="token">用户验证token</param>
        /// <param name="product">表单：Name,Pircture,Price</param>
        /// <returns></returns>
        /// <example>POST: /Product/Create</example>
        [HttpPost]
        public ActionResult Create(string token, Product product,int ProductCategory_Id)
        {
            if (ValidMember(token))
            {
                var pc = db.ProductCategories.Find(ProductCategory_Id);
                product.ProductCategory = pc;
                if (ModelState.IsValid)
                {                           
                    db.Products.Add(product);
                    db.SaveChanges();
                    return Json(true);
                }
            }

            return Json("没有权限");

        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns>true或触发的异常</returns>
        /// <example>POST: /Product/Delete/5</example>
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 上传图片功能
        /// </summary>
        /// <param name="upfile">上传的图片</param>
        /// <returns>上传后的路径</returns>
        /// <example>POST: /Product/UploadPicture/</example>
        [HttpPost]
        public ActionResult UploadPicture(HttpPostedFileBase upfile)
        {
            if (upfile != null)
            {
                try
                {
                    string suffix = upfile.FileName.Substring(upfile.FileName.LastIndexOf('.'));
                    string str = "/Upload/" + Guid.NewGuid().ToString() + suffix;
                    upfile.SaveAs(Server.MapPath("~" + str));
                    return Json(str);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            return Json("别闹");
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