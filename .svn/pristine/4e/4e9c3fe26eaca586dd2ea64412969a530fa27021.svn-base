using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.DataProtection;

[assembly: OwinStartup(typeof(TourTravels.API.Startup))]

namespace TourTravels.API
{
    public partial class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            DataProtectionProvider = app.GetDataProtectionProvider();
        }
    }
}
