using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Payments.TBCBankCard.Infrastructure
{
    public class RouteProvider :IRouteProvider
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
            ViewEngines.Engines.Insert(0, new TBCBankCardViewEngine());

            routes.MapRoute("Plugin.Payments.TBCBankCard.PaymentSuccess",
                "Payment/TBCBank/PaymentSuccess",
                new { controller = "TBCBank",action= "PaymentSuccess" },
                new[] { "Nop.Plugin.Payments.TBCBankCard.Controllers" }
                );
            routes.MapRoute("Plugin.Payments.TBCBankCard.PaymentFail",
                "Payment/TBCBank/PaymentFail",
                new { controller = "TBCBank", action = "PaymentFail" },
                new[] { "Nop.Plugin.Payments.TBCBankCard.Controllers" }
                );


        }

       
    }
}
