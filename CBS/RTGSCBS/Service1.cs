using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Timers;

namespace RTGSCBS
{
    public partial class Service1 : ServiceBase
    {

        public string LogPath   = ConfigurationManager.AppSettings["LogPath"];

        public string CBSURL    = ConfigurationManager.AppSettings["CBSURL"];
        public string CBSUser   = ConfigurationManager.AppSettings["CBSUser"];
        public string CBSPass   = ConfigurationManager.AppSettings["CBSPass"];

        RTGSWS.Service1 srv = new RTGSWS.Service1();

        Timer timer1 = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------
        protected override void OnStart(string[] args)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            WriteLog("Service Started.");

            long delay = 1000;
            try
            {
                delay = Int32.Parse(ConfigurationManager.AppSettings["IntervalInSeconds"]) * 1000;
            }
            catch
            {
                WriteLog("IntervalInSeconds key/value incorrect. Default set to 1 sec.");
            }

            if (delay < 1000)
            {
                WriteLog("Sleep time too short: Changed to default(1 sec).");
                delay = 1000;
            }

            timer1.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer1.Interval = delay;
            timer1.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteLog("Waking up...");
            timer1.Stop();

            #region ElapsedTimeProcesses
            try
            {
                ProcessOutwardTransaction();
            }
            catch (Exception ex)
            {
                WriteLog("Error ProcessOutwardTransaction: " + ex.Message);
            }

            try
            {
                ProcessInwardTransaction();
            }
            catch (Exception ex)
            {
                WriteLog("Error ProcessInwardTransaction: " + ex.Message);
            }
            #endregion

            timer1.Start();

            WriteLog("Going to sleep...");
        }
        protected override void OnStop()
        {
            WriteLog("Service Stopped.");
        }
        //--------------------------------------------------------------

        protected void ProcessOutwardTransaction()
        {
            RTGSWS.Result result = new RTGSWS.Result();
            CBSDB db = new CBSDB();
            CBSParams cbs = db.GetSingleOutwardTransaction();
            if (cbs.TransactionID != 0)
            {
                WriteLog("-------------------------------------");
                WriteLog("Outward TransactionID: " + cbs.TransactionID.ToString());
                WriteLog("IntrBkSttlmntDt: " + cbs.IntrBkSttlmntDt);
                WriteLog("RegFlag: " + cbs.ReqFlag);
                WriteLog("ActNo: " + cbs.ActNo);
                WriteLog("TransAmount: " + cbs.TransactionAmount);
                WriteLog("CreditRef: " + cbs.CreditRef);
                WriteLog("DebitRef: " + cbs.DebitRef);
                WriteLog("BranchCode: " + cbs.BranchCode);
                WriteLog("FTID: " + cbs.FTID);

                result = srv.ExecuteTransaction(cbs.IntrBkSttlmntDt, cbs.ReqFlag, cbs.ActNo, cbs.TransactionAmount, cbs.CreditRef, cbs.DebitRef, cbs.BranchCode, cbs.FTID);
                WriteLog("Outward CBS Completed: " + result.CBSRefID);

                db.UpdateOutwardStatus(cbs.TransactionID, result.ResponseString, result.CBSRefID, result.CBSResult);
                WriteLog("Outward Status Updated: " + cbs.TransactionID.ToString() + Environment.NewLine);
            }
            cbs = null;
        }
        protected void ProcessInwardTransaction()
        {
            RTGSWS.Result result = new RTGSWS.Result();
            CBSDB db = new CBSDB();
            CBSParams cbs = db.GetSingleInwardTransaction();
            if (cbs.TransactionID != 0)
            {
                WriteLog("-------------------------------------");
                WriteLog("Inward TransactionID: " + cbs.TransactionID.ToString());
                WriteLog("IntrBkSttlmntDt: " + cbs.IntrBkSttlmntDt);
                WriteLog("RegFlag: " + cbs.ReqFlag);
                WriteLog("BranchCode: " + cbs.BranchCode);
                WriteLog("ActNo: " + cbs.ActNo);
                WriteLog("TransAmount: " + cbs.TransactionAmount);
                WriteLog("CreditRef: " + cbs.CreditRef);
                WriteLog("DebitRef: " + cbs.DebitRef);
                WriteLog("BranchCode: " + cbs.BranchCode);

                result = srv.ExecuteTransaction(cbs.IntrBkSttlmntDt, cbs.ReqFlag, cbs.ActNo, cbs.TransactionAmount, cbs.CreditRef, cbs.DebitRef, cbs.BranchCode, "");
                WriteLog("Inward CBS Completed: " + result.CBSRefID);

                db.UpdateInwardStatus(cbs.TransactionID, result.ResponseString, result.CBSRefID, result.CBSResult);
                WriteLog("Inward Status Updated: " + cbs.TransactionID.ToString() + Environment.NewLine);

            }
            cbs = null;
        }

        //--------------------------------------------------------------
        protected void WriteLog(string Msg)
        {
            FileStream fs = new FileStream(LogPath + "\\CBS-" + System.DateTime.Today.ToString("yyyyMMdd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);
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