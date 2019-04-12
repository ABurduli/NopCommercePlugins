using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widget.SalesReporting.Models
{
    public class ResultItem
    {
        public int id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? ShippedInMinutes { get; set; }
        public int? DeliveredInMinutes { get; set; }
        public string OrderStatus { get; set; }
        public string ShippingStatus { get; set; }
        public string PaymentMethodSystemName { get; set; }
        public string Customer { get; set; }
        public string ProductName { get; set; }
        public decimal ItemBasePrice { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal OriginalProductCost { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemDiscount { get; set; }
        public decimal ItemSubTotal { get; set; }
        public decimal OrderSubTotal { get; set; }
        public decimal OrderShipping { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderTotal { get; set; }
        public string AuthorizationtransactionID { get; set; }
        public string TBC_CaptureID { get; set; }
        public string UsedDiscounts { get; set; }
        public DateTime? ClientRegistrationDate { get; set; }





    }
}
