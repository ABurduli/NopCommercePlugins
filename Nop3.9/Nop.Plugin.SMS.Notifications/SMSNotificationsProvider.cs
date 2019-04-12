using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;

namespace Nop.Plugin.SMS.Notifications
{
    public class SMSNotificationsProvider : BasePlugin, IMiscPlugin
    {
        private readonly SMSNotificationsSettings _smsSettings;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
       
        public SMSNotificationsProvider(SMSNotificationsSettings smsSettings,
            ILogger logger,
            ISettingService settingService,
            IStoreContext storeContext
            )
        {
            this._smsSettings = smsSettings;
            this._logger = logger;
            this._settingService = settingService;
            this._storeContext = storeContext;
        }

        public bool SendSms(string phone,string text,out string resText)
        {

            string url = _smsSettings.SMSServiceURL;
            try
            {
                url = string.Format(url, phone, HttpUtility.UrlEncode(text));
                string res = SendHttpRequest(url);
                if (res.ToUpper().StartsWith("OK"))
                {
                    resText = "OK";
                    return true;
                }
                else
                {
                    _logger.Error("SMS Send Error: " + res);
                    resText = res;
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SMS Send Error: "+ex.Message, ex);
                resText = ex.Message;
                return false;
            }
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "SmsNotifications";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.SMS.Notifications.Controllers" }, { "area", null } };

        }

        public override void Install()
        {
            //settings
            var settings = new SMSNotificationsSettings
            {
                Enabled = false,
                EnableOrderCanceled = false,
                EnableOrderPayed = false,
                EnableOrderPlaced = false,
                EnableShippingShipped = false,
                SMSServiceURL = "http://smsco.ge/api/sendsms.php?username=test&password=test&recipient={0}&message={1}",
                TemplateOrderCanceled = "Your order #{0} canceled", // {0} web shop name
                TemplateOrderPayed = "Your order #{0} is payed",
                TemplateOrderPlaced = "Your order #{0}, amount {1}  is received",
                TemplateShippingShipped = "Your order #{0} is shipped"
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.Enabled", "Enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.Enabled.Hint", "Check to enable SMS provider");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableOrderCanceled", "Order Cancelaion enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableOrderCanceled.Hint", "Order Cancelaion message enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPayed", "Order payed enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPayed.Hint", "Order payed message enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPlaced", "Order placed enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPlaced.Hint", "Order placed message enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableShippingShipped", "Order shipped enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.EnableShippingShipped.Hint", "Order shipped message enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.SMSServiceURL", "SMS Provider url");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.SMSServiceURL.Hint", "SMS Provider URL 'username=user&password=pass&recipient={0}&message={1}'");

            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderCanceled", "Canceled template");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderCanceled.Hint", "Order canceled message tempalte {0} order id, {1} Amount");

            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPayed", "Payed tmplate");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPayed.Hint", "Order payed message tempalte {0} order id, {1} Amount");

            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPlaced", "Placed tmplate");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPlaced.Hint", "Order Placed message tempalte {0} order id, {1} Amount");

            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateShippingShipped", "Shipped tmplate");
            this.AddOrUpdatePluginLocaleResource("Plugins.Sms.Fields.TemplateShippingShipped.Hint", "Sipped Placed message tempalte {0} order id, {1} Amount");


            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<SMSNotificationsSettings>();

            this.DeletePluginLocaleResource("Plugins.Sms.Fields.Enabled");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.Enabled.Hint");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableOrderCanceled");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableOrderCanceled.Hint");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPayed");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPayed.Hint");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPlaced");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableOrderPlaced.Hint");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableShippingShipped");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.EnableShippingShipped.Hint");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.SMSServiceURL");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.SMSServiceURL.Hint");

            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderCanceled");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderCanceled.Hint");

            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPayed");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPayed.Hint");

            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPlaced");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateOrderPlaced.Hint");

            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateShippingShipped");
            this.DeletePluginLocaleResource("Plugins.Sms.Fields.TemplateShippingShipped.Hint");

            base.Uninstall();
        }

        string SendHttpRequest(string requeststring)
        {
            string _respstring = null;
            
            HttpWebRequest _webreq = (HttpWebRequest)WebRequest.Create(requeststring);
            
            ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(delegate (object sender,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                }
            );


            HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
            using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                _respstring = _reader.ReadToEnd();
            }
            return _respstring;
        }
    }
}
