using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
namespace Nop.Plugin.DiscountRules.OnsePerNDay.Models
{
    public class RequirementModel
    {
        [NopResourceDisplayName("Plugins.DiscountRules.OnsePerNDay.Fields.Days")]
        public int DaysDelta { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }
       
    }
}
