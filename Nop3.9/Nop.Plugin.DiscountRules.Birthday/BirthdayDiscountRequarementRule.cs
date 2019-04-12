using System;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Common;
using Nop.Services.Customers;

namespace Nop.Plugin.DiscountRules.Birthday
{
    public class BirthdayDiscountRequarementRule : BasePlugin, IDiscountRequirementRule
    {

        private readonly ISettingService _settingService;
        

        public BirthdayDiscountRequarementRule(ISettingService settingService)
        {
            this._settingService = settingService;
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

            var daysDelta = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.BirthdayDelta-{0}", request.DiscountRequirementId));
            if (daysDelta == 0)
                return result;


            var bDay = request.Customer.GetAttribute<DateTime?>(Nop.Core.Domain.Customers.SystemCustomerAttributeNames.DateOfBirth);
            DateTime today = DateTime.Now;
            if (bDay.HasValue)
            {
                DateTime rbDay = (DateTime)bDay;
                DateTime bDayNow = new DateTime(today.Year, rbDay.Month, rbDay.Day);
                if (bDayNow >= today.AddDays(-1 * daysDelta) && bDayNow <= today.AddDays(daysDelta))
                {
                    result.IsValid = true;
                    return result;
                }
            }

            return result;
        }

        public string GetConfigurationUrl(int discountId, int? discountRequirementId)
        {
            //configured in RouteProvider.cs
            string result = "Plugins/DiscountRulesBirthday/Configure/?discountId=" + discountId;
            if (discountRequirementId.HasValue)
                result += string.Format("&discountRequirementId={0}", discountRequirementId.Value);
            return result;
        }

        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.Birthday.Fields.Days", "Days before and after Birthday to enable");
            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.DiscountRules.Birthday.Fields.Days");
            base.Uninstall();
        }
    }
}
