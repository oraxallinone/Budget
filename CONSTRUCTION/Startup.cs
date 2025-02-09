using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CONSTRUCTION.Startup))]
namespace CONSTRUCTION
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
