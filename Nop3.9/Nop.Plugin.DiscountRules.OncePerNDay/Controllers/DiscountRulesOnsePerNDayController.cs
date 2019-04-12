using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Security;


namespace Nop.Plugin.DiscountRules.OncePerNDay.Controllers
{
    [AdminAuthorize]
    public class DiscountRulesOncePerNDayController : BasePluginController
    {
        private readonly IDiscountService _discountService;
        private readonly ISettingService _settingService;
        private readonly IPermissiOncervice _permissiOncervice;
        private readonly IWorkContext _workContext;
        private readonly ILocalizatiOncervice _localizatiOncervice;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly IProductService _productService;

        public DiscountRulesOncePerNDayController(IDiscountService discountService,
            ISettingService settingService,
            IPermissiOncervice permissiOncervice,
            IWorkContext workContext,
            ILocalizatiOncervice localizatiOncervice,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IStoreService storeService,
            IVendorService vendorService,
            IProductService productService)
        {
            this._discountService = discountService;
            this._settingService = settingService;
            this._permissiOncervice = permissiOncervice;
            this._workContext = workContext;
            this._localizatiOncervice = localizatiOncervice;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._storeService = storeService;
            this._vendorService = vendorService;
            this._productService = productService;
        }


        public ActionResult Configure(int discountId, int? discountRequirementId)
        {

            if (!_permissiOncervice.Authorize(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            if (discountRequirementId.HasValue)
            {
                var discountRequirement = discount.DiscountRequirements.FirstOrDefault(dr => dr.Id == discountRequirementId.Value);
                if (discountRequirement == null)
                    return Content("Failed to load requirement.");
            }

            var daysDelta = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.OncePerNDay-{0}", discountRequirementId.HasValue ? discountRequirementId.Value : 0));
           
            var model = new Models.RequirementModel();
            model.RequirementId = discountRequirementId.HasValue ? discountRequirementId.Value : 0;
            model.DiscountId = discountId;
            model.DaysDelta = daysDelta;

            //add a prefix
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("DiscountRulesOncePerNDay{0}", discountRequirementId.HasValue ? discountRequirementId.Value.ToString() : "0");

            return View("~/Plugins/DiscountRules.OncePerNDay/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public ActionResult Configure(int discountId, int? discountRequirementId, int dayDelta)
        {
            if (!_permissiOncervice.Authorize(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            DiscountRequirement discountRequirement = null;
            if (discountRequirementId.HasValue)
                discountRequirement = discount.DiscountRequirements.FirstOrDefault(dr => dr.Id == discountRequirementId.Value);

            if (discountRequirement != null)
            {
                //update existing rule
                _settingService.SetSetting(string.Format("DiscountRequirement.OncePerNDay-{0}", discountRequirement.Id), dayDelta);
                _settingService.SetSetting(string.Format("DiscountRequirement.OncePerNDayDiscountID-{0}", discountRequirement.Id), discountId);
            }
            else
            {
                //save new rule
                discountRequirement = new DiscountRequirement
                {
                    DiscountRequirementRuleSystemName = "DiscountRequirement.OncePerNDay"
                };
                discount.DiscountRequirements.Add(discountRequirement);
                _discountService.UpdateDiscount(discount);

                _settingService.SetSetting(string.Format("DiscountRequirement.OncePerNDay-{0}", discountRequirement.Id), dayDelta);
                _settingService.SetSetting(string.Format("DiscountRequirement.OncePerNDayDiscountID-{0}", discountRequirement.Id), discountId);
            }
            return Json(new { Result = true, NewRequirementId = discountRequirement.Id }, JsonRequestBehavior.AllowGet);
        }

    }
}
