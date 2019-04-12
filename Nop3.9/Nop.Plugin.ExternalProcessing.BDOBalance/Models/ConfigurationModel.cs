using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nop.Plugin.ExternalProcessing.BDOBalance.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [DisplayName("BDO Balance Service URL:")]
        public string BalanceServiceURL { get; set; }
        [DisplayName("Balance User:")]
        public string BalanceUserName { get; set; }
        [DisplayName("Balance password:")]
        public string BalanceUserPass { get; set; }
        [DisplayName("Download quantity:")]
        public bool DownloadQuanity { get; set; }
        [DisplayName("Balance Weight unit name:")]
        public string BalanceWeightUnitName { get; set; }
        [DisplayName("Unpublish when lov stock:")]
        public bool UnpublishProductWhenLovStock { get; set; }
        [DisplayName("Upload complete orders:")]
        public bool UploadOrders { get; set; }
    }
}
