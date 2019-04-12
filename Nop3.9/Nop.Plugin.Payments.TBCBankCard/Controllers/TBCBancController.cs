using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Core;

using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.TBCBankCard.Controllers
{
    public class TBCBancController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        public TBCBancController(IWorkContext workContext,
            IStoreService storeService,
            ISettingService settingService,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._localizationService = localizationService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var TBCSettings = _settingService.LoadSetting<TBCBankCardSettings>(storeScope);

            var model = new Models.ConfigurationModel(TBCSettings);

            return View("~/Plugins/Payments.TBCBankCard/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(Models.ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var TBCSettings = _settingService.LoadSetting<TBCBankCardSettings>(storeScope);

            //save settings
            TBCSettings.CertificatePath = model.CertificatePath;
            TBCSettings.CurrencyCode = model.CurrencyCode;
            TBCSettings.SecretPass = model.SecretPass;
            TBCSettings.ServiceURL = model.ServiceURL;
            
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.CertificatePath, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.CurrencyCode, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.SecretPass, true, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            var model = new Models.PaymentInfoModel();

            //set postback values
            var form = this.Request.Form;
            model.PurchaseOrderNumber = form["PurchaseOrderNumber"];

            return View("~/Plugins/Payments.PurchaseOrder/Views/PaymentInfo.cshtml", model);
        }



        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            paymentInfo.CustomValues.Add("Order number", form["PurchaseOrderNumber"]);
            return paymentInfo;
        }

        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }
    }
}
