using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TBCBankCard.Models
{
    public class PaymentResultModel
    {

        public bool PaymentSucess { get; set; }
        public string trans_id { get; set; }

        public int OrderId { get; set; }
        public string PaymentResultText { get; set; }

        public decimal total { get; set; }
        public decimal tax { get; set; }
        public decimal shipping { get; set; }

        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }

        public List<PaymentResultSubItemModel> items { get; set; }
        public PaymentResultModel()
        {
            items = new List<PaymentResultSubItemModel>();
        }

    }

    public class PaymentResultSubItemModel
    {
        public int OrderID { get; set; }
        public string SKU { get; set; }
        public string Product { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int quantity { get; set; }
    }
}
