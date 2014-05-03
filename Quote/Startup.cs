using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Quote.Startup))]
namespace Quote
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
