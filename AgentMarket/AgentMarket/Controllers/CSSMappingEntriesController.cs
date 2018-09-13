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
    [Authorize(Roles="Designer, SuperDesigner")]
    public class CSSMappingEntriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /CSSMappingEntries/
        public async Task<ActionResult> Index()
        {
            var cssmappingentries = db.CSSMappingEntries.Include(c => c.CSSMapping).Include(c => c.DynamicMenuItem);
            return View(await cssmappingentries.ToListAsync());
        }

        // GET: /CSSMappingEntries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSMappingEntry cssmappingentry = await db.CSSMappingEntries.FindAsync(id);
            if (cssmappingentry == null)
            {
                return HttpNotFound();
            }
            return View(cssmappingentry);
        }

        // GET: /CSSMappingEntries/Create
        public ActionResult Create()
        {
            ViewBag.CSSMapping_Id = new SelectList(db.CSSMappings, "Id", "PrettyName");
            ViewBag.DynamicMenuItem_Id = new SelectList(db.DynamicMenuItems, "Id", "Title");
            return View();
        }

        // POST: /CSSMappingEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,CSSMapping_Id,Value,DynamicMenuItem_Id")] CSSMappingEntry cssmappingentry)
        {
            if (ModelState.IsValid)
            {
                db.CSSMappingEntries.Add(cssmappingentry);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CSSMapping_Id = new SelectList(db.CSSMappings, "Id", "PrettyName", cssmappingentry.CSSMapping_Id);
            ViewBag.DynamicMenuItem_Id = new SelectList(db.DynamicMenuItems, "Id", "Title", cssmappingentry.DynamicMenuItem_Id);
            return View(cssmappingentry);
        }

        // GET: /CSSMappingEntries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSMappingEntry cssmappingentry = await db.CSSMappingEntries.FindAsync(id);
            if (cssmappingentry == null)
            {
                return HttpNotFound();
            }
            ViewBag.CSSMapping_Id = new SelectList(db.CSSMappings, "Id", "PrettyName", cssmappingentry.CSSMapping_Id);
            ViewBag.DynamicMenuItem_Id = new SelectList(db.DynamicMenuItems, "Id", "Title", cssmappingentry.DynamicMenuItem_Id);
            return View(cssmappingentry);
        }

        // POST: /CSSMappingEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,CSSMapping_Id,Value,DynamicMenuItem_Id")] CSSMappingEntry cssmappingentry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cssmappingentry).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CSSMapping_Id = new SelectList(db.CSSMappings, "Id", "PrettyName", cssmappingentry.CSSMapping_Id);
            ViewBag.DynamicMenuItem_Id = new SelectList(db.DynamicMenuItems, "Id", "Title", cssmappingentry.DynamicMenuItem_Id);
            return View(cssmappingentry);
        }

        // GET: /CSSMappingEntries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSMappingEntry cssmappingentry = await db.CSSMappingEntries.FindAsync(id);
            if (cssmappingentry == null)
            {
                return HttpNotFound();
            }
            return View(cssmappingentry);
        }

        // POST: /CSSMappingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CSSMappingEntry cssmappingentry = await db.CSSMappingEntries.FindAsync(id);
            db.CSSMappingEntries.Remove(cssmappingentry);
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
