
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Xml;

namespace FloraRTGSWS
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class Service1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetAccountInfo(string AccountNo)
        {
            string inputstr = "{\"outSource\":\"FloraRTGS\", \"requestFlag\":\"2\", \"accountNumber\":\""+AccountNo+"\"}";
            WriteLog("GetAccountInfo(" + AccountNo + ") inputstr: " + inputstr);

            string outputstr = Invoke(inputstr);
            WriteLog("GetAccountInfo(" + AccountNo + ") outputstr: " + outputstr);

            string responsestring = GetOFSResponse(outputstr);
            WriteLog("GetAccountInfo("+ AccountNo + ") responsestring: " + responsestring);

            return responsestring;
        }

        [WebMethod]
        public string GetSignature(string AccountNo)
        {
            string inputstr = "{ \"outSource\":\"FloraRTGS\",\"requestFlag\":\"5\",\"accountNumber\":\" " + AccountNo + "\"}";
            WriteLog("GetSignature(" + AccountNo + ") inputstr: " + inputstr);

            string outputstr = Invoke(inputstr);
            WriteLog("GetSignature(" + AccountNo + ") outputstr: " + outputstr);

            string responsestring = GetSignaturePath(outputstr);
            WriteLog("GetSignature(" + AccountNo + ") responsestring: " + responsestring);

            return responsestring;
        }

        [WebMethod]
        public Result ExecuteTransaction(string IntrBkSttlmntDt, string ReqFlag, string ActNo, string TransactionAmount, string CreditRef, string DebitRef, string BranchCode, string FTID)
        {
            Result result = new Result();

            string inputstr = "{\"outSource\":\"FloraRTGS\",\n\"requestFlag\":\"" + ReqFlag + "\", \"accountNumber\":\"" + ActNo + "\",\n \"requestTxnDate\":\"" + IntrBkSttlmntDt + "\",\n\"debitAmount\":\"" + TransactionAmount + "\",\n \"creditTheirRef\":\"" + CreditRef + "\",\n \"debitTheirRef\":\"" + DebitRef + "\",\n \"cocodePB\":\"" + BranchCode + "\"}";
            if (FTID != "")
            {
                inputstr = "{\"outSource\":\"FloraRTGS\",\n\"requestFlag\":\"" + ReqFlag + "\", \"accountNumber\":\"" + ActNo + "\",\n \"requestTxnDate\":\"" + IntrBkSttlmntDt + "\",\n\"debitAmount\":\"" + TransactionAmount + "\",\n \"cocodePB\":\"" + BranchCode + "\", \n \"ftidPB\":\"" + FTID + "\"}";
            }

            WriteLog("InputString: " + inputstr);

            string outputstr = Invoke(inputstr);
            result.ResponseString = outputstr;
            WriteLog("ResponseString: " + outputstr);
            result.CBSResult = "FAILED";

            string OFSResponse = GetOFSResponse(outputstr);
            result.CBSRefID = OFSResponse;
            if (OFSResponse.Substring(0, 2) == "FT")
            {
                result.CBSResult = "PASSED";
            }
            if(result.CBSRefID.Length > 15)
            {
                result.CBSRefID = result.CBSRefID.Substring(0, 15);
            }
            WriteLog("ExecuteTransaction CBSRefID: " + result.CBSRefID);
            return result;
        }

        //----------------------------------------------
        public string Invoke(string InputStr)
        {
            string OutputStr = "";
            HttpWebRequest req = null;
            WebResponse rsp = null;
            try
            {
                string uri = AppConfig.CBSURL;
                req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.MediaType = "application/json";
                req.Accept = "application/json";
                req.Credentials = CredentialCache.DefaultNetworkCredentials;
                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                writer.WriteLine(InputStr);
                writer.Close();
                rsp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(rsp.GetResponseStream());
                OutputStr = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                WriteLog("Error on Invoke: " + ex.Message);
            }
            return OutputStr;
        }
        public string GetOFSResponse(string responsestring)
        {
            responsestring = responsestring.Replace("{", "").Replace("}", "");
            responsestring = responsestring.Replace("\"", "");

            string OFSResponse = "No OFS Response found.";
            string[] a = responsestring.Split(',');
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                if (a[i].IndexOf("outputOFSResponse") > -1)
                {
                    string[] outstr = a[i].Split(':');
                    OFSResponse = outstr[1];
                    break;
                }
            }
            return OFSResponse.Trim();
        }

        public string GetSignaturePath(string responsestring)
        {
            responsestring = responsestring.Replace("{", "").Replace("}", "");
            responsestring = responsestring.Replace("\"", "");

            string OFSResponse = "No Response found.";
            string[] a = responsestring.Split('|');
            if(a.Length == 3)
            {
                OFSResponse = a[1];
            }
            return OFSResponse.Trim();
        }
        protected void WriteLog(string Msg)
        {
            FileStream fs = new FileStream(AppConfig.LogPath + "\\WS-" + System.DateTime.Today.ToString("yyyyMMdd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(System.DateTime.Now.ToString() + ": " + Msg);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }
    public class Result
    {
        public string ResponseString = "";
        public string CBSRefID       = "";
        public string CBSResult      = "";
    }
}