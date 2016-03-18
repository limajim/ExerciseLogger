using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExerciseLoggerApp.Startup))]
namespace ExerciseLoggerApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
