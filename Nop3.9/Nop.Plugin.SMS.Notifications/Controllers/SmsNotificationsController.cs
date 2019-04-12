using System;
using System.Web.Mvc;
using Nop.Core.Plugins;
using Nop.Plugin.SMS.Notifications.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.SMS.Notifications.Controllers
{
    [AdminAuthorize]
    public class SmsNotificationsController : BasePluginController
    {
        private readonly SMSNotificationsSettings _smsSettings;
        private readonly ISettingService _settingService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public SmsNotificationsController(SMSNotificationsSettings smsSettings,
            ISettingService settingService,
            IPluginFinder pluginFinder,
            ILanguageService languageService,
            ILocalizationService localizationService)
        {
            this._smsSettings = smsSettings;
            this._settingService = settingService;
            this._pluginFinder = pluginFinder;
            this._localizationService = localizationService;
            this._languageService = languageService;
        }

        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new Models.SMSConfigurationModel()
            {
                Enabled = _smsSettings.Enabled,
                EnableOrderCanceled = _smsSettings.EnableOrderCanceled,
                EnableOrderPayed = _smsSettings.EnableOrderPayed,
                EnableOrderPlaced = _smsSettings.EnableOrderPlaced,
                EnableShippingShipped = _smsSettings.EnableShippingShipped,
                SMSServiceURL = _smsSettings.SMSServiceURL,
                TemplateOrderCanceled = _smsSettings.TemplateOrderCanceled,
                TemplateOrderPayed = _smsSettings.TemplateOrderPayed,
                TemplateOrderPlaced = _smsSettings.TemplateOrderPlaced,
                TemplateShippingShipped = _smsSettings.TemplateShippingShipped,
                SecondLanguageID = _smsSettings.SecondLanguageID,
                TemplateOrderCanceled2 = _smsSettings.TemplateOrderCanceled2,
                TemplateOrderCanceled3 = _smsSettings.TemplateOrderCanceled3,
                TemplateOrderPayed2 = _smsSettings.TemplateOrderPayed2,
                TemplateOrderPayed3 = _smsSettings.TemplateOrderPayed3,
                TemplateOrderPlaced2 = _smsSettings.TemplateOrderPlaced2,
                TemplateOrderPlaced3 = _smsSettings.TemplateOrderPlaced3,
                TemplateShippingShipped2 = _smsSettings.TemplateShippingShipped2,
                TemplateShippingShipped3 = _smsSettings.TemplateShippingShipped3,
                ThirdLanguageID = _smsSettings.ThirdLanguageID,
                IgnnoreUserIDs = _smsSettings.IgnnoreUserIDs

            };

            return View("~/Plugins/SMS.Notifications/Views/Configure.cshtml", model);
        }

        [ChildActionOnly]
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public ActionResult ConfigurePOST(SMSConfigurationModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            //save settings
            _smsSettings.Enabled = model.Enabled;
            _smsSettings.EnableOrderCanceled = model.EnableOrderCanceled;
            _smsSettings.EnableOrderPayed = model.EnableOrderPayed;
            _smsSettings.EnableOrderPlaced = model.EnableOrderPlaced;
            _smsSettings.EnableShippingShipped = model.EnableShippingShipped;
            _smsSettings.SMSServiceURL = model.SMSServiceURL;
            _smsSettings.TemplateOrderCanceled = model.TemplateOrderCanceled;
            _smsSettings.TemplateOrderPayed = model.TemplateOrderPayed;
            _smsSettings.TemplateOrderPlaced = model.TemplateOrderPlaced;
            _smsSettings.TemplateShippingShipped = model.TemplateShippingShipped;

            _smsSettings.SecondLanguageID = model.SecondLanguageID;
            _smsSettings.TemplateOrderCanceled2 = model.TemplateOrderCanceled2;
            _smsSettings.TemplateOrderCanceled3 = model.TemplateOrderCanceled3;
            _smsSettings.TemplateOrderPayed2 = model.TemplateOrderPayed2;
            _smsSettings.TemplateOrderPayed3 = model.TemplateOrderPayed3;
            _smsSettings.TemplateOrderPlaced2 = model.TemplateOrderPlaced2;
            _smsSettings.TemplateOrderPlaced3 = model.TemplateOrderPlaced3;
            _smsSettings.TemplateShippingShipped2 = model.TemplateShippingShipped2;
            _smsSettings.TemplateShippingShipped3 = model.TemplateShippingShipped3;
            _smsSettings.ThirdLanguageID = model.ThirdLanguageID;
            _smsSettings.IgnnoreUserIDs = model.IgnnoreUserIDs;

            _settingService.SaveSetting(_smsSettings);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [ChildActionOnly]
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("test-sms")]
        public ActionResult TestSms(SMSConfigurationModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.TestPhoneNumber))
                {
                    ErrorNotification("Enter test message");
                }
                else
                {
                    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("SMS.Notifications");
                    if (pluginDescriptor == null)
                        throw new Exception("Cannot load the plugin");
                    var plugin = pluginDescriptor.Instance() as SMSNotificationsProvider;
                    if (plugin == null)
                        throw new Exception("Cannot load the plugin");
                    string resText = "";
                    if (!plugin.SendSms(model.TestPhoneNumber,"This is a test SMS message",out resText))
                    {
                        ErrorNotification("Test Failed. Error "+resText);
                    }
                    else
                    {
                        SuccessNotification("Test Success");
                    }
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.ToString());
            }

            return View("~/Plugins/SMS.Notifications/Views/Configure.cshtml", model);
        }
    }
}
