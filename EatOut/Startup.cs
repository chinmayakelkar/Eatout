using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EatOut.Startup))]
namespace EatOut
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
