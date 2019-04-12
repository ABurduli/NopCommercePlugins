using System;
using System.Linq;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Core.Domain.Customers;
using Nop.Core;
using Nop.Services.Orders;

namespace Nop.Plugin.DiscountRules.WasOrder
{
    public class WasOrderRule : BasePlugin, IDiscountRequirementRule
    {
        private readonly ISettingService _settingService;

        private readonly IStoreContext _storeContext;
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;

        public WasOrderRule(ISettingService settingService, IOrderService orderService,
                            IWorkContext workContext,
                            IStoreContext storeContext)
        {
            this._settingService = settingService;
            this._orderService = orderService;
            this._workContext = workContext;
            this._storeContext = storeContext;

        }


        public DiscountRequirementValidationResult CheckRequirement(DiscountRequirementValidationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            //invalid by default
            var result = new DiscountRequirementValidationResult();

            if (request.Customer.IsGuest())
                return result;

            int orderCount = 0;
            try
            {
                orderCount = _orderService.SearchOrders(customerId: request.Customer.Id).Where(x => x.OrderStatus == Core.Domain.Orders.OrderStatus.Complete).Count();
            }
            catch { }


            var minOrderCount = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.WasOrder.OrerCount-{0}", request.DiscountRequirementId));

            if (orderCount>= minOrderCount)
            {
                result.IsValid = true;
                return result;
            }

            return result;
        }

        public string GetConfigurationUrl(int discountId, int? discountRequirementId)
        {
            //configured in RouteProvider.cs
            string result = "Plugins/DiscountRulesWasOrder/Configure/?discountId=" + discountId;
            if (discountRequirementId.HasValue)
                result += string.Format("&discountRequirementId={0}", discountRequirementId.Value);
            return result;
        }

        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.WasOrder.Fields.OrderCount", "Has orders");
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.WasOrder.Fields.RegDate.Hint", "Has orders.");

            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.DiscountRules.WasOrder.Fields.OrderCount");
            this.DeletePluginLocaleResource("Plugins.DiscountRules.WasOrder.Fields.RegDate.Hint");

            base.Uninstall();
        }
    }
}
