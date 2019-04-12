using Nop.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Cms;
using Nop.Web.Framework.Menu;
using Nop.Core.Data;
using Nop.Services.Configuration;
using Nop.Services.Tasks;
using Nop.Core.Domain.Tasks;
using System.Web;
using System.Web.Routing;

namespace Nop.Plugin.ExternalProcessing.BDOBalance
{
    public class BDOBalancePlugin : BasePlugin, IWidgetPlugin
    {
        public ISettingService _settingService { get; private set; }
        public IScheduleTaskService _scheduleTaskService { get; private set; }

        public BDOBalancePlugin(ISettingService settingService,
                                IScheduleTaskService scheduleTaskService
            )
        {
            _settingService = settingService;
            _scheduleTaskService = scheduleTaskService;
        }

        public override void Install()
        {
            var settings = new BDOBalanceSettings()
            {
                BalanceServiceURL = "",
                DownloadQuanity = false,
                UnpublishProductWhenLovStock = false,
                UploadOrders = false
            };
            _settingService.SaveSetting(settings);

            _scheduleTaskService.InsertTask(new Core.Domain.Tasks.ScheduleTask()
            {
                Enabled = false,
                Name = "BDO Balance Sync task",
                Seconds = 3600,
                StopOnError = false,
                Type = "Nop.Plugin.ExternalProcessing.BDOBalance.SyncTask, Nop.Plugin.ExternalProcessing.BDOBalance"
            });
            base.Install();
        }
        public override void Uninstall()
        {
            ScheduleTask task = _scheduleTaskService.GetTaskByType("Nop.Plugin.ExternalProcessing.BDOBalance.SyncTask, Nop.Plugin.ExternalProcessing.BDOBalance");

            if (task != null)
            {
                _scheduleTaskService.DeleteTask(task);
            }
            _settingService.DeleteSetting<BDOBalanceSettings>();
            base.Uninstall();
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>();
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "BDOBalanceSync";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.ExternalProcessing.BDOBalance.Controllers" }, { "area", null } };

        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "PublicBDOBalanceSync";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.ExternalProcessing.BDOBalance.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
    }
}
