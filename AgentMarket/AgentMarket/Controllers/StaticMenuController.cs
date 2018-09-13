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
    [Authorize(Roles="SuperModerator")]
    public class StaticMenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /StaticMenu/
        public async Task<ActionResult> Index()
        {
            var staticmenuitems = db.StaticMenuItems.Include(s => s.Language);
            return View(await staticmenuitems.ToListAsync());
        }

        // GET: /StaticMenu/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticMenuItem staticmenuitem = await db.StaticMenuItems.FindAsync(id);
            if (staticmenuitem == null)
            {
                return HttpNotFound();
            }
            return View(staticmenuitem);
        }

        // GET: /StaticMenu/Create
        public ActionResult Create()
        {
            ViewBag.Languages = new SelectList(db.Languages.ToList(), "Id", "Name");
            return View();
        }

        // POST: /StaticMenu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include="Id,Title,Content,OrderNo,Language_Id")] StaticMenuItem staticmenuitem)
        {
            if (ModelState.IsValid)
            {
                db.StaticMenuItems.Add(staticmenuitem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", staticmenuitem.Language_Id);
            return View(staticmenuitem);
        }

        // GET: /StaticMenu/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticMenuItem staticmenuitem = await db.StaticMenuItems.FindAsync(id);
            if (staticmenuitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", staticmenuitem.Language_Id);
            return View(staticmenuitem);
        }

        // POST: /StaticMenu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Title,Content,OrderNo,Language_Id")] StaticMenuItem staticmenuitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staticmenuitem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", staticmenuitem.Language_Id);
            return View(staticmenuitem);
        }

        // GET: /StaticMenu/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticMenuItem staticmenuitem = await db.StaticMenuItems.FindAsync(id);
            if (staticmenuitem == null)
            {
                return HttpNotFound();
            }
            return View(staticmenuitem);
        }

        // POST: /StaticMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            StaticMenuItem staticmenuitem = await db.StaticMenuItems.FindAsync(id);
            db.StaticMenuItems.Remove(staticmenuitem);
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
