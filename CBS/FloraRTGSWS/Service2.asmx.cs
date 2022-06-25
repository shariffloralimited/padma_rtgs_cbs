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

    public class Service2 : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetAccountInfo(string AccountNo)
        {
            string successstring = "{\"status\":\"Success\",\"responseId\":\"200\",\"outputOFSResponse\":\"MD.WASIM BILLAH| 100007766 | BD0010002 | GULSHAN COR | 20 APR 2014 | ACTIVE | 500016.27 | BDT | \"}";
            string failedstring  = "{\"status\":\"Success\",\"responseId\":\"200\",\"outputOFSResponse\":\"No records were found that matched the selection criteria || SSELECT FBNK.ACCOUNT | \"}";
            string outputstr = failedstring;
            WriteLog("outputstr: " + outputstr);
            string result    = "";
            try
            {
                int ActNo = 1;
                if (AccountNo.Length > 9)
                {
                    AccountNo = AccountNo.Substring(9);
                    ActNo = Int32.Parse(AccountNo);
                }
                else
                {
                    ActNo = Int32.Parse(AccountNo);
                }

                int n = ActNo % 2;
                if (n == 0)
                {
                    outputstr = successstring;
                }
                result = GetOFSResponse(outputstr);
            }
            catch { }

            return result;
        }

        [WebMethod]
        public string GetSignature(string AccountNo)
        {
            string successstring = "{\"status\":\"Success\",\"responseId\":\"200\",\"outputOFSResponse\":\"No Images to display| 201407151127901449.jpg | \"}";
            string failedstring = "{\"status\":\"Success\",\"responseId\":\"200\",\"outputOFSResponse\":\"No records were found that matched the selection criteria || SSELECT FBNK.ACCOUNT | \"}";
            string outputstr = failedstring;
            WriteLog("outputstr: " + outputstr);
            string result = "";
            try
            {
                int ActNo = 1;
                if (AccountNo.Length > 9)
                {
                    AccountNo = AccountNo.Substring(9);
                    ActNo = Int32.Parse(AccountNo);
                }
                else
                {
                    ActNo = Int32.Parse(AccountNo);
                }

                int n = ActNo % 2;
                if (n == 0)
                {
                    outputstr = successstring;
                }
                result = GetSignaturePath(outputstr);
            }
            catch { }

            return result;

        }

        [WebMethod]
        public Result ExecuteTransaction(string IntrBkSttlmntDt, string ReqFlag, string ActNo, string TransactionAmount, string CreditRef, string DebitRef, string BranchCode, string FTID)
        {
            Result result = new Result();

            string inputstr = "{\"outSource\":\"FloraRTGS\",\n\"requestFlag\":\"" + ReqFlag + "\", \"accountNumber\":\"" + ActNo + "\",\n \"requestTxnDate\":\"" + IntrBkSttlmntDt + "\",\n\"debitAmount\":\"" + TransactionAmount + "\",\n\"creditTheirRef\":\"" + CreditRef + "\",\n\"debitTheirRef\":\"" + DebitRef + "\",\n\"cocodePB\":\"" + BranchCode + "\"}";
            if (FTID != "")
            {
                inputstr = "{\"outSource\":\"FloraRTGS\",\n\"requestFlag\":\"" + ReqFlag + "\", \"accountNumber\":\"" + ActNo + "\",\n \"requestTxnDate\":\"" + IntrBkSttlmntDt + "\",\n\"debitAmount\":\"" + TransactionAmount + "\",\n \"cocodePB\":\"" + BranchCode + "\", \n \"ftidPB\":\"" + FTID + "\"}";
            }

            WriteLog("inputstr: " + inputstr);
            string outputstr = Invoke(ActNo, inputstr);
            WriteLog("outputstr: " + outputstr);

            result.ResponseString = outputstr;
            result.CBSResult = "FAILED";

            string OFSResponse = GetOFSResponse(outputstr);
            result.CBSRefID = OFSResponse;
            if (OFSResponse.Substring(0, 2) == "FT")
            {
                result.CBSResult = "PASSED";
            }


            return result;
        }

        public string Invoke(string AccountNo, string InputStr)
        {
            string outputStr = "";
            string successstr = "{\"status\":\"Success\",\"responseId\":\"200\",\"outputOFSResponse\":\" FT19150R6M2G \"}";
            string failedsstr = "{\"status\":\"Success\",\"responseId\":\"200\",\"outputOFSResponse\":\"DEBIT.ACCT.NO:1:1 = MISSING ACCOUNT - RECORD \"}";
            outputStr = failedsstr;
            try
            {
                int ActNo = 1;
                if (AccountNo.Length > 9)
                {
                    AccountNo = AccountNo.Substring(9);
                    ActNo = Int32.Parse(AccountNo);
                }
                else
                {
                    ActNo = Int32.Parse(AccountNo);
                }

                int n = ActNo % 2;
                if (n == 0)
                {
                    outputStr = successstr;
                }
            }
            catch { }
            return outputStr;
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
            if (a.Length == 3)
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
}