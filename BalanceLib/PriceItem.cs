using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceLib
{
    public class PriceItem
    {
        public Guid PriceType { get; set; }
        public Guid Item { get; set; }
        public Guid Currency { get; set; }
        public string Price { get; set; }

    }
}
