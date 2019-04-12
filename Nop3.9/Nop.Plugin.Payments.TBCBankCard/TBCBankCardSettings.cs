using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.TBCBankCard
{
    public class TBCBankCardSettings : ISettings
    {
        public string ServiceURL { get; set; }

        public string RedirectURL { get; set; }

        public string CertificatePath { get; set; }
        public string SecretPass { get; set; }
        public string PaymentDescription { get; set; }
        public int CurrencyCode { get; set; }
        public bool EnableDebugLogging { get; set; }
    }
}
