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
    [Authorize(Roles="SuperDesigner")]
    public class CSSMappingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /CSSMappings/
        public async Task<ActionResult> Index()
        {
            return View(await db.CSSMappings.ToListAsync());
        }

        // GET: /CSSMappings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSMapping cssmapping = await db.CSSMappings.FindAsync(id);
            if (cssmapping == null)
            {
                return HttpNotFound();
            }
            return View(cssmapping);
        }

        // GET: /CSSMappings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CSSMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,PrettyName,CSSName,CSSProperty,CSSUnit")] CSSMapping cssmapping)
        {
            if (ModelState.IsValid)
            {
                db.CSSMappings.Add(cssmapping);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cssmapping);
        }

        // GET: /CSSMappings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSMapping cssmapping = await db.CSSMappings.FindAsync(id);
            if (cssmapping == null)
            {
                return HttpNotFound();
            }
            return View(cssmapping);
        }

        // POST: /CSSMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,PrettyName,CSSName,CSSProperty,CSSUnit")] CSSMapping cssmapping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cssmapping).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cssmapping);
        }

        // GET: /CSSMappings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSMapping cssmapping = await db.CSSMappings.FindAsync(id);
            if (cssmapping == null)
            {
                return HttpNotFound();
            }
            return View(cssmapping);
        }

        // POST: /CSSMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CSSMapping cssmapping = await db.CSSMappings.FindAsync(id);
            db.CSSMappings.Remove(cssmapping);
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
