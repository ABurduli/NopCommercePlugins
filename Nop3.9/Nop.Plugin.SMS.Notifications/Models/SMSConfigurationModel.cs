using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Notifications.Models
{
    public class SMSConfigurationModel
    {
        

        [NopResourceDisplayName("Plugins.Sms.Fields.Enabled")]
        public bool Enabled { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.EnableOrderPlaced")]
        public bool EnableOrderPlaced { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.TemplateOrderPlaced")]
        public string TemplateOrderPlaced { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.EnableOrderCanceled")]
        public bool EnableOrderCanceled { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.TemplateOrderCanceled")]
        public string TemplateOrderCanceled { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.EnableOrderPayed")]
        public bool EnableOrderPayed { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.TemplateOrderPayed")]
        public string TemplateOrderPayed { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.EnableShippingShipped")]
        public bool EnableShippingShipped { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.TemplateShippingShipped")]
        public string TemplateShippingShipped { get; set; }
        [NopResourceDisplayName("Plugins.Sms.Fields.SMSServiceURL")]
        public string SMSServiceURL { get; set; }
        [NopResourceDisplayName("Test Phone Number")]
        public string TestPhoneNumber { get; set; }

        [NopResourceDisplayName("Second language ID")]
        public int SecondLanguageID { get; set; }
        [NopResourceDisplayName("Third language ID")]
        public int ThirdLanguageID { get; set; }

        [NopResourceDisplayName("Template order placed. Second Language")]
        public string TemplateOrderPlaced2 { get; set; }
        [NopResourceDisplayName("Template order canceled. Second Language")]
        public string TemplateOrderCanceled2 { get; set; }
        [NopResourceDisplayName("Template order payed. Second Language")]
        public string TemplateOrderPayed2 { get; set; }
        [NopResourceDisplayName("Template order shipped. Second Language")]
        public string TemplateShippingShipped2 { get; set; }

        [NopResourceDisplayName("Template order placed. Third Language")]
        public string TemplateOrderPlaced3 { get; set; }
        [NopResourceDisplayName("Template order canceled. Third Language")]
        public string TemplateOrderCanceled3 { get; set; }
        [NopResourceDisplayName("Template order payed. Third Language")]
        public string TemplateOrderPayed3 { get; set; }
        [NopResourceDisplayName("Template order shipped. Third Language")]
        public string TemplateShippingShipped3 { get; set; }
        [NopResourceDisplayName("Black list user id's")]
        public string IgnnoreUserIDs { get; set; }


    }


}
