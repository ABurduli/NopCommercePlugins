using System.Collections.Generic;
using System.Web;


namespace Nop.Plugin.Payments.TBCBankCard.Code
{
    public class CommandParams
    {
        public string command { get; set; }
        string commandStr
        {
            get
            {
                return string.Format("command={0}", command);
            }
        }

        public decimal amount { get; set; }
        string amountStr
        {
            get
            {
                return amount != 0 ? string.Format("&amount={0}", (amount * 100).ToString("0")) : "";
            }
        }

        public string auth_id { get; set; }
        string auth_idStr
        {
            get
            {
                return auth_id != "" ? $"&auth_id={auth_id}" : "";
            }
        }

        public string currency { get; set; }
        string currencyStr
        {
            get
            {
                return string.Format("&currency={0}", currency);
            }
        }

        public string ip { get; set; }
        string ipStr
        {
            get
            {
                return string.Format("&client_ip_addr={0}", ip);
            }
        }

        public string desc { get; set; }
        string descStr
        {
            get
            {
                return desc != "" ? string.Format("&description={0}", HttpContext.Current.Server.UrlEncode(desc)) : "";
            }
        }

        public string type { get; set; }
        string typeStr
        {
            get
            {
                return type != "" ? string.Format("&msg_type={0}", HttpContext.Current.Server.UrlEncode(type)) : "";
            }
        }

        public string trans_id { get; set; }
        string trans_idStr
        {
            get
            {
                return trans_id != "" ? string.Format("&trans_id={0}", HttpContext.Current.Server.UrlEncode(trans_id)) : "";
            }
        }

        string language { get; set; }
        string languageStr
        {
            get
            {
                return language != "" ? string.Format("&language={0}", HttpContext.Current.Server.UrlEncode(language)) : "";
            }
        }

        public string rec_pmnt_id { get; set; }
        string rec_pmnt_idStr
        {
            get
            {
                return rec_pmnt_id != "" ? string.Format("&biller_client_id={0}", HttpContext.Current.Server.UrlEncode(rec_pmnt_id)) : "";
            }
        }

        public string expiry { get; set; }
        string expiryStr
        {
            get
            {
                return expiry != "" ? string.Format("&perspayee_expiry={0}", HttpContext.Current.Server.UrlEncode(expiry)) : "";
            }
        }

        public string perspayee_gen { get; set; }
        string perspayee_genStr
        {
            get
            {
                return perspayee_gen != "" ? string.Format("&perspayee_gen={0}", HttpContext.Current.Server.UrlEncode(perspayee_gen)) : "";
            }
        }

        /*
        public string server_versionStr
        {
            get
            {
                return string.Format("&server_version={0}", HttpContext.Current.Server.UrlEncode(Properties.Resources.Server_Version));
            }
        }
        */
        
        public List<MerchantPropertiItem> Props { get; set; }
        
        string propsStr
        {
            get
            {
                return GetMerchantPropertiesParamString(Props);
            }
        }
        

        public CommandParams(string lang="ge")
        {
            ip = HttpContext.Current.Request.UserHostAddress;
            currency = "981";
            language = lang;// "ge" : "en";
            type = "";
            desc = "";
            perspayee_gen = "";
            trans_id = "";
            rec_pmnt_id = "";
            expiry = "";
            auth_id = "";
            Props = null;
        }

        private string GetMerchantPropertiesParamString(List<MerchantPropertiItem> param)
        {
            string res = "";
            if (param != null)
            {
                foreach (var i in param)
                {
                    res = res + string.Format("&{0}={1}", i.KEY, HttpContext.Current.Server.UrlEncode(i.Value));
                }
            }
            return res;
        }

        public string CommandString()
        {
            return commandStr + amountStr + currencyStr + ipStr + typeStr + descStr + trans_idStr + languageStr + rec_pmnt_idStr + expiryStr + perspayee_genStr + propsStr + auth_idStr;// +server_versionStr;
        }
    }
}