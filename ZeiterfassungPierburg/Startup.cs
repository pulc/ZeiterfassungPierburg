using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZeiterfassungPierburg.Startup))]
namespace ZeiterfassungPierburg
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
