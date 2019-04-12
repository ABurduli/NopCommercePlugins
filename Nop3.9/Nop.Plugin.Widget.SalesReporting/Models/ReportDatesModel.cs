using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widget.SalesReporting.Models
{
    public class ReportDatesModel : BaseNopModel
    {
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
        public bool OnlyHeaders { get; set; }
    }
}
