using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTGSCBS
{
    class CBSParams
    {
        public int TransactionID = 0;
        public string IntrBkSttlmntDt = "";
        public string ReqFlag = "";
        public string ActNo = "";
        public string TransactionAmount = "";
        public string CreditRef = "";
        public string DebitRef = "";
        public string BranchCode = "";
        public string FTID = "";

    }
    class CBSDB
    {
        public CBSParams GetSingleOutwardTransaction()
        {
            // TransactionID, OutwardID, IntrBkSttlmntDt, ReqFlag, ActNo, TransactionAmount, ResponseTime, ResponseString, CBSRefID

            SqlConnection myConnection = new SqlConnection(AppVariable.ServerLogin);
            SqlCommand myCommand = new SqlCommand("CBS_GetListOutward", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.Int, 4);
            parameterTransactionID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTransactionID);

            SqlParameter parameterIntrBkSttlmntDt = new SqlParameter("@IntrBkSttlmntDt", SqlDbType.VarChar, 8);
            parameterIntrBkSttlmntDt.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterIntrBkSttlmntDt);

            SqlParameter parameterReqFlag = new SqlParameter("@ReqFlag", SqlDbType.VarChar, 3);
            parameterReqFlag.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterReqFlag);

            SqlParameter parameterActNo = new SqlParameter("@ActNo", SqlDbType.VarChar, 13);
            parameterActNo.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterActNo);

            SqlParameter parameterTTransactionAmount = new SqlParameter("@TransactionAmount", SqlDbType.VarChar, 17);
            parameterTTransactionAmount.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTTransactionAmount);

            SqlParameter parameterCreditRef = new SqlParameter("@CreditRef", SqlDbType.VarChar, 16);
            parameterCreditRef.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterCreditRef);

            SqlParameter parameterDebitRef = new SqlParameter("@DebitRef", SqlDbType.VarChar, 16);
            parameterDebitRef.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterDebitRef);

            SqlParameter parameterBranchCode = new SqlParameter("@BranchCode", SqlDbType.VarChar, 10);
            parameterBranchCode.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterBranchCode);

            SqlParameter parameterFTID = new SqlParameter("@FTID", SqlDbType.VarChar, 30);
            parameterFTID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterFTID);

            myConnection.Open();
            myCommand.ExecuteNonQuery();

            CBSParams cbs = new CBSParams();
            cbs.TransactionID       = (int) parameterTransactionID.Value;
            cbs.IntrBkSttlmntDt     = (string) parameterIntrBkSttlmntDt.Value;
            cbs.ReqFlag             = (string) parameterReqFlag.Value;
            cbs.ActNo               = (string) parameterActNo.Value;
            cbs.TransactionAmount   = (string) parameterTTransactionAmount.Value;
            cbs.CreditRef           = (string) parameterCreditRef.Value;
            cbs.DebitRef            = (string) parameterDebitRef.Value;
            cbs.BranchCode          = (string) parameterBranchCode.Value;
            cbs.FTID                = (string) parameterFTID.Value;
            myCommand.Dispose();
            myConnection.Close();
            myConnection.Dispose();

            return cbs;
        }

        public CBSParams GetSingleInwardTransaction()
        {
            //TransactionID, InwardID, IntrBkSttlmntDt, ReqFlag, ActNo, TransactionAmount, ResponseTime, ResponseString, CBSRefID

            SqlConnection myConnection = new SqlConnection(AppVariable.ServerLogin);
            SqlCommand myCommand = new SqlCommand("CBS_GetListInward", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.Int, 4);
            parameterTransactionID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTransactionID);

            SqlParameter parameterIntrBkSttlmntDt = new SqlParameter("@IntrBkSttlmntDt", SqlDbType.VarChar, 8);
            parameterIntrBkSttlmntDt.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterIntrBkSttlmntDt);

            SqlParameter parameterReqFlag = new SqlParameter("@ReqFlag", SqlDbType.VarChar, 3);
            parameterReqFlag.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterReqFlag);

            SqlParameter parameterActNo = new SqlParameter("@ActNo", SqlDbType.VarChar, 13);
            parameterActNo.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterActNo);

            SqlParameter parameterTransactionAmount = new SqlParameter("@TransactionAmount", SqlDbType.VarChar, 17);
            parameterTransactionAmount.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTransactionAmount);

            SqlParameter parameterCreditRef = new SqlParameter("@CreditRef", SqlDbType.VarChar, 16);
            parameterCreditRef.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterCreditRef);

            SqlParameter parameterDebitRef = new SqlParameter("@DebitRef", SqlDbType.VarChar, 16);
            parameterDebitRef.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterDebitRef);

            SqlParameter parameterBranchCode = new SqlParameter("@BranchCode", SqlDbType.VarChar, 10);
            parameterBranchCode.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterBranchCode);

            myConnection.Open();
            myCommand.ExecuteNonQuery();

            CBSParams cbs = new CBSParams();
            cbs.TransactionID       = (int)parameterTransactionID.Value;
            cbs.IntrBkSttlmntDt     = (string)parameterIntrBkSttlmntDt.Value;
            cbs.ReqFlag             = (string)parameterReqFlag.Value;
            cbs.BranchCode          = (string)parameterBranchCode.Value;
            cbs.ActNo               = (string)parameterActNo.Value;
            cbs.TransactionAmount   = (string)parameterTransactionAmount.Value;
            cbs.CreditRef           = (string)parameterCreditRef.Value;
            cbs.DebitRef            = (string)parameterDebitRef.Value;
            cbs.BranchCode          = (string)parameterBranchCode.Value;

            myCommand.Dispose();
            myConnection.Close();
            myConnection.Dispose();

            return cbs;
        }

        public void UpdateOutwardStatus(int TransactionID, string ResponseString, string CBSRefID, string CBSResult)
        {
            SqlConnection myConnection = new SqlConnection(AppVariable.ServerLogin);
            SqlCommand myCommand = new SqlCommand("CBS_UpdateOutwardStatus", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.Int, 4);
            parameterTransactionID.Value = TransactionID;
            myCommand.Parameters.Add(parameterTransactionID);

            SqlParameter parameterResponseString = new SqlParameter("@ResponseString", SqlDbType.VarChar, 500);
            parameterResponseString.Value = ResponseString;
            myCommand.Parameters.Add(parameterResponseString);

            SqlParameter parameterCBSRefID = new SqlParameter("@CBSRefID", SqlDbType.VarChar, 15);
            parameterCBSRefID.Value = CBSRefID;
            myCommand.Parameters.Add(parameterCBSRefID);

            SqlParameter parameterCBSResult = new SqlParameter("@CBSResult", SqlDbType.VarChar, 6);
            parameterCBSResult.Value = CBSResult;
            myCommand.Parameters.Add(parameterCBSResult);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            myCommand.Dispose();
            myConnection.Dispose();
        }

        public void UpdateInwardStatus(int TransactionID, string ResponseString, string CBSRefID, string CBSResult)
        {
            SqlConnection myConnection = new SqlConnection(AppVariable.ServerLogin);
            SqlCommand myCommand = new SqlCommand("CBS_UpdateInwardStatus", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.Int, 4);
            parameterTransactionID.Value = TransactionID;
            myCommand.Parameters.Add(parameterTransactionID);

            SqlParameter parameterResponseString = new SqlParameter("@ResponseString", SqlDbType.VarChar, 500);
            parameterResponseString.Value = ResponseString;
            myCommand.Parameters.Add(parameterResponseString);

            SqlParameter parameterCBSRefID = new SqlParameter("@CBSRefID", SqlDbType.VarChar, 15);
            parameterCBSRefID.Value = CBSRefID;
            myCommand.Parameters.Add(parameterCBSRefID);

            SqlParameter parameterCBSResult = new SqlParameter("@CBSResult", SqlDbType.VarChar, 6);
            parameterCBSResult.Value = CBSResult;
            myCommand.Parameters.Add(parameterCBSResult);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            myCommand.Dispose();
            myConnection.Dispose();
        }
    }
}
