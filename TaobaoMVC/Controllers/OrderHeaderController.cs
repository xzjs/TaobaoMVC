using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaobaoMVC.Models;
using TaobaoMVC.App_Code;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace TaobaoMVC.Controllers
{
    public class OrderHeaderController : Controller
    {
        private TaobaoMVCContext db = new TaobaoMVCContext();

        //
        // GET: /OrderHeader/

        public ActionResult Index()
        {
            return View(db.OrderHeaders.ToList());
        }

        //
        // GET: /OrderHeader/Details/5

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
        // GET: /OrderHeader/Create

        public ActionResult Create()
        {
            return View();
        }
 
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="orderheader">表单：ContactName,ContactPhoneNo,ContactAddress,Memo</param>
        /// <param name="token"></param>
        /// <param name="json">代表购物车的json：[{"pid":1,"num":1},{"pid":2,"num":2}]</param>
        /// <returns></returns>
        /// <example>POST: /OrderHeader/Create</example>
        [HttpPost]
        public ActionResult Create(OrderHeader orderheader, string token, string json)
        {
            try
            {
                var member =db.Members.Find(((Member)HttpContext.Application[token]).Id);
                if (member != null)
                {
                    var serializer = new JavaScriptSerializer();
                    List<CartJson> lcj = serializer.Deserialize<List<CartJson>>(json);
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
                    decimal total_price = 0;
                    foreach (var item in lcj)
                    {
                        var product = db.Products.Find(item.pid);
                        if (product == null)
                        {
                            return Json("没有指定商品");
                        }
                        total_price += product.Price * item.num;
                        oh.OrderDetailItems.Add(new OrderDetail() { Product = product, Amount = item.num });
                    }
                    oh.TotalPrice = total_price;
                    db.OrderHeaders.Add(oh);
                    db.SaveChanges();
                    return Json(true);
                }

                return Json("未识别用户");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //
        // GET: /OrderHeader/Edit/5

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
        // POST: /OrderHeader/Edit/5

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
        // GET: /OrderHeader/Delete/5

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
        // POST: /OrderHeader/Delete/5
        /// <summary>
        /// 订单不需要删除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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