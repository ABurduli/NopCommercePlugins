using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BalanceLib
{
    public class BalanceConnector
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int port { get; set; }

        public BalanceConnector(string url, string user,string password,int Port =80)
        {
            Url = url;
            UserName = user;
            Password = password;

        }


       

        string SendHttpPut(string requeststring, string url)
        {
            
            HttpWebRequest _webreq = (HttpWebRequest)WebRequest.Create(url);
            _webreq.Credentials = new NetworkCredential(UserName, Password);
            _webreq.Method = "PUT";
            _webreq.ContentType = "application/x-www-form-urlencoded";
            string _respstring = "";
            _webreq.ContentLength = requeststring.Length;

            using (var sw = new StreamWriter(_webreq.GetRequestStream(), Encoding.ASCII))
            {
                sw.Write(requeststring);
            }

            try
            {
                HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
                using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    _respstring = _reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _respstring = ex.Message;
            }
            return _respstring;

        }

        string SendHttpPost(string requeststring, string url)
        {

            HttpWebRequest _webreq = (HttpWebRequest)WebRequest.Create(url);
            _webreq.Credentials = new NetworkCredential(UserName, Password);

            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            string postdata = requeststring;
            byte[] data = encoding.GetBytes(postdata);
            _webreq.Method = "POST";
            _webreq.ContentType = "application/x-www-form-urlencoded";
            _webreq.ContentLength = data.Length;

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

            try
            {
                HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
                using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    _respstring = _reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _respstring = ex.Message;
            }
            return _respstring;

        }

        public async Task<List<BalanceLib.StockItem>> GetStockAsync(string Date = "", string Item = "", string Warehouse = "", string Branch = "")
        {
            string CurUrl = Url + "/Stocks";
            string urlParams = "";
            if (!string.IsNullOrEmpty(Date))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Date)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Date)}";
            if (!string.IsNullOrEmpty(Item))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Item)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Item)}";
            if (!string.IsNullOrEmpty(Warehouse))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Warehouse={System.Web.HttpUtility.UrlPathEncode(Warehouse)}" : $"&Warehouse={System.Web.HttpUtility.UrlPathEncode(Warehouse)}";
            if (!string.IsNullOrEmpty(Branch))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Branch)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Branch)}";

            List<BalanceLib.StockItem> res = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CurUrl);
            httpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    res = (List<BalanceLib.StockItem>)js.Deserialize(objText, typeof(List<BalanceLib.StockItem>));
                }

            }

            return res;
        }

        public List<BalanceLib.StockItem> GetStock(string Date="", string Item = "", string Warehouse = "", string Branch = "")
        {
            string CurUrl = Url + "/Stocks";
            string urlParams = "";
            if (!string.IsNullOrEmpty(Date))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Date)}": $"&Date={System.Web.HttpUtility.UrlPathEncode(Date)}";
            if (!string.IsNullOrEmpty(Item))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Item)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Item)}";
            if (!string.IsNullOrEmpty(Warehouse))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Warehouse={System.Web.HttpUtility.UrlPathEncode(Warehouse)}" : $"&Warehouse={System.Web.HttpUtility.UrlPathEncode(Warehouse)}";
            if (!string.IsNullOrEmpty(Branch))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Branch)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Branch)}";

            List<BalanceLib.StockItem> res = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CurUrl);
            httpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    res = (List<BalanceLib.StockItem>)js.Deserialize(objText, typeof(List<BalanceLib.StockItem>));
                }

            }

            return res;
        }

        public List<BalanceLib.ClientOrderItem> GetOrders()
        {
            List<BalanceLib.ClientOrderItem> res = null;
            string CurUrl = Url + "/SalesOrders";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CurUrl);
            httpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    res = (List<BalanceLib.ClientOrderItem>)js.Deserialize(objText, typeof(List<BalanceLib.ClientOrderItem>));
                }

            }
            return res;
        }

        public string CreateSalesOrder(List<ClientOrderItem> order)
        {
            string res = "";
            string CurUrl = Url + "/SalesOrders";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(order);
            res = SendHttpPut(json, CurUrl);

            return res;
        }

        public List<BalanceLib.PriceItem> GetPrice(string Date = "", string Item = "", string PriceType = "")
        {
            string CurUrl = Url + "/Prices";
            string urlParams = "";
            if (!string.IsNullOrEmpty(Date))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Date)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Date)}";
            if (!string.IsNullOrEmpty(Item))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(Item)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(Item)}";
            if (!string.IsNullOrEmpty(PriceType))
                urlParams = string.IsNullOrEmpty(urlParams) ? $"?Date={System.Web.HttpUtility.UrlPathEncode(PriceType)}" : $"&Date={System.Web.HttpUtility.UrlPathEncode(PriceType)}";
            

            List<BalanceLib.PriceItem> res = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CurUrl);
            httpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    res = (List<BalanceLib.PriceItem>)js.Deserialize(objText, typeof(List<BalanceLib.PriceItem>));
                }

            }

            return res;
        }
                
        public List<BalanceLib.ProdItem> GetItems(string Field ="",string Value="")
        {
            string CurUrl = Url+ "/Items";
            if (Field!="")
            {
                CurUrl += $"?{Field}={System.Web.HttpUtility.UrlPathEncode(Value)}";
            }
            List<BalanceLib.ProdItem> res = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CurUrl);
            httpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";

            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    res = (List<BalanceLib.ProdItem>)js.Deserialize(objText, typeof(List<BalanceLib.ProdItem>));
                }

            }
            return res;
        }
             
        public List<BalanceLib.ClientItem> GetClients(string Field = "", string Value = "")
        {
            string CurUrl = Url + "/Clients";
            if (Field != "")
            {
                CurUrl += $"?{Field}={System.Web.HttpUtility.UrlPathEncode(Value)}";
            }
            List<BalanceLib.ClientItem> res = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CurUrl);
            httpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";

            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    res = (List<BalanceLib.ClientItem>)js.Deserialize(objText, typeof(List<BalanceLib.ClientItem>));
                }

            }
            return res;
        }
    }
}
