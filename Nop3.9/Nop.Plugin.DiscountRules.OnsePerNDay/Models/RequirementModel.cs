using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
namespace Nop.Plugin.DiscountRules.OncePerNDay.Models
{
    public class RequirementModel
    {
        [NopResourceDisplayName("Plugins.DiscountRules.OncePerNDay.Fields.Days")]
        public int DaysDelta { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }
       
    }
}
