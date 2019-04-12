using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TBCBankCard.Code
{
    public class CloseDayResult
    {
        public string RESULT { get; set; }
        public string RESULT_CODE { get; set; }
        public int FLD_074 { get; set; }
        public int FLD_075 { get; set; }
        public int FLD_076 { get; set; }
        public int FLD_077 { get; set; }
        public int FLD_086 { get; set; }
        public int FLD_087 { get; set; }
        public int FLD_088 { get; set; }
        public int FLD_089 { get; set; }

        public bool isParced { get; set; }

        public CloseDayResult(string src)
        {
            RESULT = "";
            isParced = false;
            try
            {
                var mlist = src.Split('\n');
                foreach (string s in mlist)
                {
                    var data = s.Split(':');
                    switch (data[0])
                    {
                        case "RESULT":
                            RESULT = data[1].Trim();
                            break;
                        case "RESULT_CODE":
                            RESULT_CODE = data[1].Trim();
                            break;
                        case "FLD_074":
                            FLD_074 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_075":
                            FLD_075 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_076":
                            FLD_076 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_077":
                            FLD_077 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_086":
                            FLD_086 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_087":
                            FLD_087 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_088":
                            FLD_088 = TryToInt(data[1].Trim());
                            break;
                        case "FLD_089":
                            FLD_089 = TryToInt(data[1].Trim());
                            break;
                    }
                }
            }
            catch { isParced = false; return; }
            isParced = true;
        }

        private int TryToInt(string val)
        {
            int res = -1;
            try
            {
                res = Int32.Parse(val);
            }
            catch { res = -1; }
            return res;
        }

    }
}
