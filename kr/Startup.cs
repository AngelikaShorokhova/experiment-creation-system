using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(kr.Startup))]
namespace kr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
