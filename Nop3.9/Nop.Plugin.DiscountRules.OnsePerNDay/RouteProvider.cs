using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.DiscountRules.OnsePerNDay
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
            routes.MapRoute("Plugin.DiscountRules.OnsePerNDay.Configure",
                "Plugins/DiscountRulesOnsePerNDay/Configure",
                new { controller = "DiscountRulesOnsePerNDay", action = "Configure" },
                new[] { "Nop.Plugin.DiscountRules.OnsePerNDay.Controllers" }
           );
        }
    }
}
