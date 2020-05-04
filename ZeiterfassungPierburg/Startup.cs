using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Threading;
using ZeiterfassungPierburg.Data;

[assembly: OwinStartupAttribute(typeof(ZeiterfassungPierburg.Startup))]
namespace ZeiterfassungPierburg
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            ConfigureAuth(app);
        }
    }
}
