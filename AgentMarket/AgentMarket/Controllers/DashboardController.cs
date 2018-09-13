using AgentMarket.Database.Repositories;
using AgentMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgentMarket.Controllers
{
    [RoutePrefix("Dashboard"), Route("{action=index}")]
    public class DashboardController : Controller
    {
        private Repository<Product> rdb = new Repository<Product>(new ApplicationDbContext());
        private Repository<ApplicationUser> udb = new Repository<ApplicationUser>(new ApplicationDbContext());

        // GET: Dashboard
        public ActionResult Index()
        {
            var model = new DashboardViewModel();
            var currentuser = User.Identity;
             if (currentuser !=null) { 
           
            DateTime RightNow = DateTime.Now;

            model.ProductList = rdb.GetAll().Where(x => x.PostDate < RightNow).ToList();
            model.ActiveOrder = rdb.GetAll().Where(x => x.IsActive == true).Count();           
            model.ProductCount = rdb.GetAll().Where(x => x.IsActive == true).Count();
            model.Users = udb.GetAll().Count();
            model.UserList = udb.GetAll().Take(5).OrderBy(x => x.Id).ToList();

            }


            return View(model);
        }
    }
}