using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using ZeiterfassungPierburg.Data;

[assembly: OwinStartupAttribute(typeof(ZeiterfassungPierburg.Startup))]
namespace ZeiterfassungPierburg
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public const string Administrators = "MSI\\pulc";
        public const string Managers= "Pierburg\\CA72438, Pierburg\\CA724396";
        
    }
}
