using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SethWebster.OpenLogging.Startup))]
namespace SethWebster.OpenLogging
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureWebApi(app);
            app.MapSignalR();

        }

    }
}
