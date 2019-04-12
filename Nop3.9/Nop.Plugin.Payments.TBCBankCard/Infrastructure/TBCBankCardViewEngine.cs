using Nop.Web.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TBCBankCard.Infrastructure
{
    public class TBCBankCardViewEngine : ThemeableRazorViewEngine
    {
       public TBCBankCardViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/Payments.TBCBankCard/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/Payments.TBCBankCard/Views/{0}.cshtml" };
        }
    }
}
