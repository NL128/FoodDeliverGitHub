using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FoodDelivery.Startup))]
namespace FoodDelivery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
