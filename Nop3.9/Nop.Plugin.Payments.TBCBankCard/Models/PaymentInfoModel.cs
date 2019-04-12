using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.TBCBankCard.Models
{
    public class PaymentInfoModel : BaseNopModel
    {
        [NopResourceDisplayName("Purchase order number")]
        [AllowHtml]
        public string PurchaseOrderNumber { get; set; }
    }
}
