using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgentMarket.Models
{
    public class DashboardViewModel
    {
        public int ActiveOrder { get; set; }
        public int ClosedOrderPercent { get; set; }
        public int CustomerCount { get; set; }
        public int ProductCount { get; set; }
        public int Users { get; set; }
        public List<Product> ProductList { get; set; }
        public List<ApplicationUser> UserList { get; set; }

    }
}