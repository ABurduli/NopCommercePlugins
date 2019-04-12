using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExternalProcessing.BDOBalance
{
    public class BDOBalanceSettings : ISettings
    {
        public string BalanceServiceURL { get; set; }
        public string BalanceUserName { get; set; }
        public string BalanceUserPass { get; set; }

        public bool DownloadQuanity { get; set; }
        public string BalanceWeightUnitName { get; set; }
        public bool UnpublishProductWhenLovStock { get; set; }
        public bool UploadOrders { get; set; }
    }
}
