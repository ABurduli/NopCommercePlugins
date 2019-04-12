using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Core;

using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Services.Orders;
using Nop.Services.Logging;

namespace Nop.Plugin.Payments.TBCBankCard.Controllers
{
    public class TBCBankController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;
        private readonly TBCBankCardSettings _TbcPaymentSettings;


        public TBCBankController(IWorkContext workContext,
            IStoreService storeService,
            ISettingService settingService,
            ILocalizationService localizationService,
            IOrderService orderService,
            ILogger logger,
            TBCBankCardSettings tbcPaymentSettings)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._orderService = orderService;
            this._logger = logger;
            this._TbcPaymentSettings = tbcPaymentSettings;
        }

        public ActionResult GetSopingCardTotal()
        {
            var ShoppingFactory = Nop.Core.Infrastructure.EngineContext.Current.Resolve<Nop.Web.Factories.IShoppingCartModelFactory>();
            var card = ShoppingFactory.PrepareMiniShoppingCartModel();
            string subTotal = "";
            if (card != null)
                subTotal = card.SubTotal;

            return Json(new
            {
                success = true,
                message = "Updated",
                updatetopcartAmount = subTotal
            });
        }
        
        public ActionResult PaymentSuccess(string trans_id="")
        {
            Models.PaymentResultModel model = new Models.PaymentResultModel();
            string trID = trans_id;
            model.trans_id = trID;

            var order = _orderService.GetOrderByAuthorizationTransactionIdAndPaymentMethod(trID, "Payments.TBCBankCard");

            if (order ==null)
            {
                // Incorect request from bank
                _logger.Error($"Incorrect trans_id = {trans_id} from Bank. order not found.");
                model.PaymentResultText = "Error process payment";
                model.PaymentSucess = false;
                return View(model);
            }
            model.OrderId = order.Id;
            try
            {
                model.total = order.OrderTotal;
                model.tax = order.OrderTax;
                model.shipping = order.OrderShippingInclTax;
            }
            catch { }
            try
            {
                model.city = order.ShippingAddress.City;
                model.country = order.ShippingAddress.Country.Name;
                model.region = order.ShippingAddress.StateProvince.Name;
            }
            catch
            {
                model.city = "Tbilisi";
                model.country = "Georgia";
                model.region = "GE";

            }
            foreach (var i in order.OrderItems)
            {
                try
                {
                    model.items.Add(new Models.PaymentResultSubItemModel()
                    {
                        OrderID = order.Id,
                        Category = i.Product.ProductCategories.ToString(),
                        Product = i.Product.Name,
                        quantity = i.Quantity,
                        SKU = i.Product.Sku,
                        UnitPrice = i.Product.Price
                    });
                }
                catch { }
            }


            // get Result Frm Bank
            string lang = _workContext.WorkingLanguage.UniqueSeoCode;
            string url = _TbcPaymentSettings.ServiceURL;
            Debug($"Payment URL = {url}");
            string certPath = $@"{HttpContext.Request.PhysicalApplicationPath}Plugins\Payments.TBCBankCard\KeyStore\{_TbcPaymentSettings.CertificatePath}";
            Code.Merchant merchant = new Code.Merchant(certPath, _TbcPaymentSettings.SecretPass, url, 30000);
            string bankPaymentResult = "";
            Code.StatusResult CheckResult = null;
            try
            {
                bankPaymentResult = merchant.GetTransResult(new Code.CommandParams(lang) { trans_id = trans_id });
                
                CheckResult = new Code.StatusResult(bankPaymentResult);

                if (CheckResult.RESULT == "OK")
                {
                    order.PaymentStatus = Core.Domain.Payments.PaymentStatus.Authorized;
                    order.AuthorizationTransactionCode = CheckResult.RESULT;
                    _orderService.UpdateOrder(order);
                    Debug($"Check result: {CheckResult.RESULT}. Code: {CheckResult.RESULT_CODE}");
                    model.PaymentResultText = CheckResult.GetResultMSG(lang);// "Payment sucessful";
                    model.PaymentSucess = true;
                    return View(model);
                }
                else
                {
                    order.AuthorizationTransactionCode = bankPaymentResult;
                    _orderService.UpdateOrder(order);

                    model.PaymentResultText = CheckResult.GetResultMSG(lang);//"Payment error.";
                    model.PaymentSucess = false;
                    return View(model);

                }
            }
            catch (Exception ex)
            {
                _logger.Error($"TBC Paysucces error check transaction :{ex.Message}");
                model.PaymentResultText = "Error check transaction.";
                model.PaymentSucess = false;
                return View(model);

            }
            


        }

        
        public ActionResult PaymentFail(string trans_id = "")
        {
            Models.PaymentResultModel model = new Models.PaymentResultModel();
            string trID = trans_id;
            model.trans_id = trID;

            var order = _orderService.GetOrderByAuthorizationTransactionIdAndPaymentMethod(trID, "Payments.TBCBankCard");

            if (order == null)
            {
                // Incorect request from bank
                _logger.Error($"Incorrect trans_id = {trans_id} from Bank. order not found.");
                model.PaymentResultText = "Error process payment";
                model.PaymentSucess = false;
                return View(model);
            }
            model.OrderId = order.Id;
            model.PaymentResultText = "Error process payment";
            model.PaymentSucess = false;

            return View(model);
        }


        private void Debug(string message)
        {
            //if (_TbcPaymentSettings.EnableDebugLogging)
                _logger.Information("TBC payment: " + message);
        }


        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var TBCSettings = _settingService.LoadSetting<TBCBankCardSettings>(storeScope);

            var model = new Models.ConfigurationModel(TBCSettings);

            return View("~/Plugins/Payments.TBCBankCard/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(Models.ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var TBCSettings = _settingService.LoadSetting<TBCBankCardSettings>(storeScope);

            //save settings
            TBCSettings.CertificatePath = model.CertificatePath;
            TBCSettings.CurrencyCode = model.CurrencyCode;
            TBCSettings.SecretPass = model.SecretPass;
            TBCSettings.ServiceURL = model.ServiceURL;
            TBCSettings.EnableDebugLogging = model.EnableDebugLogging;
            TBCSettings.PaymentDescription = model.PaymentDescription;
            TBCSettings.RedirectURL = model.RedirectURL;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.CertificatePath, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.CurrencyCode, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.SecretPass, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.EnableDebugLogging, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.PaymentDescription, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.ServiceURL, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(TBCSettings, x => x.RedirectURL, true, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            var model = new Models.PaymentInfoModel();

            //set postback values
            var form = this.Request.Form;
            model.PurchaseOrderNumber = form["PurchaseOrderNumber"];

            return View("~/Plugins/Payments.PurchaseOrder/Views/PaymentInfo.cshtml", model);
        }



        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            paymentInfo.CustomValues.Add("Order number", form["PurchaseOrderNumber"]);
            return paymentInfo;
        }

        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }



    }
}
