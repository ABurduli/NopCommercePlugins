using System;
using System.Collections.Generic;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using System.Web.Routing;
using Nop.Core.Domain.Directory;
using System.Web;
using Nop.Services.Directory;
using Nop.Services.Common;
using Nop.Services.Tax;
using Nop.Core;
using Nop.Services.Logging;
using System.Net;
using System.IO;
using Nop.Services.Tasks;
using Nop.Core.Domain.Tasks;
using System.Text;


namespace Nop.Plugin.Payments.TBCBankCard
{
    public class TBCBankCardPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly CurrencySettings _currencySettings;
        private readonly HttpContextBase _httpContext;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly ICurrencyService _currencyService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ISettingService _settingService;
        private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly TBCBankCardSettings _TbcPaymentSettings;
        private readonly IWorkContext _workContext;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IOrderService _orderService;

        public TBCBankCardPaymentProcessor(CurrencySettings currencySettings,
           HttpContextBase httpContext,
           ICheckoutAttributeParser checkoutAttributeParser,
           ICurrencyService currencyService,
           IGenericAttributeService genericAttributeService,
           ILocalizationService localizationService,
           IOrderTotalCalculationService orderTotalCalculationService,
           ISettingService settingService,
           ITaxService taxService,
           IWebHelper webHelper,
           ILogger logger,
           IWorkContext workContext,
           TBCBankCardSettings tbcPaymentSettings,
           IScheduleTaskService scheduleTaskService,
           IOrderService orderService
            )
        {
            this._currencySettings = currencySettings;
            this._httpContext = httpContext;
            this._checkoutAttributeParser = checkoutAttributeParser;
            this._currencyService = currencyService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._settingService = settingService;
            this._taxService = taxService;
            this._webHelper = webHelper;
            this._logger = logger;
            this._workContext = workContext;
            this._TbcPaymentSettings = tbcPaymentSettings;
            this._scheduleTaskService = scheduleTaskService;
            this._orderService = orderService;


        }

        public bool SupportCapture => true;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => true;

        public bool SupportVoid => true;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => true;

        public string PaymentMethodDescription => "TBC Bank card processing";

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //it's not a redirection payment method. So we always return false
            return true;
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            CapturePaymentResult result = new CapturePaymentResult();

            string lang = _workContext.WorkingLanguage.UniqueSeoCode;
            string trID = capturePaymentRequest.Order.AuthorizationTransactionId;

            
            Debug("start capture of:"+ capturePaymentRequest.Order.OrderTotal.ToString());
            // Registre transaction
            string url = _TbcPaymentSettings.ServiceURL;
            //Debug($"Payment URL = {url}");
            string certPath = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Plugins\Payments.TBCBankCard\KeyStore\{_TbcPaymentSettings.CertificatePath}";
            Code.Merchant merchant = new Code.Merchant(certPath, _TbcPaymentSettings.SecretPass, url, 30000);
            string res = "";
            Code.CommandParams param = new Code.CommandParams(lang) { trans_id = trID, amount = capturePaymentRequest.Order.OrderTotal };
            //Debug(param.CommandString());
            try
            {
                res = merchant.SendCapture(param);
            }
            catch (Exception ex)
            {
                _logger.Error("TBC payment error - Capture:" + ex.Message);
                result.AddError("TBC Capture error.");
                return result;
            }
            Code.StatusResult CheckResult = null;
            CheckResult = new Code.StatusResult(res);
            if (CheckResult.RESULT_CODE=="000")
            {
                // success
                result.CaptureTransactionId = CheckResult.APPROVAL_CODE;
                result.CaptureTransactionResult = res;
                result.NewPaymentStatus = PaymentStatus.Paid;
               // _logger.Information("TBC OK: " + CheckResult.APPROVAL_CODE);
            }
            else
            {
                _logger.Error("TBC Capture failed: " + res);
                result.AddError("Capture failed: " + res);

            }
            
            return result;
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return 0;//throw new NotImplementedException();
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "TBCBank";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Payments.TBCBankCard.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            throw new NotImplementedException();
        }

        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "TBCBank";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Payments.TBCBankCard.Controllers" }, { "area", null } };

        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return false;
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            string rfcUrl = $"{_TbcPaymentSettings.RedirectURL}?trans_id={HttpUtility.UrlEncode(postProcessPaymentRequest.Order.AuthorizationTransactionId)}";
            Debug("Start Redirect to URL:" + rfcUrl);

            _httpContext.Response.Redirect(rfcUrl);
            
            //string res = SendHttpPost($"trans_id={HttpUtility.UrlEncode(postProcessPaymentRequest.Order.AuthorizationTransactionId)}", _TbcPaymentSettings.RedirectURL);
            
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            string GetTransactionID(string responce)
            {
                if (responce.Substring(0, 14).ToUpper() == "TRANSACTION_ID")
                {
                    return responce.Substring(16, 28);
                }
                else return "";
            }


            string lang = _workContext.WorkingLanguage.UniqueSeoCode;

            Debug("process start");
            // Registre transaction
            string url = _TbcPaymentSettings.ServiceURL;
            //Debug($"Payment URL = {url}");
            string certPath = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Plugins\Payments.TBCBankCard\KeyStore\{_TbcPaymentSettings.CertificatePath}";
            Code.Merchant merchant = new Code.Merchant(certPath, _TbcPaymentSettings.SecretPass, url, 30000);
            string res = "";
           
            try
            {
                Code.CommandParams param = new Code.CommandParams(lang) { amount = processPaymentRequest.OrderTotal };
                //Debug(param.CommandString());
                res = merchant.SendPreAuthorization(param);
            }
            catch (Exception ex)
            {
                result.AuthorizationTransactionResult = ex.Message;
                _logger.Debug("TBC payment error - PostProcess:" + ex.Message);
                result.AddError("TBC error: Register payment. "+ ex.Message);
                return result;

            }
            string transaction_id = GetTransactionID(res);
            result.AuthorizationTransactionId = transaction_id;
            result.AuthorizationTransactionCode = "";
            result.AuthorizationTransactionResult = res;
            if (transaction_id == "")
            {
                _logger.Debug("TBC payment error - PostProcess: error registring payment");
                result.AddError("TBC payment error: No Transaction ID");
                return result;
            }
            result.NewPaymentStatus = PaymentStatus.Pending;
            return result;
        }

        private void Debug(string message)
        {
           // if (_TbcPaymentSettings.EnableDebugLogging)
                _logger.Information("TBC payment: "+message);
        }



        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            /// CHECKING ORDER
            if (!refundPaymentRequest.IsPartialRefund)
            {
                

                if (!string.IsNullOrEmpty(refundPaymentRequest.Order.AuthorizationTransactionId) && 
                        string.IsNullOrEmpty(refundPaymentRequest.Order.CaptureTransactionId) )
                {
                    // Can check
                    RefundPaymentResult res = new RefundPaymentResult();
                    // get Result Frm Bank

                    var order = _orderService.GetOrderByAuthorizationTransactionIdAndPaymentMethod(refundPaymentRequest.Order.AuthorizationTransactionId, "Payments.TBCBankCard");

                    string lang = _workContext.WorkingLanguage.UniqueSeoCode;
                    string url = _TbcPaymentSettings.ServiceURL;
                    Debug($"Payment URL = {url}");
                    string certPath = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Plugins\Payments.TBCBankCard\KeyStore\{_TbcPaymentSettings.CertificatePath}";
                    Code.Merchant merchant = new Code.Merchant(certPath, _TbcPaymentSettings.SecretPass, url, 30000);
                    string bankPaymentResult = "";
                    Code.StatusResult CheckResult = null;
                    try
                    {
                        bankPaymentResult = merchant.GetTransResult(new Code.CommandParams(lang) { trans_id = refundPaymentRequest.Order.AuthorizationTransactionId });
                        CheckResult = new Code.StatusResult(bankPaymentResult);

                        if (CheckResult.RESULT == "OK")
                        {
                            order.PaymentStatus = Core.Domain.Payments.PaymentStatus.Authorized;
                            order.AuthorizationTransactionCode = CheckResult.RESULT;
                            _orderService.UpdateOrder(order);
                            Debug($"Check result: {CheckResult.RESULT}. Code: {CheckResult.RESULT_CODE}");
                            res.Errors.Clear();
                            res.NewPaymentStatus = PaymentStatus.Authorized;
                            return res;
                        }
                        else
                        {
                            order.AuthorizationTransactionCode = bankPaymentResult;
                            _orderService.UpdateOrder(order);
                            res.AddError("bankPaymentResult");
                            Debug($"Check result: {CheckResult.RESULT}. Code: {CheckResult.RESULT_CODE}");
                            return res;

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"TBC Paysucces error check transaction :{ex.Message}");
                        res.AddError(ex.Message);
                        return res;

                    }
                }
            }

            return new RefundPaymentResult();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {

            // Cancel Autorised Payment
            VoidPaymentResult Vres = new VoidPaymentResult();
            
            string lang = _workContext.WorkingLanguage.UniqueSeoCode;

            string trID = voidPaymentRequest.Order.AuthorizationTransactionId;


            Debug("start Void of:" + voidPaymentRequest.Order.OrderTotal.ToString());
            // Void transaction
            string url = _TbcPaymentSettings.ServiceURL;
            string certPath = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Plugins\Payments.TBCBankCard\KeyStore\{_TbcPaymentSettings.CertificatePath}";
            Code.Merchant merchant = new Code.Merchant(certPath, _TbcPaymentSettings.SecretPass, url, 30000);
            string res = "";
            Code.CommandParams param = new Code.CommandParams(lang) { trans_id = trID, amount = voidPaymentRequest.Order.OrderTotal };
            try
            {
                res = merchant.SendReversal(param);
            }
            catch (Exception ex)
            {
                _logger.Error("TBC payment error - Reversal:" + ex.Message);
                Vres.AddError("TBC Reversal error.");

                return Vres;
            }
            Code.StatusResult CheckResult = null;
            CheckResult = new Code.StatusResult(res);
            if (CheckResult.RESULT_CODE == "400")
            {
                // success
                Vres.NewPaymentStatus = PaymentStatus.Voided;
                
            }
            else
            {
                _logger.Error("TBC Reversal failed: " + res);
                Vres.AddError("Reversal failed: " + res);
            }
            return Vres;

        }

        public override void Install()
        {
            //settings
            var settings = new TBCBankCardSettings();
            
            _settingService.SaveSetting(settings);

            _scheduleTaskService.InsertTask(new Core.Domain.Tasks.ScheduleTask()
            {
                Enabled = false,
                Name = "TBC Bank Close day task",
                Seconds = 86400,
                StopOnError = false,
                Type = "Nop.Plugin.Payments.TBCBankCard.CloseDayTask, Nop.Plugin.Payments.TBCBankCard"
            });

            base.Install();
        }

        public override void Uninstall()
        {

            ScheduleTask task = _scheduleTaskService.GetTaskByType("Nop.Plugin.Payments.TBCBankCard.CloseDayTask, Nop.Plugin.Payments.TBCBankCard");

            if (task != null)
            {
                _scheduleTaskService.DeleteTask(task);
            }

            _settingService.DeleteSetting<TBCBankCardSettings>();


            base.Uninstall();
        }


        string SendHttpPost(string requeststring, string url)
        {
            HttpWebRequest _webreq = (HttpWebRequest)WebRequest.Create(url);

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            string postdata = requeststring;
            byte[] data = encoding.GetBytes(postdata);
            _webreq.Method = "POST";
            _webreq.ContentType = "application/x-www-form-urlencoded";
            _webreq.ContentLength = data.Length;

            _webreq.KeepAlive = false;
            _webreq.ProtocolVersion = HttpVersion.Version11;
            _webreq.PreAuthenticate = true;
            _webreq.Timeout = 60000;
            /*
            string _respstring = null;
            try
            {
                using (Stream stream = _webreq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                _respstring = ex.Message;
            }
            */
            string _respstring = "";

            _webreq.ContentLength = requeststring.Length;

            using (var sw = new StreamWriter(_webreq.GetRequestStream(), Encoding.ASCII))
            {
                sw.Write(requeststring);
            }


            HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
            using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                _respstring = _reader.ReadToEnd();
            }
            return _respstring;

        }

        
    }
}
