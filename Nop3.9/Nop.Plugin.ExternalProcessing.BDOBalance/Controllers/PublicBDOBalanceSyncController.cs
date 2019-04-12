using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Plugin.ExternalProcessing.BDOBalance.Controllers
{
    public class PublicBDOBalanceSyncController : BasePluginController
    {
        public ActionResult PublicInfo()
        {
            return View();
        }

    }
}
