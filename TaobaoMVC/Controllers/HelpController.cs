using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaobaoMVC.Models;

namespace TaobaoMVC.Controllers
{
    /// <summary>
    /// 帮助控制器
    /// </summary>
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            string str = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/TaobaoMVC.XML"));
            return Content(str, "text/xml");
            //return View();
        }

        public ActionResult Test(List<Product> lp)
        {
            //string s=FormsAuthentication.HashPasswordForStoringInConfigFile("xzjs1", "SHA1");
            //HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Json(true, JsonRequestBehavior.AllowGet);

            //return Json(new {{ id = 1, num = 2 },{id = 2,num = 2}}, JsonRequestBehavior.AllowGet);
        }

    }
}
