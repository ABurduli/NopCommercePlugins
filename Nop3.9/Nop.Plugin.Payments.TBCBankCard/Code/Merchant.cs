using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Text;

namespace Nop.Plugin.Payments.TBCBankCard.Code
{
    public class Merchant
    {
        public string Bank_server_url { get; set; }
        public string Keystore_file { get; set; }
        public string Keystore_password { get; set; }
        public int Bank_server_timeout { get; set; }
        

        private string keySubject { get; set; }
        public List<CertificateSubjectItem> SubjectItems { get; set; }
        private X509Certificate MerchantCertificate = null;

        public Merchant(string certFile, string pass, string ServerUrl, int bank_server_timeout)
        {
            Bank_server_url = ServerUrl;
            Keystore_file = certFile;
            Keystore_password = pass;
            Bank_server_timeout = bank_server_timeout;

            //var cert =new X509Certificate(System.IO.File.ReadAllBytes(Keystore_file), Keystore_password);
            //MerchantCertificate = new X509Certificate(Keystore_file, Keystore_password, X509KeyStorageFlags.MachineKeySet);
            MerchantCertificate = new X509Certificate2(Keystore_file, Keystore_password);
            keySubject = MerchantCertificate.Subject;
            SubjectItems =
                (from i in keySubject.Split(',')
                 select new CertificateSubjectItem()
                 {
                     KEY = i.Split('=')[0],
                     Value = i.Split('=')[1]
                 }).ToList();
        }

        private string GetMerchantIDFromKeystore()
        {
            return (from i in SubjectItems
                    where i.KEY == "CN"
                    select i.Value).FirstOrDefault();
        }

        private string GetMerchantIdParamString()
        {
            return string.Format("&MerchantIdentifier={0}", HttpContext.Current.Server.UrlEncode(GetMerchantIDFromKeystore()));
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

        public string startP2PTrans(CommandParams cm)
        {
            cm.command = "s";
            cm.desc = "Fishmarket Pyment";
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }


        // SMS - ტრანზაქციის რეგისტრაცია -v
        public string SendTransData(CommandParams cm)
        {
            cm.command = "v";
            cm.desc = "Fishmarket Pyment";
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }
        // DMS - ტრანზაქციის რეგისტრაცია -a
        // public String startDMSAuth(String amount, String currency, String ip, String desc, String language, Properties properties)
        public string SendPreAuthorization(CommandParams cm, string description="Web Shop Payment")
        {
            cm.command = "a";
            cm.desc = description;
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }


        // DMS - ტრანზაქციის შესრულება -t
        //public String makeDMSTrans(String auth_id, String amount, String currency, String ip, String desc, String language, Properties properties)
        public string SendCapture(CommandParams cm, string description = "Web Shop Payment")
        {
            cm.command = "t";
            cm.desc = description;
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }

        public string SendReversal(CommandParams cm)
        {
            cm.command = "r";
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }


        // SMS/DMS - ტრანზაქციის შესრულების შედეგი -c
        public string GetTransResult(CommandParams cm)
        {
            cm.command = "c";
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }


        public string Refund(CommandParams cm)
        {
            cm.command = "k";
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }

        public string CloseDay(CommandParams cm)
        {
            cm.command = "b";
            return SendHttpPost(cm.CommandString());
        }

        public string registerRP(CommandParams cm)
        {
            cm.command = "p";
            cm.desc = "Card Save";
            cm.type = "AUTH";
            cm.perspayee_gen = "1";
            return SendHttpPost(cm.CommandString() + GetMerchantIdParamString());
        }


        #region HTTP Senders
        string SendHttpPost(string requeststring)
        {
            ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(delegate (object sender,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                }
            );

            HttpWebRequest _webreq = (HttpWebRequest)WebRequest.Create(Bank_server_url);
            _webreq.ClientCertificates.Add(MerchantCertificate);

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            string postdata = requeststring;
            byte[] data = encoding.GetBytes(postdata);
            _webreq.Method = "POST";
            _webreq.ContentType = "application/x-www-form-urlencoded; charset=ASCII";
            _webreq.ContentLength = data.Length;
            
            _webreq.KeepAlive = false;
            _webreq.ProtocolVersion = HttpVersion.Version11;
            _webreq.PreAuthenticate = true;
            _webreq.Timeout = 60000;
            string _respstring = "";

            _webreq.ContentLength = requeststring.Length;

            using (var sw = new StreamWriter(_webreq.GetRequestStream(), Encoding.ASCII))
            {
                sw.Write(requeststring);
            }

            /*
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

            HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
            using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                _respstring = _reader.ReadToEnd();
            }
            return _respstring;

        }


        string SendHttpPost(string requeststring,string url)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(delegate (object sender,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                }
            );

            HttpWebRequest _webreq = (HttpWebRequest)WebRequest.Create(url);
            _webreq.ClientCertificates.Add(MerchantCertificate);

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

            HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
            using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                _respstring = _reader.ReadToEnd();
            }
            return _respstring;

        }

        string SendHttpRequest(string requeststring)
        {
            string _respstring = null;
            HttpWebRequest _webreq = GetRequest(requeststring);

            HttpWebResponse _webresp = (HttpWebResponse)_webreq.GetResponse();
            using (StreamReader _reader = new StreamReader(_webresp.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                _respstring = _reader.ReadToEnd();
            }
            return _respstring;
        }

        private HttpWebRequest GetRequest(string requeststring)
        {

            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(Bank_server_url + "?" + requeststring);
            _request.ClientCertificates.Add(MerchantCertificate);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                    | SecurityProtocolType.Tls11
                                                    | SecurityProtocolType.Tls12
                                                    | SecurityProtocolType.Ssl3;

            ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(delegate (object sender,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                }
            );

            _request.Timeout = Bank_server_timeout;
            //_request.Credentials = CredentialCache.DefaultCredentials;
            _request.KeepAlive = false;
            _request.ProtocolVersion = HttpVersion.Version10;
            _request.PreAuthenticate = true;

            return _request;
        }
        #endregion
    }
}
