using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;

namespace Nop.Plugin.DiscountRules.WasOrder.Models
{
    public class RequirementModel
    {
        [NopResourceDisplayName("Plugins.DiscountRules.WasOrder.Fields.OrderCount")]
        public int OrderCount { get; set; }
        public int DiscountId { get; set; }
        public int RequirementId { get; set; }
        
    }

}