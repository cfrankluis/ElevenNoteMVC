using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElevenNoteMVC.Startup))]
namespace ElevenNoteMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
