using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceLib
{
    public class ClientOrderItem
    {
        public string uid { get; set; }
        public string Date { get; set; }
        public Guid Branch { get; set; }
        public Guid Department { get; set; }
        public Guid Client { get; set; }
        public Guid Agreement { get; set; }
        public string AmountIncludesVAT { get; set; }
        public Guid PriceType { get; set; }
        public Guid Currency { get; set; }
        public decimal CurrencyRate { get; set; }
        public string Comments { get; set; }
        public Guid Warehouse { get; set; }
        public string ComponentsReservation { get; set; }
        public string AutomaticReservation { get; set; }
        //public List<InventoryItem> Inventory { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class InventoryItem
    {
        public string Production { get; set; }
        public string Item { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Reserve { get; set; }
    }

    public class OrderItem
    {
        public Guid Item { get; set; }
        //public Guid Unit { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal Amount { get; set; }
        public Guid VATRate { get; set; }
        public decimal Reserve { get; set; }
        public Guid Specification { get; set; }
    }
}
