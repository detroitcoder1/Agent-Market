using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgentMarket.Controllers
{
    [Authorize(Roles="SuperModerator")]
    public class UploadController : Controller
    {
        //
        // GET: /Upload/
        public ActionResult Index()
        {
            string path = Request.Url.GetLeftPart(UriPartial.Authority) + "/CMSResources/Uploads/";
            IEnumerable<string> files = Directory.EnumerateFiles(Server.MapPath("~/CMSResources/Uploads")).Select(x => path + Path.GetFileName(x));
            ViewBag.Files = files;
            return View();
        }

        //
        // POST: /Upload/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/CMSResources/Uploads"), fileName);
                    if (System.IO.File.Exists(path))
                        path = Path.Combine(Server.MapPath("~/CMSResources/Uploads"), Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToFileTime() + Path.GetExtension(fileName));
                    file.SaveAs(path);
                }
            }
            catch { }
            return RedirectToAction("Index");
        }

        //
        // POST: /Upload/Delete/5
        [HttpPost]
        public ActionResult Delete(string filename)
        {
            try
            {
                filename = Path.GetFileName(filename);
                filename = Path.Combine(Server.MapPath("~/CMSResources/Uploads"), filename);
                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);
            }
            catch { }
            return RedirectToAction("Index");
        }
    }
}
