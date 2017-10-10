using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComercioE.Startup))]
namespace ComercioE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
