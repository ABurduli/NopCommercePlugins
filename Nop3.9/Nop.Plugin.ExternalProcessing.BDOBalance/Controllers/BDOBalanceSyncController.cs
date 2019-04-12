using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Plugin.ExternalProcessing.BDOBalance.Controllers
{
    public class BDOBalanceSyncController : BasePluginController
    {

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;

        public BDOBalanceSyncController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            IProductService productService,
            IProductTagService productTagService
            )
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
         

        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var bdoSettings = _settingService.LoadSetting<BDOBalanceSettings>(storeScope);
            var model = new Models.ConfigurationModel()
            {
                BalanceServiceURL = bdoSettings.BalanceServiceURL,
                DownloadQuanity = bdoSettings.DownloadQuanity,
                BalanceUserName = bdoSettings.BalanceUserName,
                BalanceUserPass = bdoSettings.BalanceUserPass,
                UnpublishProductWhenLovStock = bdoSettings.UnpublishProductWhenLovStock,
                BalanceWeightUnitName = bdoSettings.BalanceWeightUnitName,
                UploadOrders = bdoSettings.UploadOrders
            };

            return View("~/Plugins/ExternalProcessing.BDOBalance/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(Models.ConfigurationModel model)
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var bdoSettings = _settingService.LoadSetting<BDOBalanceSettings>(storeScope);
            bdoSettings.BalanceServiceURL = model.BalanceServiceURL;
            bdoSettings.DownloadQuanity = model.DownloadQuanity;
            bdoSettings.UploadOrders = model.UploadOrders;
            bdoSettings.UnpublishProductWhenLovStock = model.UnpublishProductWhenLovStock;
            bdoSettings.BalanceUserName = model.BalanceUserName;
            bdoSettings.BalanceUserPass = model.BalanceUserPass;
            bdoSettings.BalanceWeightUnitName = model.BalanceWeightUnitName;

            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.BalanceServiceURL, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.DownloadQuanity, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.UploadOrders, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.UnpublishProductWhenLovStock, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.BalanceUserName, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.BalanceUserPass, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bdoSettings, x => x.BalanceWeightUnitName, true, storeScope, false);


            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
    }
}
