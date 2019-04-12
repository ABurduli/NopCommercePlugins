using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceLib
{
    public class StockItem
    {
        public Guid Item { get; set; }
        public Guid Warehouse { get; set; }
        public string Quantity { get; set; }
        public Guid Branch { get; set; }

    }
}
