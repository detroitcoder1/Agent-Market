using AgentMarket.Database.Repositories;
using AgentMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgentMarket.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var ProductRepository = new Repository<Product>(db);

            IEnumerable<Product> orderedProducts = ProductRepository
                                                    .GetAll()
                                                    .Where(c => c.Id > 0)
                                                    .OrderBy(h => h.ProductName);

            List<Product> list_course = orderedProducts.ToList();


            //return View();
            return View(orderedProducts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Support()
        {
            return View();
        }
    }
}