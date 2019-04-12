using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.ExportImport;
using Nop.Services.ExportImport.Help;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Nop.Plugin.Widget.SalesReporting.Controllers
{
    [AdminAuthorize]
    public class SalesReportingController : BasePluginController
    {
        private readonly IDbContext _dbContent;
        private readonly IExportManager _exportManager;
        private CatalogSettings _catalogSettings;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;

        public SalesReportingController(
            IDbContext dbContent,
            IExportManager exportManager,
            IWorkContext workContext,
            IDateTimeHelper dateTimeHelper,
            CatalogSettings catalogSettings
            )
        {
            _dbContent = dbContent;
            _exportManager = exportManager;
            _catalogSettings = catalogSettings;
            this._workContext = workContext;
            this._dateTimeHelper = dateTimeHelper;
        }

        [HttpGet]
        public ActionResult AllSales()
        {
            var model = new Models.ReportDatesModel();
            model.StartDate = DateTime.Now.AddDays(-1).Date;
            model.EndDate = DateTime.Now.Date;
            return View("~/Plugins/Widgets.SalesReporting/Views/AllSales.cshtml",model);
        }

        [HttpPost]
        public ActionResult AllSales(Models.ReportDatesModel model)
        {
            try
            {
            var startDate = new SqlParameter("@SDate", model.StartDate);
            var endDate = new SqlParameter("@EDate", model.EndDate);

                if (model.OnlyHeaders)
                {
                    var res = _dbContent.SqlQuery<Models.ResultItem>("exec GetOrdersHeader @SDate,@EDate", startDate, endDate);


                    byte[] bytes = ExportAllOrdersToXlsx(res.ToList());
                    return File(bytes, MimeTypes.TextXlsx, "orders.xlsx");
                }
                else
                {
                    var res = _dbContent.SqlQuery<Models.ResultItem>("exec GetOrdersDetailed @SDate,@EDate", startDate, endDate);
                    byte[] bytes = ExportAllOrdersToXlsx(res.ToList());
                    return File(bytes, MimeTypes.TextXlsx, "orders.xlsx");
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return View("~/Plugins/Widgets.SalesReporting/Views/AllSales.cshtml", model);
            }
            
        }

        public virtual byte[] ExportAllOrdersToXlsx(IList<Models.ResultItem> orders)
        {
            //a vendor should have access only to part of order information
            var ignore = _workContext.CurrentVendor != null;

            //property array
            var properties = new[]
            {
                new PropertyByName<Models.ResultItem>("ID", o=>o.id),
                new PropertyByName<Models.ResultItem>("Order Date",o=>o.OrderDate),
                new PropertyByName<Models.ResultItem>("Shipping Date",o=>o.ShippingDate),
                new PropertyByName<Models.ResultItem>("Delivery Date",o=>o.DeliveryDate),
                new PropertyByName<Models.ResultItem>("Shiped in Min.",o=>o.ShippedInMinutes),
                new PropertyByName<Models.ResultItem>("Delivered in Min.",o=>o.DeliveredInMinutes),
                new PropertyByName<Models.ResultItem>("Order Status",o=>o.OrderStatus),
                new PropertyByName<Models.ResultItem>("Shipping Status",o=>o.ShippingStatus),
                new PropertyByName<Models.ResultItem>("Payment Method",o=>o.PaymentMethodSystemName),
                new PropertyByName<Models.ResultItem>("Customer",o=>o.Customer),
                new PropertyByName<Models.ResultItem>("Product",o=>o.ProductName),
                new PropertyByName<Models.ResultItem>("Item Cost",o=>o.OriginalProductCost),
                new PropertyByName<Models.ResultItem>("Item Base Price",o=>o.ItemBasePrice),
                new PropertyByName<Models.ResultItem>("Item Discount",o=>o.ItemDiscount),
                new PropertyByName<Models.ResultItem>("Item Quantity",o=>o.ItemQuantity),
                new PropertyByName<Models.ResultItem>("Item Unit Price",o=>o.ItemUnitPrice),
                //new PropertyByName<Models.ResultItem>("Item Price",o=>o.ItemPrice),
                new PropertyByName<Models.ResultItem>("Item Subtotal",o=>o.ItemSubTotal),
                new PropertyByName<Models.ResultItem>("Order Subtotal",o=>o.OrderSubTotal),
                new PropertyByName<Models.ResultItem>("Order Shipping", o=>o.OrderShipping),
                new PropertyByName<Models.ResultItem>("Order Discount",o=>o.OrderDiscount),
                new PropertyByName<Models.ResultItem>("Order Total",o=>o.OrderTotal),
                new PropertyByName<Models.ResultItem>("Auth. ID",o=>o.AuthorizationtransactionID),
                new PropertyByName<Models.ResultItem>("Capture ID",o=>o.TBC_CaptureID),
                new PropertyByName<Models.ResultItem>("Used Discounts",o=>o.UsedDiscounts),
                new PropertyByName<Models.ResultItem>("Client Registration Date",o=>o.ClientRegistrationDate)


            };

            return ExportToXlsx(properties, orders);
        }


        protected byte[] ExportToXlsx<T>(PropertyByName<T>[] properties, IEnumerable<T> itemsToExport)
        {
            using (var stream = new MemoryStream())
            {
                // ok, we can run the real code of the sample now
                using (var xlPackage = new ExcelPackage(stream))
                {
                    // uncomment this line if you want the XML written out to the outputDir
                    //xlPackage.DebugMode = true; 

                    // get handles to the worksheets
                    var worksheet = xlPackage.Workbook.Worksheets.Add(typeof(T).Name);
                    var fWorksheet = xlPackage.Workbook.Worksheets.Add("DataForFilters");
                    fWorksheet.Hidden = eWorkSheetHidden.VeryHidden;

                    //create Headers and format them 
                    var manager = new PropertyManager<T>(properties.Where(p => !p.Ignore));
                    manager.WriteCaption(worksheet, SetCaptionStyle);

                    var row = 2;
                    foreach (var items in itemsToExport)
                    {
                        manager.CurrentObject = items;
                        manager.WriteToXlsx(worksheet, row++, _catalogSettings.ExportImportUseDropdownlistsForAssociatedEntities, fWorksheet: fWorksheet);
                    }

                    xlPackage.Save();
                }
                return stream.ToArray();
            }
        }
        protected virtual void SetCaptionStyle(ExcelStyle style)
        {
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
            style.Font.Bold = true;
        }

    }
}
