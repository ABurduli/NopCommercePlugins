using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.DiscountRules.OncePerNDay
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.DiscountRules.OncePerNDay.Configure",
                "Plugins/DiscountRulesOncePerNDay/Configure",
                new { controller = "DiscountRulesOncePerNDay", action = "Configure" },
                new[] { "Nop.Plugin.DiscountRules.OncePerNDay.Controllers" }
           );
        }
    }
}
