using AgentMarket.Database.Repositories;
using AgentMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;


namespace AgentMarket.Controllers
{
    [RoutePrefix("Dashboard"), Route("{action=index}")]
    public class DashboardController : Controller
    {
        private Repository<Product> rdb = new Repository<Product>(new ApplicationDbContext());
        private Repository<ApplicationUser> udb = new Repository<ApplicationUser>(new ApplicationDbContext());
        private ApplicationDbContext context;
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public DashboardController(ApplicationDbContext context)
        {
            this.context = context;
            var user = HttpContext.User.Identity.Name;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        // GET: Dashboard
        public ActionResult Index(string username)
        {


            var user = UserManager.FindByName(username);

            var model = new DashboardViewModel();
            var currentuser = User.Identity.Name;
             if (currentuser !=null) { 
           
            DateTime RightNow = DateTime.Now;
                model.ProductList = rdb.GetAll().Where(x => x.PostDate < RightNow).ToList();
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