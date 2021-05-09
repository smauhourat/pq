using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductQuoteApp.Startup))]
namespace ProductQuoteApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
