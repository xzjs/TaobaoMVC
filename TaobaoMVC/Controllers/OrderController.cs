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
    [Authorize]
    public class OrderController : Controller
    {
        private TaobaoMVCContext db = new TaobaoMVCContext();

        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View(db.OrderHeaders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderHeader orderheader = db.OrderHeaders.Find(id);
            if (orderheader == null)
            {
                return HttpNotFound();
            }
            return View(orderheader);
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderHeader orderheader)
        {
            if (ModelState.IsValid)
            {
                var member = db.Members.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
                if(member==null)
                {
                    return Json(new
                    {
                        error = "无效的用户身份"
                    });
                }
                OrderHeader oh = new OrderHeader()
                {
                    Member = member,
                    ContactName = orderheader.ContactName,
                    ContactAddress = orderheader.ContactAddress,
                    ContactPhoneNo = orderheader.ContactPhoneNo,
                    BuyOn = DateTime.Now,
                    Memo = orderheader.Memo,
                    OrderDetailItems = new List<OrderDetail>()
                };
                //想个办法将购物车传入
                db.OrderHeaders.Add(orderheader);
                db.SaveChanges();
                return Json(new
                {
                    result = "添加成功"
                });
            }

            return Json(new
            {
                error="请登录"
            });
        }

        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderHeader orderheader = db.OrderHeaders.Find(id);
            if (orderheader == null)
            {
                return HttpNotFound();
            }
            return View(orderheader);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderHeader orderheader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderheader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderheader);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderHeader orderheader = db.OrderHeaders.Find(id);
            if (orderheader == null)
            {
                return HttpNotFound();
            }
            return View(orderheader);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderHeader orderheader = db.OrderHeaders.Find(id);
            db.OrderHeaders.Remove(orderheader);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}