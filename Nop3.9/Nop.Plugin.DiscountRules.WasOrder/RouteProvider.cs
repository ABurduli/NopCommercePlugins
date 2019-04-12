using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.DiscountRules.WasOrder
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.DiscountRules.WasOrder.Configure",
                  "Plugins/DiscountRulesWasOrder/Configure",
                  new { controller = "DiscountRulesWasOrder", action = "Configure" },
                  new[] { "Nop.Plugin.DiscountRules.WasOrder.Controllers" }
             );
        }
    }
}
