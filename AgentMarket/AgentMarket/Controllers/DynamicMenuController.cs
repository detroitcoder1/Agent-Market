using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AgentMarket.Models;

namespace AgentMarket.Controllers
{
    [Authorize(Roles = "SuperModerator")]
    public class DynamicMenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /DynamicMenu/
        public async Task<ActionResult> Index()
        {
            var dynamicmenuitems = db.DynamicMenuItems.Include(d => d.Language);
            return View(await dynamicmenuitems.ToListAsync());
        }

        // GET: /DynamicMenu/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicMenuItem dynamicmenuitem = await db.DynamicMenuItems.FindAsync(id);
            if (dynamicmenuitem == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Products", "Index", new { id = dynamicmenuitem.Id });
            //return View(dynamicmenuitem);
        }

        // GET: /DynamicMenu/Create
        public ActionResult Create()
        {
            ViewBag.Languages = new SelectList(db.Languages, "Id", "Name");
            return View();
        }

        // POST: /DynamicMenu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Title,OrderNo,Language_Id")] DynamicMenuItem dynamicmenuitem)
        {
            if (ModelState.IsValid)
            {
                db.DynamicMenuItems.Add(dynamicmenuitem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", dynamicmenuitem.Language_Id);
            return View(dynamicmenuitem);
        }

        // GET: /DynamicMenu/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicMenuItem dynamicmenuitem = await db.DynamicMenuItems.FindAsync(id);
            if (dynamicmenuitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", dynamicmenuitem.Language_Id);
            return View(dynamicmenuitem);
        }

        // POST: /DynamicMenu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Title,OrderNo,Language_Id")] DynamicMenuItem dynamicmenuitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dynamicmenuitem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", dynamicmenuitem.Language_Id);
            return View(dynamicmenuitem);
        }

        // GET: /DynamicMenu/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DynamicMenuItem dynamicmenuitem = await db.DynamicMenuItems.FindAsync(id);
            if (dynamicmenuitem == null)
            {
                return HttpNotFound();
            }
            return View(dynamicmenuitem);
        }

        // POST: /DynamicMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            DynamicMenuItem dynamicmenuitem = await db.DynamicMenuItems.FindAsync(id);
            db.DynamicMenuItems.Remove(dynamicmenuitem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
