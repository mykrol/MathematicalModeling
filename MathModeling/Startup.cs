using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MathModeling.Startup))]
namespace MathModeling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
