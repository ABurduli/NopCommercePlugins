using Nop.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Cms;
using Nop.Web.Framework.Menu;
using System.Web.Routing;

namespace Nop.Plugin.Widget.SalesReporting
{
    public class SalesReportingPlugin : BasePlugin, IAdminMenuPlugin
    {
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var parentNode = new SiteMapNode()
            {
                Visible = true,
                Title = "Reporting"
                //,Url = "/RelatedContent/"

            };

            var categoryNode = new SiteMapNode()
            {
                Visible = true,
                Title = "Sales Detailed",
                Url = "/SalesReporting/AllSales"

            };
            parentNode.ChildNodes.Add(categoryNode);
            rootNode.ChildNodes.Add(parentNode);
        }

        public override void Install()
        {
            base.Install();
        }
        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}
