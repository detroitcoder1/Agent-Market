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
using System.Data.Linq;

namespace AgentMarket.Controllers
{
    public class ProductsController : Controller
    {
         
        private const int THUMBNAIL_WIDTH = 80;
        private const int THUMBNAIL_HEIGHT = 60;

        private ApplicationDbContext db = new ApplicationDbContext();
        private Repository<Product> rdb = new Repository<Product>(new ApplicationDbContext());

        // GET: /Items/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {


            return View(rdb.GetAll().ToList());
        }

        // GET: /Items/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int id)
        {
            Product product = rdb.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [AllowAnonymous]
        public async Task<ActionResult> RSS(int id)
        {
            List<Product> products;

            products = await rdb.GetAll().Where(x => x.DynamicMenuItem_Id == id).Include(x => x.DynamicMenuItem).ToListAsync();
            string host = Request.Url.GetLeftPart(UriPartial.Authority);
            if (products.Count == 0)
                return RedirectToAction("Index");
            Product temp = products.First();
            SyndicationFeed feed = new SyndicationFeed(temp.DynamicMenuItem.Title,
                            temp.DynamicMenuItem.Title,
                            new Uri(host + "/Items/Index/" + temp.DynamicMenuItem_Id),
                            temp.ProductName + temp.DynamicMenuItem_Id,
                            DateTime.Now);
            List<SyndicationItem> feedproducts = new List<SyndicationItem>();
            foreach (Product p in products)
            {
                SyndicationItem product = new SyndicationItem(p.ProductName,
                                        p.MainDescription,
                                        new Uri(host + "/products/Details/" + p.Id),
                                        p.Id.ToString(),
                                        DateTime.Now);
                feedproducts.Add(product);
            }
            feed.Items = feedproducts;
            return new RssActionResult(feed);
        }

        // GET: /Products/Create
        public ActionResult Create()
        {
            //if (id != null)
            //    ViewBag.MenuID = id;
            return View();
        }

        // POST: /Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,Image,Image2,Image3,FeaturedDeal,IsActive,ProductName,MainDescription,DescriptionHook,BulletPoint1,BulletPoint2,BulletPoint3,BulletPoint4,BulletPoint5,BulletPoint6,DescriptionHook1,DescriptionHook2,DescriptionLong,OriginalPrice,SalePrice,DynamicMenuItem_Id")] AddProductViewModel Product)
        {
            //Work to do...
            if (ModelState.IsValid)
            {
                Product temp = new Product { ProductName = Product.ProductName,FeaturedDeal= Product.FeaturedDeal, MainDescription=Product.MainDescription, DescriptionHook1= Product.DescriptionHook1, DescriptionHook2 = Product.DescriptionHook2, DescriptionLong = Product.DescriptionLong, BulletPoint1 = Product.BulletPoint1,BulletPoint2=Product.BulletPoint2,BulletPoint3= Product.BulletPoint3, BulletPoint4 = Product.BulletPoint4, BulletPoint5 = Product.BulletPoint5, BulletPoint6 = Product.BulletPoint6, IsActive = Product.IsActive, OriginalPrice= Product.OriginalPrice, PostDate=Product.PostDate, SalePrice = Product.SalePrice, DynamicMenuItem_Id = Product.DynamicMenuItem_Id };
                rdb.Insert(temp);
                await db.SaveChangesAsync();
                await Task.Factory.StartNew(() =>
                {
                    string host = Request.Url.GetLeftPart(UriPartial.Authority);
                    temp.Image = host + "/CMSResources/Images/Item" + temp.Id + ".jpg";
                    temp.Thumbnail = host + "/CMSResources/Images/ItemThumb" + temp.Id + ".jpg";
                    string imagePath = Server.MapPath("~\\CMSResources\\Images\\Item" + temp.Id + ".jpg");
                    string thumbnailPath = Server.MapPath("~\\CMSResources\\Images\\ItemThumb" + temp.Id + ".jpg");
                    using (FileStream fileStream = new FileStream(imagePath, FileMode.CreateNew))
                        Product.Image.InputStream.CopyTo(fileStream);
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
                return RedirectToAction("Index", new { id = Product.DynamicMenuItem_Id });
            }

            return View(Product);
        }

        // GET: /Items/Edit/5
        public ActionResult Edit(int id)
        {

            Product product = rdb.GetById(id);

            return View(product);
        }

        // POST: /Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Image,Image2,Image3,FeaturedDeal,IsActive,ProductName,MainDescription,DescriptionHook,BulletPoint1,BulletPoint2,BulletPoint3,BulletPoint4,BulletPoint5,BulletPoint6,DescriptionHook1,DescriptionHook2,DescriptionLong,OriginalPrice,SalePrice,DynamicMenuItem_Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /Items/Delete/5
        public ActionResult Delete(int id)
        {

            Product product = rdb.GetById(id);

            return View(product);
        }

        // POST: /Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = rdb.GetById(id);
            rdb.Delete(product);
            await db.SaveChangesAsync();
            await Task.Factory.StartNew(() =>
            {
                string imagePath = Server.MapPath("~\\AgentResources\\Images\\Item" + product.Id + ".jpg");
                string thumbnailPath = Server.MapPath("~\\AgentResources\\Images\\ItemThumb" + product.Id + ".jpg");
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
            IEnumerable<Product> BrowseProducts = rdb.GetAll();
            return View(BrowseProducts);

        }
        // Selected Items 
        public ActionResult SelectedProduct(int id)
        {
            
            var SelectedProduct = rdb.GetById(id);
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
