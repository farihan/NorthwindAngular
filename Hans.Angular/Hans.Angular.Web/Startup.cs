using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hans.Angular.Web.Startup))]
namespace Hans.Angular.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
