using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.DiscountRules.Birthday
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
            routes.MapRoute("Plugin.DiscountRules.Birthday.Configure",
                 "Plugins/DiscountRulesBirthday/Configure",
                 new { controller = "DiscountRulesBirthday", action = "Configure" },
                 new[] { "Nop.Plugin.DiscountRules.Birthday.Controllers" }
            );
        }
    }
}
