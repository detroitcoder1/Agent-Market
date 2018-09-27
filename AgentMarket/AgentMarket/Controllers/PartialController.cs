using AgentMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace AgentMarket.Controllers
{
    public class PartialController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Menu/
        [ChildActionOnly]
        public ActionResult Menu()
        {
            short? langID;
            if (User.Identity.IsAuthenticated)
            {
                string userID = User.Identity.GetUserId();
                langID = context.Users.Where(x => x.Id == userID).First().Language_Id;
            }
            else
                langID = (short?)(Session["lang"] ?? null);
            if (langID == null)
            {
                langID = context.Languages.Where(x => x.Name == "English").Select(x => x.Id).First();
                Session.Add("lang", langID);
            }
            var staticMenuItems = context.StaticMenuItems.Where(x => x.Language_Id == langID).ToList().Select(x => new { x.Title, x.Id, x.OrderNo, MenuType = "StaticMenu" });
            var dynamicMenuItems = context.DynamicMenuItems.Where(x => x.Language_Id == langID).ToList().Select(x => new { x.Title, x.Id, x.OrderNo, MenuType = "DynamicMenu" });
            var menu = staticMenuItems.Union(dynamicMenuItems).OrderBy(x => x.OrderNo).Select(x => new Models.MenuItem(x.Title, x.Id, x.MenuType));
            ViewBag.MenuItems = menu;
            return PartialView("_PartialMenu");
        }

        [ChildActionOnly]
        public ActionResult Geolocation()
        {
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            IP2Geo.P2GeoSoapClient client = new IP2Geo.P2GeoSoapClient();
            IP2Geo.IPInformation info = client.ResolveIP(ip, "0");
            ViewBag.IPInfo = info;
            return PartialView("_Geolocation");
        }

        [ChildActionOnly]
        public ActionResult Language()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string lang = (Session["lang"] ?? 0).ToString();
            context.Languages.ToList().ForEach(x => list.Add(new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }));
            var temp = list.Where(x => x.Value == lang);
            if (temp.Count() > 0)
            {
                temp.First().Selected = true;
            }
            ViewBag.list = list;
            return PartialView("_Language");
        }

        //[ChildActionOnly]
        //public ActionResult CSS(long? id)
        //{
        //    var temp = context.CSSMappingEntries.Where(x => x.DynamicMenuItem_Id == null || x.DynamicMenuItem_Id == id).ToList();
        //    if (temp.Count == 0)
        //        ViewBag.CSS = string.Empty;
        //    else
        //    {
        //        string result = temp.GroupBy(x => x.CSSMapping.CSSName)
        //                            .Select(x => new
        //                            {
        //                                CSSClass = x.Key,
        //                                CSSValues =
        //                                    x.Select(y => y.CSSMapping.CSSProperty + ":" + y.Value + y.CSSMapping.CSSUnit + ";")
        //                                    .Aggregate((y, z) => y + "\n" + z)
        //                            })
        //                            .Select(x => x.CSSClass + "{" + x.CSSValues + "}")
        //                            .Aggregate((x, y) => x + "\n" + y);
        //        ViewBag.CSS = result;
        //    }
        //    return PartialView("_CSS");
        //}
    }
}