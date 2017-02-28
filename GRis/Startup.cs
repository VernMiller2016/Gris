using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GRis.Startup))]
namespace GRis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
