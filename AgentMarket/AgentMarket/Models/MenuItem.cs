using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgentMarket.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public short Id { get; set; }
        public string MenuType { get; set; }

        public MenuItem(string title, short id, string menuType)
        {
            Title = title;
            Id = id;
            MenuType = menuType;
        }
    }
}