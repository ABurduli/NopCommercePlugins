
using Nop.Core.Configuration;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.SMS.Notifications
{
    public class SMSNotificationsSettings : ISettings, ILocalizedEntity
    {
        public bool Enabled { get; set; }

        public bool EnableOrderPlaced { get; set; }
        public string TemplateOrderPlaced { get; set; }
        public bool EnableOrderCanceled { get; set; }
        public string TemplateOrderCanceled { get; set; }
        public bool EnableOrderPayed { get; set; }
        public string TemplateOrderPayed { get; set; }
        public bool EnableShippingShipped { get; set; }
        public string TemplateShippingShipped { get; set; }

        
        public int SecondLanguageID { get; set; }
        public int ThirdLanguageID { get; set; }

        public string TemplateOrderPlaced2 { get; set; }
        public string TemplateOrderCanceled2 { get; set; }
        public string TemplateOrderPayed2 { get; set; }
        public string TemplateShippingShipped2 { get; set; }

        public string TemplateOrderPlaced3 { get; set; }
        public string TemplateOrderCanceled3 { get; set; }
        public string TemplateOrderPayed3 { get; set; }
        public string TemplateShippingShipped3 { get; set; }
        
        public string IgnnoreUserIDs { get; set; }

        public string SMSServiceURL { get; set; } // username=user&password=pass&recipient={0}&message={1}

    }
}
