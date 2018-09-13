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
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ServiceModel.Syndication;
using AgentMarket.Database.Repositories;

namespace AgentMarket.Controllers
{
   [Authorize(Roles = "Moderator")]
    public class ItemsController : Controller
    {
        private const int THUMBNAIL_WIDTH = 80;
        private const int THUMBNAIL_HEIGHT = 60;

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Items/
        [AllowAnonymous]
        public async Task<ActionResult> Index(short? id)
        {
            if (id == null)
                return View(await db.Items.ToListAsync());
            ViewBag.MenuID = id;
            return View(await db.Items.Where(x => x.DynamicMenuItem_Id == id).ToListAsync());
        }

        // GET: /Items/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [AllowAnonymous]
        public async Task<ActionResult> RSS(long? id)
        {
            List<Item> items;
            if (id == null)
                items = await db.Items.Include(x=>x.DynamicMenuItem).ToListAsync();
            else
                items = await db.Items.Where(x => x.DynamicMenuItem_Id == id).Include(x => x.DynamicMenuItem).ToListAsync();
            string host = Request.Url.GetLeftPart(UriPartial.Authority);
            if (items.Count == 0)
                return RedirectToAction("Index");
            Item temp = items.First();
            SyndicationFeed feed = new SyndicationFeed(temp.DynamicMenuItem.Title,
                            temp.DynamicMenuItem.Title,
                            new Uri(host + "/Items/Index/" + temp.DynamicMenuItem_Id),
                            temp.Title + temp.DynamicMenuItem_Id,
                            DateTime.Now);
            List<SyndicationItem> feedItems = new List<SyndicationItem>();
            foreach (Item i in items)
            {
                SyndicationItem item = new SyndicationItem(i.Title,
                                        i.Description,
                                        new Uri(host + "/Items/Details/" + i.Id),
                                        i.Id.ToString(),
                                        DateTime.Now);
                feedItems.Add(item);
            }
            feed.Items = feedItems;
            return new RssActionResult(feed);
        }

        // GET: /Items/Create
        public ActionResult Create(long? id)
        {
            if (id != null)
                ViewBag.MenuID = id;
            return View();
        }

        // POST: /Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,Image,Title,Description,PostDate,Content,DynamicMenuItem_Id")] AddItemViewModel item)
        {
            //Work to do...
            if (ModelState.IsValid)
            {
                Item temp = new Item { Content = item.Content, Title = item.Title, PostDate = item.PostDate, Description = item.Description, DynamicMenuItem_Id = item.DynamicMenuItem_Id };
                db.Items.Add(temp);
                await db.SaveChangesAsync();
                await Task.Factory.StartNew(() =>
                    {
                        string host = Request.Url.GetLeftPart(UriPartial.Authority);
                        temp.Image = host + "/CMSResources/Images/Item" + temp.Id + ".jpg";
                        temp.Thumbnail = host + "/CMSResources/Images/ItemThumb" + temp.Id + ".jpg";
                        string imagePath = Server.MapPath("~\\CMSResources\\Images\\Item" + temp.Id + ".jpg");
                        string thumbnailPath = Server.MapPath("~\\CMSResources\\Images\\ItemThumb" + temp.Id + ".jpg");
                        using (FileStream fileStream = new FileStream(imagePath, FileMode.CreateNew))
                            item.Image.InputStream.CopyTo(fileStream);
                        using (Bitmap bmp = Bitmap.FromFile(imagePath) as Bitmap)
                        {
                            using (Bitmap newImage = new Bitmap(THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT))
                            {
                                using (Graphics gr = Graphics.FromImage(newImage))
                                {
                                    gr.SmoothingMode = SmoothingMode.HighQuality;
                                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    gr.DrawImage(bmp, new Rectangle(0, 0, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT));
                                }
                                newImage.Save(thumbnailPath);
                            }
                        }
                        db.SaveChanges();
                    });
                return RedirectToAction("Index", new { id = item.DynamicMenuItem_Id });
            }

            return View(item);
        }

        // GET: /Items/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,MenuId,Thumbnail,Image,Title,Description,PostDate,Content,DynamicMenuItem_Id")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: /Items/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            await Task.Factory.StartNew(() =>
                {
                    string imagePath = Server.MapPath("~\\CMSResources\\Images\\Item" + item.Id + ".jpg");
                    string thumbnailPath = Server.MapPath("~\\CMSResources\\Images\\ItemThumb" + item.Id + ".jpg");
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                    if (System.IO.File.Exists(thumbnailPath))
                        System.IO.File.Delete(thumbnailPath);
                });
            return RedirectToAction("Index");
        }

        // Browse Items 
        public ActionResult Browse()
        {
            var ProductRepository = new Repository<Item>(db);
            IEnumerable<Item> BrowseProducts = ProductRepository.GetAll();
            return View(BrowseProducts);

        }
        // Selected Items 
        public ActionResult SelectedProduct(int id)
        {
            var ProductRepository = new Repository<Item>(db);
            var SelectedProduct = ProductRepository.GetById(id);
            return View(SelectedProduct);

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
