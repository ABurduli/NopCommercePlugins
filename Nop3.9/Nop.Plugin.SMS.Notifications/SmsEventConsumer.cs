using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Events;
using Nop.Services.Orders;


namespace Nop.Plugin.SMS.Notifications
{
    public class SmsEventConsumer : IConsumer<OrderPlacedEvent> , 
                                    IConsumer<OrderCancelledEvent>, 
                                    IConsumer<OrderPaidEvent>,
                                    IConsumer<ShipmentSentEvent>
    {
        private readonly IPluginFinder _pluginFinder;
        private readonly IOrderService _orderService;
        private readonly IStoreContext _storeContext;
        private readonly SMSNotificationsSettings _smsSettings;

        public SmsEventConsumer(SMSNotificationsSettings smsSettings,
            IPluginFinder pluginFinder,
            IOrderService orderService,
            IStoreContext storeContext)
        {
            this._smsSettings = smsSettings;
            this._pluginFinder = pluginFinder;
            this._orderService = orderService;
            this._storeContext = storeContext;
        }

        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            if (!_smsSettings.Enabled) return;
            if (!_smsSettings.EnableOrderPlaced) return;

            if (_smsSettings.IgnnoreUserIDs.Split(',').Contains(eventMessage.Order.CustomerId.ToString()))
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "SMS alert not send. user:" + eventMessage.Order.CustomerId.ToString() + " in Ignore Lit",
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
                return;
            }


            string template = "";
            if (eventMessage.Order.CustomerLanguageId== _smsSettings.SecondLanguageID)
            {
                // second language
                template = _smsSettings.TemplateOrderPlaced2;
            }
            else
            {
                if (eventMessage.Order.CustomerLanguageId == _smsSettings.ThirdLanguageID)
                {
                    // Third language
                    template = _smsSettings.TemplateOrderPlaced3;
                }
                else
                {
                    // default
                    template = _smsSettings.TemplateOrderPlaced;
                }
            }
            if (string.IsNullOrEmpty(template))
                template = _smsSettings.TemplateOrderPlaced;

            try
            {
                template = string.Format(template, eventMessage.Order.Id, eventMessage.Order.OrderTotal);
            }
            catch { }
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("SMS.Notifications");
            if (pluginDescriptor == null)
                return;
            if (!_pluginFinder.AuthenticateStore(pluginDescriptor, _storeContext.CurrentStore.Id))
                return;

            var plugin = pluginDescriptor.Instance() as SMSNotificationsProvider;
            if (plugin == null)
                return;
            string phonenum = string.IsNullOrEmpty(eventMessage.Order.Customer.BillingAddress.PhoneNumber) ?
                                    eventMessage.Order.Customer.ShippingAddress.PhoneNumber :
                                    eventMessage.Order.Customer.BillingAddress.PhoneNumber;

            phonenum = ParcePhoneNumbe(phonenum);
            string resText = "";
            if (plugin.SendSms(phonenum, template, out resText))
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order placed\" SMS alert has been sent to phone:"+ phonenum,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
            }
            else
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order placed\" SMS alert can't send to phone:" + phonenum+". Error"+ resText,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
            }

        }

        public void HandleEvent(OrderCancelledEvent eventMessage)
        {
            if (!_smsSettings.Enabled) return;
            if (!_smsSettings.EnableOrderCanceled) return;

            if (_smsSettings.IgnnoreUserIDs.Split(',').Contains(eventMessage.Order.CustomerId.ToString()))
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "SMS alert not send. user:" + eventMessage.Order.CustomerId.ToString() + " in Ignore Lit",
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
                return;
            }


            string template = "";
            if (eventMessage.Order.CustomerLanguageId == _smsSettings.SecondLanguageID)
            {
                // second language
                template = _smsSettings.TemplateOrderCanceled2;
            }
            else
            {
                if (eventMessage.Order.CustomerLanguageId == _smsSettings.ThirdLanguageID)
                {
                    // Third language
                    template = _smsSettings.TemplateOrderCanceled3;
                }
                else
                {
                    // default
                    template = _smsSettings.TemplateOrderCanceled;
                }
            }
            if (string.IsNullOrEmpty(template))
                template = _smsSettings.TemplateOrderCanceled;

            try
            {
                template = string.Format(template, eventMessage.Order.Id, eventMessage.Order.OrderTotal);
            }
            catch { }
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("SMS.Notifications");
            if (pluginDescriptor == null)
                return;
            if (!_pluginFinder.AuthenticateStore(pluginDescriptor, _storeContext.CurrentStore.Id))
                return;

            var plugin = pluginDescriptor.Instance() as SMSNotificationsProvider;
            if (plugin == null)
                return;
            string phonenum = string.IsNullOrEmpty(eventMessage.Order.Customer.BillingAddress.PhoneNumber) ?
                                    eventMessage.Order.Customer.ShippingAddress.PhoneNumber :
                                    eventMessage.Order.Customer.BillingAddress.PhoneNumber;

            phonenum = ParcePhoneNumbe(phonenum);
            string resText = "";
            if (plugin.SendSms(phonenum, template,out resText))
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order canceled\" SMS alert has been sent to phone:" + phonenum,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
            }
            else
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order canceled\" SMS alert can't send to phone:" + phonenum+". Error "+ resText,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
            }
        }

        public void HandleEvent(OrderPaidEvent eventMessage)
        {
            if (!_smsSettings.Enabled) return;
            if (!_smsSettings.EnableOrderPayed) return;

            if (_smsSettings.IgnnoreUserIDs.Split(',').Contains(eventMessage.Order.CustomerId.ToString()))
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "SMS alert not send. user:" + eventMessage.Order.CustomerId.ToString() + " in Ignore Lit",
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
                return;
            }

            string template = "";
            if (eventMessage.Order.CustomerLanguageId == _smsSettings.SecondLanguageID)
            {
                // second language
                template = _smsSettings.TemplateOrderPayed2;
            }
            else
            {
                if (eventMessage.Order.CustomerLanguageId == _smsSettings.ThirdLanguageID)
                {
                    // Third language
                    template = _smsSettings.TemplateOrderPayed3;
                }
                else
                {
                    // default
                    template = _smsSettings.TemplateOrderPayed;
                }
            }
            if (string.IsNullOrEmpty(template))
                template = _smsSettings.TemplateOrderPayed;

            try
            {
                template = string.Format(template, eventMessage.Order.Id, eventMessage.Order.OrderTotal);
            }
            catch { }
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("SMS.Notifications");
            if (pluginDescriptor == null)
                return;
            if (!_pluginFinder.AuthenticateStore(pluginDescriptor, _storeContext.CurrentStore.Id))
                return;

            var plugin = pluginDescriptor.Instance() as SMSNotificationsProvider;
            if (plugin == null)
                return;
            string phonenum = string.IsNullOrEmpty(eventMessage.Order.Customer.BillingAddress.PhoneNumber) ?
                                    eventMessage.Order.Customer.ShippingAddress.PhoneNumber :
                                    eventMessage.Order.Customer.BillingAddress.PhoneNumber;

            phonenum = ParcePhoneNumbe(phonenum);
            string resText = "";
            if (plugin.SendSms(phonenum, template,out resText))
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order payed\" SMS alert has been sent to phone:" + phonenum,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
            }
            else
            {
                eventMessage.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order payed\" SMS alert can't send to phone:" + phonenum+". Error "+resText,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Order);
            }
        }

        public void HandleEvent(ShipmentSentEvent eventMessage)
        {
            if (!_smsSettings.Enabled) return;
            if (!_smsSettings.EnableShippingShipped) return;

            if (_smsSettings.IgnnoreUserIDs.Split(',').Contains(eventMessage.Shipment.Order.CustomerId.ToString()))
            {
                eventMessage.Shipment.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "SMS alert not send. user:" + eventMessage.Shipment.Order.CustomerId.ToString() + " in Ignore Lit",
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Shipment.Order);
                return;
            }

            string template = "";
            if (eventMessage.Shipment.Order.CustomerLanguageId == _smsSettings.SecondLanguageID)
            {
                // second language
                template = _smsSettings.TemplateShippingShipped2;
            }
            else
            {
                if (eventMessage.Shipment.Order.CustomerLanguageId == _smsSettings.ThirdLanguageID)
                {
                    // Third language
                    template = _smsSettings.TemplateShippingShipped3;
                }
                else
                {
                    // default
                    template = _smsSettings.TemplateShippingShipped;
                }
            }
            if (string.IsNullOrEmpty(template))
                template = _smsSettings.TemplateShippingShipped;

            try
            {
                template = string.Format(template, eventMessage.Shipment.Order.Id, eventMessage.Shipment.Order.OrderTotal);
            }
            catch { }
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("SMS.Notifications");
            if (pluginDescriptor == null)
                return;
            if (!_pluginFinder.AuthenticateStore(pluginDescriptor, _storeContext.CurrentStore.Id))
                return;

            var plugin = pluginDescriptor.Instance() as SMSNotificationsProvider;
            if (plugin == null)
                return;
            string phonenum = string.IsNullOrEmpty(eventMessage.Shipment.Order.Customer.BillingAddress.PhoneNumber) ?
                                    eventMessage.Shipment.Order.Customer.ShippingAddress.PhoneNumber :
                                    eventMessage.Shipment.Order.Customer.BillingAddress.PhoneNumber;

            phonenum = ParcePhoneNumbe(phonenum);
            string resText = "";
            if (plugin.SendSms(phonenum, template,out resText))
            {
                eventMessage.Shipment.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order shipped\" SMS alert has been sent to phone:" + phonenum,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Shipment.Order);
            }
            else
            {
                eventMessage.Shipment.Order.OrderNotes.Add(new OrderNote
                {
                    Note = "\"Order shipped\" SMS alert can't send to phone:" + phonenum+". Error "+resText,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(eventMessage.Shipment.Order);
            }
        }

        string ParcePhoneNumbe(string phone)
        {
            string phonenum = phone;
            string allowedChars = "0123456789";
            //phonenum = phonenum.Replace(" ", "").Replace("-", "").Replace("+","");
            phonenum = new string(phonenum.Where(c => allowedChars.Contains(c)).ToArray());

            if (phonenum.Length < 12)
            {
                phonenum = "995" + phonenum;
            }
            return phonenum;
        }
    }
}
