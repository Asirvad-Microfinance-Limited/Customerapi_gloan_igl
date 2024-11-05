using APIBaseClassLibrary.V1.Log;
using DataAccessLibrary;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static GlobalValues.GlobalVariables;

namespace APIBaseClassLibrary.V1.TokenAttribute
{
    public class Token : IToken
    {
        public IDBAccessHelper helper;

        public Token(IDBAccessHelper _helper)
        {
            helper = _helper;
           
        }
        public bool isValidToken(string userContextObj)
        {
            bool isValidToken = false;
            ErrorLog logClass = new ErrorLog();
            try 
            {
                var strr= logClass.writeLog("TokenValdationStart");
                OracleParameter[] parm_coll = new OracleParameter[3];
               
                parm_coll[0] = new OracleParameter("var_token", OracleDbType.Varchar2);
                parm_coll[0].Value = userContextObj;
                parm_coll[0].Direction = ParameterDirection.Input;
                parm_coll[1] = new OracleParameter("var_otp_duration", OracleDbType.Decimal);
                parm_coll[1].Value = (int)Durations.LoginOTP;
                parm_coll[1].Direction = ParameterDirection.Input;
                parm_coll[2] = new OracleParameter("out_status", OracleDbType.Decimal);
                parm_coll[2].Direction = ParameterDirection.Output;
                var strr1 = logClass.writeLog("TokenValdationProcedureCall");
                helper.ExecuteNonQuery("proc_ValidateToken", parm_coll);
                if (parm_coll[2].Value.ToString() == "1")
                {
                    isValidToken = true;
                    var strr2 = logClass.writeLog("TokenValdationProcedureSuccess");
                }
            }
            catch (Exception ex)
            {
                isValidToken = false;
                var strr3 = logClass.writeLog("exception:"+ ex.Message );
            }
            return isValidToken;
        }
    }
}
