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

namespace Nop.Plugin.ExternalProcessing.BDOBalance
{
    public class SyncTask : ITask
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

        public SyncTask(ILogger logger,
            IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            IProductService productService,
            IProductTagService productTagService
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
        }

        public void Execute()
        {
            _logger.Information("START SYNC TO BALANCE");
            var BDOSyncSettings = _settingService.LoadSetting<BDOBalanceSettings>();

            var BConnector = new BalanceLib.BalanceConnector(BDOSyncSettings.BalanceServiceURL, 
                                    BDOSyncSettings.BalanceUserName, BDOSyncSettings.BalanceUserPass);

            if (BDOSyncSettings.DownloadQuanity)
            {
                UpdateStockProuctStock(BConnector,BDOSyncSettings.UnpublishProductWhenLovStock);
            }

            

            _logger.Information("END SYNC");
        }

        private void UpdateStockProuctStock(BalanceLib.BalanceConnector conn, bool unpublish=false)
        {
            var BDOSyncSettings = _settingService.LoadSetting<BDOBalanceSettings>();
            _logger.Information("Start stock sync");
            List<BalanceLib.StockItem> StockData = null;
            List<BalanceLib.ProdItem> BProducts = null;
            try
            {
                StockData = conn.GetStock();
                BProducts = conn.GetItems();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message,ex);
                return;
            }
            if (StockData==null)
            {
                _logger.Information("No stock data received.");
                return;
            }
            _logger.Information($"Received {StockData.Count()} records.");

            

            var prods = _productService.SearchProducts().ToList();
            if (prods != null)
            {
                foreach (var p in prods)
                {
                    var bdoProd = BProducts.Where(x => x.ExtCode.ToUpper() == p.Id.ToString()).FirstOrDefault();
                    if (bdoProd==null)
                    {
                        _logger.Information($"Product ID:{p.Id}. not found in BDO Products (ExtCode).");
                    }
                    else
                    {
                        var s = StockData.Where(x => x.Item == bdoProd.uid).FirstOrDefault();
                        if (s==null)
                        {
                            _logger.Information($"BDO Product UID:{bdoProd.uid} not found in ERP Stock.");
                        }
                        else
                        {
                            try
                            {
                                decimal bq = decimal.Parse(s.Quantity.Replace(" ", "").Replace(',', '.'));
                                if (!string.IsNullOrEmpty( BDOSyncSettings.BalanceWeightUnitName) && bdoProd.Unit== BDOSyncSettings.BalanceWeightUnitName)
                                {
                                    if (p.Weight > 0)
                                        bq = bq / p.Weight;
                                }

                                p.StockQuantity = (int)bq;
                                _productService.UpdateProduct(p);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("Asigne quantity Error: " + ex.Message, ex);
                            }
                        }
                    }


                    
                }
            }
            else
            {
                _logger.Warning("No products found in store?");
            }


        }
    }
}
