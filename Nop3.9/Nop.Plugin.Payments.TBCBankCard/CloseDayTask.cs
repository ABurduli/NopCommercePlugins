using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Stores;
using Nop.Services.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nop.Plugin.Payments.TBCBankCard
{
    public class CloseDayTask : ITask
    {
        private readonly ILogger _logger;

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly TBCBankCardSettings _TbcPaymentSettings;

        public CloseDayTask(ILogger logger,
            IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            IProductService productService,
            IProductTagService productTagService,
            TBCBankCardSettings tbcPaymentSettings
            )
        {
            this._logger = logger;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
            this._productService = productService;
            this._productTagService = productTagService;
            this._TbcPaymentSettings = tbcPaymentSettings;
        }


        public void Execute()
        {
            _logger.Information("START TBC close Day task");
            string url = _TbcPaymentSettings.ServiceURL;
            
            string certPath = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Plugins\Payments.TBCBankCard\KeyStore\{_TbcPaymentSettings.CertificatePath}";
            Code.Merchant merchant = new Code.Merchant(certPath, _TbcPaymentSettings.SecretPass, url, 30000);
            string closeResult = "";

            try
            {
                closeResult = merchant.CloseDay(new Code.CommandParams());
                Code.CloseDayResult clRes = new Code.CloseDayResult(closeResult);
                if (clRes.RESULT=="OK")
                {
                    _logger.Information("START TBC close Day OK!");
                }
                else
                {
                    _logger.Information(closeResult);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("TBC CLOSE Day error:" + ex.Message);
                
            }
            _logger.Information("END TBC close Day task");
        }
    }
}
