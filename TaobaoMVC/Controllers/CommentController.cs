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
    public class CommentController : Controller
    {
        private TaobaoMVCContext db = new TaobaoMVCContext();

        //
        // GET: /Comment/

        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }

        //
        // GET: /Comment/Details/5

        public ActionResult Details(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // GET: /Comment/Create

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment">标签name：Content</param>
        /// <param name="Product_ID">产品ID</param>
        /// <param name="token">用户验证token</param>
        /// <example>POST: /Comment/Create</example>
        /// <returns>错误信息或者true</returns>
        [HttpPost]
        public ActionResult Create(Comment comment, int Product_ID, string token)
        {
            try
            {
                if (ValidMember(token))
                {
                    var m =db.Members.Find(((Member)HttpContext.Application[token]).Id);
                    //Member m = db.Members.Find(Member_ID);
                    if (m == null)
                    {
                        return Json("用户不存在");
                    }
                    Product p = db.Products.Find(Product_ID);
                    if (p == null)
                    {
                        return Json("商品不存在");
                    }
                    comment.Date = DateTime.Now;
                    comment.Member = m;
                    comment.Product = p;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return Json(true);
                }
                return Json("没有权限");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //
        // GET: /Comment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comment/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id">评论ID</param>
        /// <example> POST: /Comment/Delete/5</example>
        /// <returns>错误信息或true</returns>
        [HttpPost]
        public ActionResult Delete(int id,string token)
        {
            try
            {
                if (ValidMember(token))
                {
                    Comment comment = db.Comments.Find(id);
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                    return Json(true);
                }
                return Json("没有权限");
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
        /// 验证用户token的权限
        /// </summary>
        /// <param name="token">传回来的token</param>
        /// <returns>true ，false</returns>
        private bool ValidMember(string token)
        {
            var member = (Member)HttpContext.Application[token];
            if (member == null)
            {
                return false;
            }
            return true;
        }
    }
}