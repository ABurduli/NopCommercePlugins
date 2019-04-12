using System;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Orders;

namespace Nop.Plugin.DiscountRules.OnsePerNDay
{
    public class OnsePerNDayDiscountRequarementRule : BasePlugin, IDiscountRequirementRule
    {
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        private readonly IDiscountService _discountService;

        public OnsePerNDayDiscountRequarementRule(ISettingService settingService,
                                                    IOrderService orderService,
                                                    IDiscountService discountService)
        {
            this._settingService = settingService;
            this._orderService = orderService;
            this._discountService = discountService;
        }

        public DiscountRequirementValidationResult CheckRequirement(DiscountRequirementValidationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            //invalid by default
            var result = new DiscountRequirementValidationResult();
            result.IsValid = false;

            if (request.Customer == null)
                return result;

            var daysDelta = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.OnsePerNDay-{0}", request.DiscountRequirementId));
            var discountID = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.OnsePerNDayDiscountID-{0}", request.DiscountRequirementId));

            if (discountID == 0)
                return result;
            if (daysDelta == 0)
                return result;

            DateTime minDate = DateTime.Now.AddDays(-1 * daysDelta);
            var customerOrders = _orderService.SearchOrders(customerId: request.Customer.Id).Where(x=>x.CreatedOnUtc>=minDate && x.OrderStatus!= OrderStatus.Cancelled 
                                && x.DiscountUsageHistory.Where(y=>y.DiscountId==discountID).Count()>0).Count();

           if (customerOrders>0)
            {
                result.IsValid = false;
                return result;
            }
            else
            {
                result.IsValid = true;
                return result;
            }
            
        }

        public string GetConfigurationUrl(int discountId, int? discountRequirementId)
        {
            //configured in RouteProvider.cs
            string result = "Plugins/DiscountRulesOnsePerNDay/Configure/?discountId=" + discountId;
            if (discountRequirementId.HasValue)
                result += string.Format("&discountRequirementId={0}", discountRequirementId.Value);
            return result;
        }

        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.OnsePerNDay.Fields.Days", "Days before use this discount next time");
            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.DiscountRules.OnsePerNDay.Fields.Days");
            base.Uninstall();
        }
    }
}
