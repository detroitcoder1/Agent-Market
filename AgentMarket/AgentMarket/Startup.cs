﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgentMarket.Startup))]
namespace AgentMarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
