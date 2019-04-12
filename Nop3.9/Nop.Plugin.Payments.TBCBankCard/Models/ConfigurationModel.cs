using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.TBCBankCard.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Payment Service URL")]
        public string ServiceURL { get; set; }
        [NopResourceDisplayName("Certificate path")]
        public string CertificatePath { get; set; }
        [NopResourceDisplayName("Secred word")]
        public string SecretPass { get; set; }

        [NopResourceDisplayName("Payment description")]
        public string PaymentDescription { get; set; }
        [NopResourceDisplayName("Currency code")]
        public int CurrencyCode { get; set; }
        [NopResourceDisplayName("Enable debug logging")]
        public bool EnableDebugLogging { get; set; }
        [NopResourceDisplayName("Bank Redirect URL")]
        public string RedirectURL { get; set; }

        public ConfigurationModel()
        {

        }

        public ConfigurationModel(TBCBankCardSettings v)
        {
            ServiceURL = v.ServiceURL;
            CertificatePath = v.CertificatePath;
            SecretPass = v.SecretPass;
            CurrencyCode = v.CurrencyCode;
            EnableDebugLogging = v.EnableDebugLogging;
            PaymentDescription = v.PaymentDescription;
            RedirectURL = v.RedirectURL;
        }
    }
}
