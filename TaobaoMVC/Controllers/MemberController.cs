using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Web.Security;
using TaobaoMVC.Models;

namespace TaobaoMVC.Controllers
{
    public class MemberController : Controller
    {
        private TaobaoMVCContext db = new TaobaoMVCContext();
        private string pwSalt = "xzjs";

        //
        // GET: /Member/

        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        //
        // GET: /Member/Details/5

        public ActionResult Details(int id = 0)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //
        // GET: /Member/Create

        public ActionResult Register()
        {
            return View();
        }

        
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="member">标签name：Email，Password，Name,NickName</param>
        /// <returns>json信息</returns>
        /// <example>POST: /Member/Register/</example>
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "Registeron,IsAdmin,AuthCode")]Member member)
        {

            var chk_member = db.Members.Where(p => p.Email == member.Email).FirstOrDefault();
            if (chk_member != null)
            {
                return Json("您输入的Email已经有人注册过了");
            }

            member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + member.Password, "SHA1");
            member.RegisterOn = DateTime.Now;
            member.AuthCode = Guid.NewGuid().ToString();
            db.Members.Add(member);
            db.SaveChanges();
            SendAuthCodeToMember(member);
            return Json("请登录邮箱完成邮箱认证");
        }

        //
        // GET: /Member/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //
        // POST: /Member/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        //
        // GET: /Member/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //
        // POST: /Member/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="email">用户email</param>
        /// <param name="password">用户密码，未加密的</param>
        /// <returns>返回一个token作为验证凭证，否则为错误信息</returns>
        public ActionResult Login(string email, string password)
        {
            var hash_pw = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + password, "SHA1");
            var member = (from p in db.Members
                          where p.Email == email && p.Password == hash_pw
                          select p).FirstOrDefault();
            if (member != null)
            {
                if (member.AuthCode == null)
                {
                    //FormsAuthentication.SetAuthCookie(email,false);
                    string token = Guid.NewGuid().ToString();
                    HttpContext.Application[token] = member;
                    return Json(new { token = token });
                }
                else
                {
                    return Json("未通过验证");
                }
            }
            else
            {
                return Json("账号或密码错误");
            }
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
        }

        private void SendAuthCodeToMember(Member member)
        {
            // 準備郵件內容
            string mailBody = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/MemberRegisterEMailTemplate.htm"));

            mailBody = mailBody.Replace("{{Name}}", member.Name);
            mailBody = mailBody.Replace("{{RegisterOn}}", member.RegisterOn.ToString("F"));
            var auth_url = new UriBuilder(Request.Url)
            {
                Path = Url.Action("ValidateRegister", new { id = member.AuthCode }),
                Query = ""
            };
            mailBody = mailBody.Replace("{{AUTH_URL}}", auth_url.ToString());

            // 發送郵件給會員
            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.163.com");
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("asp_net2014@163.com", "aspnet2014");
                SmtpServer.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("asp_net2014@163.com");
                mail.To.Add(member.Email);
                mail.Subject = "【淘宝】會員註冊確認信";
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
                // 發生郵件寄送失敗，需紀錄進資料庫備查，以免有會員無法登入
            }
        }

        public ActionResult ValidateRegister(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var member = db.Members.Where(p => p.AuthCode == id).FirstOrDefault();

            if (member != null)
            {
                //result = "会员验证成功，可以登录";
                member.AuthCode = null;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("验证码无效" , JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CheckDup(string Email)
        {
            var member = db.Members.Where(p => p.Email == Email).FirstOrDefault();
            if (member != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        /// <summary>
        /// 获得用户实例
        /// </summary>
        /// <example>Post /Member/GetMember/</example>
        /// <param name="token">身份凭证</param>
        /// <returns>包含用户昵称和是否为管理员身份</returns>
        [HttpPost]
        public ActionResult GetMember(string token)
        {
            var member = (Member)HttpContext.Application[token];
            if (member != null)
            {
                return Json(new { name = member.Nickname, member.IsAdmin });
            }
            else
            {
                return Json("用户凭证错误");
            }
        }
    }
}