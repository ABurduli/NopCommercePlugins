using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BdoToTroniTag
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<string> res = new List<string>();
            res.Add(Properties.Settings.Default.HEADER);
            statusStrip1.Items[0].Text = "Get BDO Records...";
            Application.DoEvents();
            BalanceLib.BalanceConnector conn = new BalanceLib.BalanceConnector(Properties.Settings.Default.BDOUrl,
                                Properties.Settings.Default.BDOUser,
                                Properties.Settings.Default.BDOPass);

            try
            { 
                var items  = conn.GetItems();
                foreach (var p in items.Where(x=>!x.IsGroup))
                {
                    //if (Properties.Settings.Default.BDOGroupFilter != "" && p.Group == new Guid(Properties.Settings.Default.BDOGroupFilter))
                    if (!string.IsNullOrEmpty( p.Barcode))
                    {
                        string price = p.Price.Replace(",", ".");
                        if (Properties.Settings.Default.PriceDevider > 1)
                        {
                            decimal lpr = 0;
                            try
                            {
                                lpr = decimal.Parse(price);
                                lpr = lpr / Properties.Settings.Default.PriceDevider;
                            }
                            catch
                            {
                                lpr = 0;
                            }
                            if (lpr > 0)
                            {
                                price = lpr.ToString("0.##");
                            }


                        }

                        string oldPrice = "";
                        string isPromo = "";
                        if (!string.IsNullOrEmpty(p.AdditionalRequisites1))
                        {

                            decimal lpr = 0;
                            try
                            {
                                lpr = decimal.Parse(p.AdditionalRequisites1.Replace(",", "."));
                                if (p.Unit != "ცალი")
                                    lpr = lpr / Properties.Settings.Default.PriceDevider;
                            }
                            catch
                            {
                                lpr = 0;
                            }
                            if (lpr > 0)
                            {
                                oldPrice = lpr.ToString("0.##");
                            }
                        }
                        if (!string.IsNullOrEmpty(p.AdditionalRequisites2))
                        {
                            try
                            {
                                isPromo = p.AdditionalRequisites2.ToUpper() == "TRUE" ? "promo" : "";
                            }
                            catch { }
                        }

                        string szUnit = "100 გრ.";
                        if ( (p.Barcode.Length>=8 && !p.Barcode.StartsWith("22")) || p.Unit== "ცალი" )
                        {
                            szUnit = ""; 
                            price = p.Price.Replace(",", ".");
                        }

                        string enName = "";
                        if (string.IsNullOrEmpty(p.AdditionalRequisites3))
                        {
                            enName = p.FullName.Replace(",", " ");
                        }
                        else
                        {
                            enName = p.AdditionalRequisites3.Replace(",", " ");
                        }
                        string ns = $"{p.Barcode.Replace(",", " ")},{p.Name.Replace(","," ")},{enName},{price.Replace(",", ".")},₾,{isPromo},{szUnit.Replace(",", ".")},{oldPrice.Replace(",", " ")}";
                        //string ns = $"\"{p.Barcode}\",\"{p.Name.Replace(",", " ")}\",\"{p.FullName.Replace(",", " ")}\",\"{price.Replace(",", ".")}\",\"₾\",\"{isPromo}\",\"{szUnit.Replace(",", ".")}\",\"{oldPrice}\"";
                        res.Add(ns);
                    }

                }
            }
            catch (Exception ex)
            {
                statusStrip1.Items[0].Text = ex.Message;
                return;
            }

            statusStrip1.Items[0].Text = "Writing file...";
            Application.DoEvents();

            string path = Path.GetTempPath();
            DateTime d = DateTime.Now;
            //string fname = $"import_{d.Year}{d.Month}{d.Day}{d.Hour}{d.Minute}{d.Second}.csv";
            string fname = string.Format("import_{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}.csv",d.Year,d.Month,d.Day,d.Hour,d.Minute,d.Second); 
            string fullfilename = Path.Combine(path, fname);
            tbFileName.Text = fullfilename;
            StreamWriter sw = new StreamWriter(fullfilename,false,Encoding.Unicode);
            foreach (var i in res)
            {
                sw.WriteLine(i);
            }
            sw.Close();

            if (checkBox1.Checked)
            {
                statusStrip1.Items[0].Text = $"Uploading To FTP: {Properties.Settings.Default.OutputFTPUrl}";
                Application.DoEvents();

                string url = $"{Properties.Settings.Default.OutputFTPUrl}/{Properties.Settings.Default.Folder}";
                Upload(url, Properties.Settings.Default.FTPUser,
                    Properties.Settings.Default.FTPPass, fullfilename);
            }
            else
            {
                File.Copy(fullfilename, Path.Combine(Properties.Settings.Default.DirectFilePath, fname));
            }
            statusStrip1.Items[0].Text = "Finish";








        }

        private void Upload(string ftpServer, string userName, string password, string filename)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.UploadFile(ftpServer + "/" + new FileInfo(filename).Name, "STOR", filename);
            }
        }

        
    }
}
