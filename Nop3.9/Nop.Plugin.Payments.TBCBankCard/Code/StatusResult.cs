using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TBCBankCard.Code
{
    public class StatusResult
    {
        public string RESULT { get; set; }
        public string RESULT_MSG
        {
            get
            {
                string resstr = "";
                if (!string.IsNullOrEmpty(RESULT))
                {
                    switch (RESULT)
                    {
                        case "OK":
                            resstr = "დასრულებულია";
                            break;
                        case "FAILED":
                            resstr = "დაფიქსირდა შეცდომა";
                            break;
                        case "CREATED":
                            resstr = "შექმნილია";
                            break;
                        case "PENDING":
                            resstr = "მუშავდება";
                            break;
                        case "DECLINED":
                            resstr = "უარყოფილია";
                            break;
                        case "REVERSED":
                            resstr = "გაუქმდა";
                            break;
                        case "AUTOREVERSED":
                            resstr = "ავტომატურად გაუქმდა";
                            break;
                        case "TIMEOUT":
                            resstr = "გადააჭარბა მოლოდინის დროს";
                            break;
                        default:
                            resstr = "განუსაზღვრელი სტატუსი";
                            break;
                    }
                }
                return resstr;
            }
        }
        public int RESULT_ID
        {
            //// 0 Not processed, 1 Registered, 2 Sucess, 3 failed
            get
            {
                int res = 0;
                if (!string.IsNullOrEmpty(RESULT))
                {
                    switch (RESULT)
                    {
                        case "OK":
                            res = 2;
                            break;
                        case "FAILED":
                            res = 3;
                            break;
                        case "CREATED":
                            res = 0;
                            break;
                        case "PENDING":
                            res = 1;
                            break;
                        case "DECLINED":
                            res = 4;
                            break;
                        case "REVERSED":
                            res = 5;
                            break;
                        case "AUTOREVERSED":
                            res = 6;
                            break;
                        case "TIMEOUT":
                            res = 7;
                            break;
                        default:
                            res = -1;
                            break;
                    }
                }
                return res;
            }
        }

        public string RESULT_PS { get; set; }
        public string RESULT_CODE { get; set; }

        public string GetResultMSG(string lang="ge")
        {
            MessageResources mCode = new MessageResources();

            string resstr = "";
            if (!string.IsNullOrEmpty(RESULT_CODE))
            {
                switch (RESULT_CODE)
                {
                    case "000":
                    case "001":
                    case "002":
                    case "003":
                    case "004":
                    case "005":
                    case "006":
                    case "007":
                    case "100":
                    case "101":
                    case "102":
                    case "103":
                    case "104":
                    case "105":
                    case "106":
                    case "107":
                    case "108":
                    case "109":
                    case "110":
                    case "111":
                    case "112":
                    case "113":
                    case "114":
                    case "115":
                    case "116":
                    case "117":
                    case "118":
                    case "119":
                    case "120":
                    case "121":
                    case "122":
                    case "123":
                    case "124":
                    case "125":
                    case "126":
                    case "127":
                    case "128":
                    case "129":
                    case "180":
                    case "200":
                    case "201":
                    case "202":
                    case "203":
                    case "204":
                    case "205":
                    case "206":
                    case "207":
                    case "208":
                    case "209":
                    case "210":
                    case "300":
                    case "301":
                    case "302":
                    case "303":
                    case "304":
                    case "305":
                    case "306":
                    case "307":
                    case "308":
                    case "309":
                    case "400":
                    case "499":
                    case "500":
                    case "501":
                    case "502":
                    case "503":
                    case "504":
                    case "600":
                    case "601":
                    case "602":
                    case "603":
                    case "604":
                    case "605":
                    case "606":
                    case "680":
                    case "681":
                    case "700":
                    case "800":
                    case "900":
                    case "901":
                    case "902":
                    case "903":
                    case "904":
                    case "905":
                    case "906":
                    case "907":
                    case "908":
                    case "909":
                    case "910":
                    case "911":
                    case "912":
                    case "913":
                    case "914":
                    case "915":
                    case "916":
                    case "917":
                    case "918":
                    case "919":
                    case "920":
                    case "921":
                    case "922":
                    case "923":
                    case "950":
                        resstr = mCode.GetMessageResource("UFCRESULTCODE_" + RESULT_CODE, lang);
                        break;
                    default:
                        resstr = mCode.GetMessageResource("UFCRESULTCODE_XXX",lang) + " - " + RESULT_CODE;
                        break;
                }
            }
            return resstr;

        }

        public string RESULT_CODE_MSG
        {
            get
            {
                MessageResources mCode = new MessageResources();

                string resstr = "";
                if (!string.IsNullOrEmpty(RESULT_CODE))
                {
                    switch (RESULT_CODE)
                    {
                        case "000":
                        case "001":
                        case "002":
                        case "003":
                        case "004":
                        case "005":
                        case "006":
                        case "007":
                        case "100":
                        case "101":
                        case "102":
                        case "103":
                        case "104":
                        case "105":
                        case "106":
                        case "107":
                        case "108":
                        case "109":
                        case "110":
                        case "111":
                        case "112":
                        case "113":
                        case "114":
                        case "115":
                        case "116":
                        case "117":
                        case "118":
                        case "119":
                        case "120":
                        case "121":
                        case "122":
                        case "123":
                        case "124":
                        case "125":
                        case "126":
                        case "127":
                        case "128":
                        case "129":
                        case "180":
                        case "200":
                        case "201":
                        case "202":
                        case "203":
                        case "204":
                        case "205":
                        case "206":
                        case "207":
                        case "208":
                        case "209":
                        case "210":
                        case "300":
                        case "301":
                        case "302":
                        case "303":
                        case "304":
                        case "305":
                        case "306":
                        case "307":
                        case "308":
                        case "309":
                        case "400":
                        case "499":
                        case "500":
                        case "501":
                        case "502":
                        case "503":
                        case "504":
                        case "600":
                        case "601":
                        case "602":
                        case "603":
                        case "604":
                        case "605":
                        case "606":
                        case "680":
                        case "681":
                        case "700":
                        case "800":
                        case "900":
                        case "901":
                        case "902":
                        case "903":
                        case "904":
                        case "905":
                        case "906":
                        case "907":
                        case "908":
                        case "909":
                        case "910":
                        case "911":
                        case "912":
                        case "913":
                        case "914":
                        case "915":
                        case "916":
                        case "917":
                        case "918":
                        case "919":
                        case "920":
                        case "921":
                        case "922":
                        case "923":
                        case "950":
                            resstr = mCode.GetMessageResource("UFCRESULTCODE_" + RESULT);
                            break;
                        default:
                            resstr = mCode.GetMessageResource("UFCRESULTCODE_XXX") + " - " + RESULT_CODE;
                            break;
                    }
                }
                return resstr;
            }
        }

        public string D3SECURE { get; set; }
        public string RRN { get; set; }
        public string APPROVAL_CODE { get; set; }
        public string CARD_NUMBER { get; set; }
        public string AAV { get; set; }
        public string RECC_PMNT_ID { get; set; }
        public string RECC_PMNT_EXPIRY { get; set; }
        public string MRCH_TRANSACTION_ID { get; set; }
        public string TRANSACTION_ID { get; set; }

        public string ERROR { get; set; }

        public bool isParced { get; set; }
        public bool hasError { get; set; }

        public StatusResult(string src)
        {
            RESULT = "";
            isParced = false;
            hasError = false;
            try
            {
                var mlist = src.Split('\n');
                foreach (string s in mlist)
                {
                    var data = s.Split(new string[] { ": " }, StringSplitOptions.None);
                    switch (data[0])
                    {
                        case "RESULT":
                            RESULT = data[1].Trim();
                            break;
                        case "RESULT_PS":
                            RESULT_PS = data[1];
                            break;
                        case "RESULT_CODE":
                            RESULT_CODE = data[1];
                            break;
                        case "3DSECURE":
                            D3SECURE = data[1];
                            break;
                        case "RRN":
                            RRN = data[1];
                            break;
                        case "APPROVAL_CODE":
                            APPROVAL_CODE = data[1];
                            break;
                        case "CARD_NUMBER":
                            CARD_NUMBER = data[1];
                            break;
                        case "AAV":
                            AAV = data[1];
                            break;
                        case "RECC_PMNT_ID":
                            RECC_PMNT_ID = data[1];
                            break;
                        case "RECC_PMNT_EXPIRY":
                            RECC_PMNT_EXPIRY = data[1];
                            break;
                        case "MRCH_TRANSACTION_ID":
                            MRCH_TRANSACTION_ID = data[1];
                            break;
                        case "TRANSACTION_ID":
                            TRANSACTION_ID = data[1];
                            break;
                        case "error":
                            ERROR = data[1];
                            hasError = true;
                            break;
                    }
                }
            }
            catch { isParced = false; return; }
            isParced = true;
        }
    }
}
