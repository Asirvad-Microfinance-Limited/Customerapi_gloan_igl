using DataAccessLibrary;
using EmployeeAPI.Models;
using EmployeeAPI.Models.Request;
using EmployeeAPI.V1.BLL;
using EmployeeAPI.V1.Models.Request;
using EmployeeAPI.V1.Models.Response;
using EmployeeAPI.V1.TokenManager;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using static GlobalValues.GlobalVariables;
using static GlobalValues.GlobalVariables.Constants;
using APIBaseClassLibrary.V1.BLL;
using System.Security.Cryptography;
using System.Text;
using APIBaseClassLibrary.V1.Log;

namespace Employee.V1.BLL
{
    public class LoginBLL : APIBaseBLL, ILoginBLL
    {

        public IDBAccessHelper helper;
        public IEmployee employee;
        

        public LoginBLL(IDBAccessHelper _helper, IEmployee _employee)
        {
            helper = _helper;
            employee = _employee;
        }

        //protected byte[] getEdata(string userName, string password)
        //{
        //    MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        //    byte[] hashedDataBytes;
        //    UTF8Encoding encoder = new UTF8Encoding();
        //    hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(userName + "raju" + password));
        //    return hashedDataBytes;
        //}

        /// <Created>Uneesh - 100156</Created>
        /// <summary>User Login Check</summary> 
        public LoginResponse userLogin(LoginRequest request)
        {
            /*
             
             1. Validate user with the given user credentials and Log the validate process (Failed or Succes)
                If the user (username only) is failed to login for 5 times, then lock that particular user 
             2. Is the user is valid then check whether the Second factor is enabled or not
             3. If the User is not validated then return negative msg to the client
             4. If second factor is enabled then Generate the OTP and save to the DB, include OTP duration and expiration after min period (ex: Max 5 mint)
             5. Else generate the token and update the login key in the Log table and send to the client
             6. 
             
             */

            //----------------------------------
            //string sql = "update  employee_master set  password=:psd   where emp_code=" + request.employeeID + "";
            //byte[] hashedDataBytes1;
            //hashedDataBytes1 = employee.getEdata(request.employeeID.ToString(), request.password);

            //OracleParameter[] parm_coll1 = new OracleParameter[1];
            //parm_coll1[0] = new OracleParameter("psd", OracleDbType.Raw);
            //parm_coll1[0].Value = hashedDataBytes1;
            //parm_coll1[0].Direction = ParameterDirection.Input;
            //int val = helper.ExecuteNonQuery(sql, parm_coll1);
            //-----------------------------------


            LoginResponse response = new LoginResponse();
			

			try
            {
                if(request.branchID==0)
                {
                    response.status.code = APIStatus.failed;
                    response.status.message = "You are not authorise to access this module";
                    response.loginStatus = LoginStatus.inValidUser;
                    response.status.flag = ProcessStatus.success;
                    return response;
                }

                int login = -1;
                switch (request.formMode)
                {
                    case LoginFormMode.Login:
                        {
                            JwtTokenManager jwtToken = new JwtTokenManager();
                            string token = string.Empty;

                            ErrorLog logClass = new ErrorLog();
                            var strr = logClass.writeLog("CreateTokenStart");

                            token = jwtToken.CreateToken(request.branchID, request.employeeID);
                           var strr5 = logClass.writeLog("CreatedToken" + token);

                            if (!String.IsNullOrEmpty(token))
                            {
                                //DBAccessHelper helper = new DBAccessHelper();
                                OracleParameter[] parm_coll = new OracleParameter[21];
                                try
                                {

                                    byte[] hashedDataBytes;
                                    hashedDataBytes = employee.getEdata(request.employeeID.ToString(), new PasswordSecurity().AngularDecrypt(request.password));

                                    parm_coll[0] = new OracleParameter("var_employeeid", OracleDbType.Decimal);
                                    parm_coll[0].Value = request.employeeID;
                                    parm_coll[0].Direction = ParameterDirection.Input;
                                    parm_coll[1] = new OracleParameter("var_passwd", OracleDbType.Raw, 1000);
                                    parm_coll[1].Value = hashedDataBytes;
                                    parm_coll[1].Direction = ParameterDirection.Input;

                                    parm_coll[2] = new OracleParameter("var_branchid", OracleDbType.Decimal);
                                    parm_coll[2].Value = request.branchID;
                                    parm_coll[2].Direction = ParameterDirection.Input;

                                    parm_coll[3] = new OracleParameter("var_token", OracleDbType.Varchar2, 1000);
                                    parm_coll[3].Value = token;
                                    parm_coll[3].Direction = ParameterDirection.Input;

                                    parm_coll[4] = new OracleParameter("var_token_duration", OracleDbType.Decimal);
                                    parm_coll[4].Value = (int)Durations.Token;
                                    parm_coll[4].Direction = ParameterDirection.Input;

                                    parm_coll[5] = new OracleParameter("var_otp_duration", OracleDbType.Decimal);
                                    parm_coll[5].Value = (int)Durations.LoginOTP;
                                    parm_coll[5].Direction = ParameterDirection.Input;

                                    parm_coll[6] = new OracleParameter("var_signature_data", OracleDbType.Varchar2, 500);
                                    parm_coll[6].Value = request.siganture;
                                    parm_coll[6].Direction = ParameterDirection.Input;

                                    parm_coll[7] = new OracleParameter("var_accessid", OracleDbType.Decimal);
                                    parm_coll[7].Direction = ParameterDirection.Output;
                                    parm_coll[8] = new OracleParameter("var_roleid", OracleDbType.Decimal);
                                    parm_coll[8].Direction = ParameterDirection.Output;
                                    parm_coll[9] = new OracleParameter("var_emp_br", OracleDbType.Decimal);
                                    parm_coll[9].Direction = ParameterDirection.Output;
                                    parm_coll[10] = new OracleParameter("var_passwd_flg", OracleDbType.Decimal);
                                    parm_coll[10].Direction = ParameterDirection.Output;

                                    parm_coll[11] = new OracleParameter("firmID", OracleDbType.Decimal);
                                    parm_coll[11].Direction = ParameterDirection.Output;
                                    parm_coll[12] = new OracleParameter("firmName", OracleDbType.Varchar2,200);
                                    parm_coll[12].Direction = ParameterDirection.Output;
                                    parm_coll[13] = new OracleParameter("employeeName", OracleDbType.Varchar2, 100);
                                    parm_coll[13].Direction = ParameterDirection.Output;
                                    parm_coll[14] = new OracleParameter("branchName", OracleDbType.Varchar2, 100);
                                    parm_coll[14].Direction = ParameterDirection.Output;
                                    parm_coll[15] = new OracleParameter("postID", OracleDbType.Decimal);
                                    parm_coll[15].Direction = ParameterDirection.Output;
                                    
                                    parm_coll[16] = new OracleParameter("OTP", OracleDbType.Decimal);
                                    parm_coll[16].Direction = ParameterDirection.Output;
                                    parm_coll[17] = new OracleParameter("OTPmoblie_no", OracleDbType.Varchar2, 100);
                                    parm_coll[17].Direction = ParameterDirection.Output;
                                    parm_coll[18] = new OracleParameter("LoginKey", OracleDbType.Varchar2, 500);
                                    parm_coll[18].Direction = ParameterDirection.Output;
                                    parm_coll[19] = new OracleParameter("error_msg", OracleDbType.Varchar2, 1000);
                                    parm_coll[19].Direction = ParameterDirection.Output;
                                    parm_coll[20] = new OracleParameter("error_status", OracleDbType.Decimal,100);
                                    parm_coll[20].Direction = ParameterDirection.Output;
                                    var strr6 = logClass.writeLog("Start proc_ValidateEmployee , Password:"+ hashedDataBytes);
                                    helper.ExecuteNonQuery("proc_ValidateEmployee", parm_coll);
                                    var strr7 = logClass.writeLog("End proc_ValidateEmployee"+ Convert.ToInt64(parm_coll[20].Value.ToString()));
                                    if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_Success))
                                    {
                                        if ((Convert.ToInt64(parm_coll[10].Value.ToString()) >= 1) && (Convert.ToInt64(parm_coll[10].Value.ToString()) < 7) && (request.branchID == (Convert.ToInt64(parm_coll[9].Value.ToString()))))
                                        {
                                            response.loginStatus = LoginStatus.validUser;
                                            response.firmID = Convert.ToInt32(parm_coll[11].Value.ToString());
                                            response.firmName = parm_coll[12].Value.ToString();
                                            response.empCode = request.employeeID;
                                            response.employeeName = parm_coll[13].Value.ToString();
                                            response.branchID = Convert.ToInt32(parm_coll[9].Value.ToString());
                                            response.branchName = parm_coll[14].Value.ToString();
                                            response.accessID = Convert.ToInt32(parm_coll[7].Value.ToString());
                                            response.roleID = Convert.ToInt32(parm_coll[8].Value.ToString());

                                            int checkToken = 0;
                                            DataTable dt = new DataTable();
                                            string checkTokenQuery = "select count(*)  from .LOGIN_TOKEN_HANDLER t  where t.emp_code =  " + request.employeeID + " and t.login_branch_id =  " + request.branchID + " and t.status = 1 and endtime <= sysdate";
                                            dt = helper.ExecuteDataSet(checkTokenQuery).Tables[0];
                                            checkToken = Convert.ToInt32(dt.Rows[0][0]);
                                            if (checkToken == 1)
                                            {
                                                response.token = token;
                                            }
                                            else
                                            {
                                                string TokenQuery = "select t.token  from .LOGIN_TOKEN_HANDLER t  where t.emp_code = " + request.employeeID + " and t.login_branch_id =  " + request.branchID + "  and t.status = 1";
                                                dt = helper.ExecuteDataSet(TokenQuery).Tables[0];
                                                    response.token = dt.Rows[0][0].ToString();
                                            }

                                            response.status.code = APIStatus.success;
                                            response.status.message = "Success";
                                            response.status.flag = ProcessStatus.success;
                                        }
                                        else
                                        {
                                            response.status.code = APIStatus.failed;
                                            response.status.message = "Failed";
                                            response.loginStatus = LoginStatus.inValidUser;
                                            response.status.flag = ProcessStatus.success;
                                        }
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_Failed))
                                    {
                                        response.status.code = APIStatus.failed;
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_NoMobileNumber))
                                    {
                                        response.status.code = APIStatus.NoMobileNumber;
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_OTP_Sent))
                                    {
                                        response.token = token;
                                        response.status.code = APIStatus.OTP_SentTOClient;
                                        response.mobileNumber = parm_coll[17].Value.ToString();
                                        response.loginKey = parm_coll[18].Value.ToString();
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_OTP_Already_Sent))
                                    {
                                        response.token = token;
                                        response.status.code = APIStatus.OTP_AlreadySent;
                                        response.mobileNumber = parm_coll[17].Value.ToString();
                                        response.loginKey = parm_coll[18].Value.ToString();
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    response.status.flag = ProcessStatus.failed;
                                    response.status.code = APIStatus.exception;
                                    response.status.message = "Exception :- " + ex.Message;
                                    var strr3 = logClass.writeLog("exception:" + ex.Message);
                                }
                            }
                            else {
                                response.status.flag = ProcessStatus.failed;
                                response.status.code = APIStatus.faileTOGenerateToken;
                                response.status.message = "Failed to generate token";
                                var strr4 = logClass.writeLog("Failed to generate token");
                            }
                            break;
                        }
                    case LoginFormMode.GL1_Verification:
                        login = employeeValidation_Repledge(request.employeeID, request.password, request.branchID,request.bio_flag);
                        //login = 1;
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.GL2_Verification:
                        //login = 1;
                        login = employeeValidation_Repledge(request.employeeID, request.password, request.branchID,request.bio_flag);
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.ABH_Verification:
                        //login = 1;
                        login = employeeValidation_ABH_BH(request.employeeID, request.password, request.branchID,request.bio_flag);
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.BH_Verification:
                        //login = 1;
                        login = employeeValidation_ABH_BH(request.employeeID, request.password, request.branchID,request.bio_flag);
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.Repledge:
                       login = employeeValidation_Repledge(request.employeeID, request.password, request.branchID,request.bio_flag);
                        //login = 1;
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.Settlement: // DOORSTEP GL2 ---14-JUL-2020  --VALUE 8
                         login = employeeValidation_doorstep(request.employeeID, request.password, request.branchID,request.bio_flag);
                        //login = 1;
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.Inventory_Release:
                        break;
                    case LoginFormMode.FormK:
                        login = employeeValidation_FormK(request.employeeID, request.password, request.branchID,request.bio_flag);
                        //login = 1;
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                    case LoginFormMode.CashNeftDisburse: // door step login ---14-jul-2020---Sreerekha K // Value = 11
                        {
                            JwtTokenManager jwtToken = new JwtTokenManager();
                            string token = string.Empty;

                            token = jwtToken.CreateToken(request.branchID, request.employeeID);
                            if (!String.IsNullOrEmpty(token))
                            {
                                //DBAccessHelper helper = new DBAccessHelper();
                                OracleParameter[] parm_coll = new OracleParameter[21];
                                try
                                {

                                    byte[] hashedDataBytes;
                                    hashedDataBytes = employee.getEdata(request.employeeID.ToString(), new PasswordSecurity().AngularDecrypt(request.password));

                                    parm_coll[0] = new OracleParameter("var_employeeid", OracleDbType.Decimal);
                                    parm_coll[0].Value = request.employeeID;
                                    parm_coll[0].Direction = ParameterDirection.Input;
                                    parm_coll[1] = new OracleParameter("var_passwd", OracleDbType.Raw, 1000);
                                    parm_coll[1].Value = hashedDataBytes;
                                    parm_coll[1].Direction = ParameterDirection.Input;

                                    parm_coll[2] = new OracleParameter("var_branchid", OracleDbType.Decimal);
                                    parm_coll[2].Value = request.branchID;
                                    parm_coll[2].Direction = ParameterDirection.Input;

                                    parm_coll[3] = new OracleParameter("var_token", OracleDbType.Varchar2, 1000);
                                    parm_coll[3].Value = token;
                                    parm_coll[3].Direction = ParameterDirection.Input;

                                    parm_coll[4] = new OracleParameter("var_token_duration", OracleDbType.Decimal);
                                    parm_coll[4].Value = (int)Durations.Token;
                                    parm_coll[4].Direction = ParameterDirection.Input;

                                    parm_coll[5] = new OracleParameter("var_otp_duration", OracleDbType.Decimal);
                                    parm_coll[5].Value = (int)Durations.LoginOTP;
                                    parm_coll[5].Direction = ParameterDirection.Input;

                                    parm_coll[6] = new OracleParameter("var_signature_data", OracleDbType.Varchar2, 500);
                                    parm_coll[6].Value = request.siganture;
                                    parm_coll[6].Direction = ParameterDirection.Input;

                                    parm_coll[7] = new OracleParameter("var_accessid", OracleDbType.Decimal);
                                    parm_coll[7].Direction = ParameterDirection.Output;
                                    parm_coll[8] = new OracleParameter("var_roleid", OracleDbType.Decimal);
                                    parm_coll[8].Direction = ParameterDirection.Output;
                                    parm_coll[9] = new OracleParameter("var_emp_br", OracleDbType.Decimal);
                                    parm_coll[9].Direction = ParameterDirection.Output;
                                    parm_coll[10] = new OracleParameter("var_passwd_flg", OracleDbType.Decimal);
                                    parm_coll[10].Direction = ParameterDirection.Output;

                                    parm_coll[11] = new OracleParameter("firmID", OracleDbType.Decimal);
                                    parm_coll[11].Direction = ParameterDirection.Output;
                                    parm_coll[12] = new OracleParameter("firmName", OracleDbType.Varchar2, 200);
                                    parm_coll[12].Direction = ParameterDirection.Output;
                                    parm_coll[13] = new OracleParameter("employeeName", OracleDbType.Varchar2, 100);
                                    parm_coll[13].Direction = ParameterDirection.Output;
                                    parm_coll[14] = new OracleParameter("branchName", OracleDbType.Varchar2, 100);
                                    parm_coll[14].Direction = ParameterDirection.Output;
                                    parm_coll[15] = new OracleParameter("postID", OracleDbType.Decimal);
                                    parm_coll[15].Direction = ParameterDirection.Output;

                                    parm_coll[16] = new OracleParameter("OTP", OracleDbType.Decimal);
                                    parm_coll[16].Direction = ParameterDirection.Output;
                                    parm_coll[17] = new OracleParameter("OTPmoblie_no", OracleDbType.Varchar2, 100);
                                    parm_coll[17].Direction = ParameterDirection.Output;
                                    parm_coll[18] = new OracleParameter("LoginKey", OracleDbType.Varchar2, 500);
                                    parm_coll[18].Direction = ParameterDirection.Output;
                                    parm_coll[19] = new OracleParameter("error_msg", OracleDbType.Varchar2, 1000);
                                    parm_coll[19].Direction = ParameterDirection.Output;
                                    parm_coll[20] = new OracleParameter("error_status", OracleDbType.Decimal, 100);
                                    parm_coll[20].Direction = ParameterDirection.Output;

                                    helper.ExecuteNonQuery("proc_ValidateEmployee_doorstep", parm_coll);

                                    if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_Success))
                                    {
                                        if ((Convert.ToInt64(parm_coll[10].Value.ToString()) >= 1) && (Convert.ToInt64(parm_coll[10].Value.ToString()) < 7) )
                                        {
                                            response.loginStatus = LoginStatus.validUser;
                                            response.firmID = Convert.ToInt32(parm_coll[11].Value.ToString());
                                            response.firmName = parm_coll[12].Value.ToString();
                                            response.empCode = request.employeeID;
                                            response.employeeName = parm_coll[13].Value.ToString();
                                            response.branchID = Convert.ToInt32(parm_coll[9].Value.ToString());
                                            response.branchName = parm_coll[14].Value.ToString();
                                            response.accessID = Convert.ToInt32(parm_coll[7].Value.ToString());
                                            response.roleID = Convert.ToInt32(parm_coll[8].Value.ToString());
                                            response.postID= Convert.ToInt32(parm_coll[15].Value.ToString());
                                            int checkToken = 0;
                                            DataTable dt = new DataTable();
                                            string checkTokenQuery = "select count(*)  from LOGIN_TOKEN_HANDLER t  where t.emp_code =  " + request.employeeID + " and t.login_branch_id =  " + request.branchID + " and t.status = 1 and endtime <= sysdate";
                                            dt = helper.ExecuteDataSet(checkTokenQuery).Tables[0];
                                            checkToken = Convert.ToInt32(dt.Rows[0][0]);
                                            if (checkToken == 1)
                                            {
                                                response.token = token;
                                            }
                                            else
                                            {
                                                string TokenQuery = "select t.token  from LOGIN_TOKEN_HANDLER t  where t.emp_code = " + request.employeeID + " and t.login_branch_id =  " + request.branchID + "  and t.status = 1";
                                                dt = helper.ExecuteDataSet(TokenQuery).Tables[0];
                                                response.token = dt.Rows[0][0].ToString();
                                            }

                                            response.status.code = APIStatus.success;
                                            response.status.message = "Success";
                                            response.status.flag = ProcessStatus.success;
                                        }
                                        else
                                        {
                                            response.status.code = APIStatus.failed;
                                            response.status.message = "Failed";
                                            response.loginStatus = LoginStatus.inValidUser;
                                            response.status.flag = ProcessStatus.success;
                                        }
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_Failed))
                                    {
                                        response.status.code = APIStatus.failed;
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_NoMobileNumber))
                                    {
                                        response.status.code = APIStatus.NoMobileNumber;
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_OTP_Sent))
                                    {
                                        response.token = token;
                                        response.status.code = APIStatus.OTP_SentTOClient;
                                        response.mobileNumber = parm_coll[17].Value.ToString();
                                        response.loginKey = parm_coll[18].Value.ToString();
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                    else if ((Convert.ToInt64(parm_coll[20].Value.ToString()) == Constants.LoginFlags.Flag_OTP_Already_Sent))
                                    {
                                        response.token = token;
                                        response.status.code = APIStatus.OTP_AlreadySent;
                                        response.mobileNumber = parm_coll[17].Value.ToString();
                                        response.loginKey = parm_coll[18].Value.ToString();
                                        response.status.message = parm_coll[19].Value.ToString();
                                        response.loginStatus = LoginStatus.inValidUser;
                                        response.status.flag = ProcessStatus.success;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    response.status.flag = ProcessStatus.failed;
                                    response.status.code = APIStatus.exception;
                                    response.status.message = "Exception :- " + ex.Message;
                                }
                            }
                            else
                            {
                                response.status.flag = ProcessStatus.failed;
                                response.status.code = APIStatus.faileTOGenerateToken;
                                response.status.message = "Failed to generate token";
                            }
                            break;
                        }


                    case LoginFormMode.MaxValue :
                        //two wheeler loan sticker updation authority verification
                        //done by Sreerekha K 100006 --- 5-jul-2019
                        // login = employeeValidation_StickerUpdation(request.employeeID, request.password, request.branchID);
                        //login = 1;
                        if (login == 1)
                        {
                            response.loginStatus = LoginStatus.validUser;
                            response.status.code = APIStatus.success;
                            response.status.message = "Success";
                            response.status.flag = ProcessStatus.success;
                        }
                        break;
                }


            }
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }


        /// <Created>Uneesh - 100156</Created>
        /// <summary>Verify OTP</summary> 

        public LoginResponse verifyOTP(VerifyLoginOTPRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                OracleParameter[] parm_coll = new OracleParameter[14];
                //DBAccessHelper helper = new DBAccessHelper();
                parm_coll[0] = new OracleParameter("var_moblieno", OracleDbType.Varchar2,15);
                parm_coll[0].Value = request.moblieNo;
                parm_coll[0].Direction = ParameterDirection.Input;

                parm_coll[1] = new OracleParameter("var_otpno", OracleDbType.Varchar2,15);
                parm_coll[1].Value = request.otpno;
                parm_coll[1].Direction = ParameterDirection.Input;

                parm_coll[2] = new OracleParameter("var_loginkey", OracleDbType.Varchar2,100);
                parm_coll[2].Value = request.loginkey;
                parm_coll[2].Direction = ParameterDirection.Input;

                parm_coll[3] = new OracleParameter("var_token_duration", OracleDbType.Long,30);
                parm_coll[3].Value = (int)Durations.LoginOTP;
                parm_coll[3].Direction = ParameterDirection.Input;

                parm_coll[4] = new OracleParameter("var_token", OracleDbType.Varchar2,500);
                parm_coll[4].Value = request.token;
                parm_coll[4].Direction = ParameterDirection.Input;

                parm_coll[5] = new OracleParameter("var_signature_data", OracleDbType.Varchar2,500);
                parm_coll[5].Value = request.signature;
                parm_coll[5].Direction = ParameterDirection.Input;

                parm_coll[6] = new OracleParameter("firmID", OracleDbType.Long,3);
                parm_coll[6].Direction = ParameterDirection.Output;

                parm_coll[7] = new OracleParameter("firmName", OracleDbType.Varchar2, 100);
                parm_coll[7].Direction = ParameterDirection.Output;

                parm_coll[8] = new OracleParameter("empCode", OracleDbType.Long,10);
                parm_coll[8].Direction = ParameterDirection.Output;

                parm_coll[9] = new OracleParameter("employeeName", OracleDbType.Varchar2, 80);
                parm_coll[9].Direction = ParameterDirection.Output;

                parm_coll[10] = new OracleParameter("var_branchID", OracleDbType.Long,5);
                parm_coll[10].Direction = ParameterDirection.Output;

                parm_coll[11] = new OracleParameter("branchName", OracleDbType.Varchar2, 50);
                parm_coll[11].Direction = ParameterDirection.Output;

                parm_coll[12] = new OracleParameter("postID", OracleDbType.Long,3);
                parm_coll[12].Direction = ParameterDirection.Output;
                
                parm_coll[13] = new OracleParameter("out_status", OracleDbType.Long,3);
                parm_coll[13].Direction = ParameterDirection.Output;
                

                helper.ExecuteNonQuery("proc_vaildateloginotp", parm_coll);
                if (Convert.ToInt64(parm_coll[13].Value.ToString()) == 1)
                {
                    response.loginStatus = LoginStatus.validUser;
                    response.firmID = Convert.ToInt32(parm_coll[6].Value.ToString());
                    response.firmName = parm_coll[7].Value.ToString();
                    response.empCode = Convert.ToInt32(parm_coll[8].Value.ToString());
                    response.employeeName = parm_coll[9].Value.ToString();
                    response.branchID = Convert.ToInt32(parm_coll[10].Value.ToString());
                    response.branchName = parm_coll[11].Value.ToString();
                    response.postID = Convert.ToInt32(parm_coll[12].Value.ToString());
                    //response.roleID = Convert.ToInt32(parm_coll[8].Value.ToString());

                    response.token = request.token;
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.status.code = APIStatus.failed;
                    response.status.message = "Invalid request";
                    response.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }
            return response;
        }


        /// <Created>Uneesh - 100156</Created>
        /// <summary>ReSend OTP</summary> 
        public ResendLoginOTPResponse reSendOTP(ResendLoginOTPRequest request)
        {
            ResendLoginOTPResponse response = new ResendLoginOTPResponse();
            try
            {
                OracleParameter[] parm_coll = new OracleParameter[5];
                //DBAccessHelper helper = new DBAccessHelper();
                parm_coll[0] = new OracleParameter("var_employee", OracleDbType.Long);
                parm_coll[0].Value = request.empCode;
                parm_coll[0].Direction = ParameterDirection.Input;
                parm_coll[1] = new OracleParameter("var_loginkey", OracleDbType.Varchar2,100);
                parm_coll[1].Value = request.loginKey;
                parm_coll[1].Direction = ParameterDirection.Input;
                parm_coll[2] = new OracleParameter("var_moblieno", OracleDbType.Varchar2,15);
                parm_coll[2].Direction = ParameterDirection.Output;
                parm_coll[3] = new OracleParameter("var_otpno", OracleDbType.Varchar2,15);
                parm_coll[3].Direction = ParameterDirection.Output;
                parm_coll[4] = new OracleParameter("out_status", OracleDbType.Long, 3);
                parm_coll[4].Direction = ParameterDirection.Output;
                helper.ExecuteNonQuery("proc_resendloginotp", parm_coll);
                if (Convert.ToInt64(parm_coll[4].Value.ToString()) == 1)
                {
                    response.status.code = APIStatus.success;
                    response.status.message = "OTP successfully resent";
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.status.code = APIStatus.failed;
                    response.status.message = "Invalid request";
                    response.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }
            return response;
        }

        /// <Created>Uneesh - 100156</Created>
        /// <summary>User Logout</summary> 
        public LogoutResponse userLogout(LogoutRequest logoutRequest)
        {
            LogoutResponse logoutResponse = new LogoutResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string query = "update LOGIN_TOKEN_HANDLER set status = "+ TokenStatus.manualExpired + " where emp_code = "+ logoutRequest.employeeCode + " and token = '"+ logoutRequest.token + "' and status = " + TokenStatus.open + "";
                helper.ExecuteNonQuery(query);
                logoutResponse.status.code = APIStatus.success;
                logoutResponse.status.message = "Success";
                logoutResponse.status.flag = ProcessStatus.success;

            }
            catch(Exception ex)
            {
                logoutResponse.status.flag = ProcessStatus.failed;
                logoutResponse.status.code = APIStatus.exception;
                logoutResponse.status.message = ex.Message;

            }
            return logoutResponse;
        }

        #region "Employee Logins"

        /// <Created>Uneesh - 100156</Created>
        /// <summary>Employee Validation Repledge</summary> 
        public int employeeValidation_Repledge(int userID, string password, int branchID,int bio_flag=0)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            string sql;
            int ans = 0;
            long cn = 0;
            ////PHelper.passwdClass ps = new PHelper.passwdClass();
            // int cn = 1;
            ////cn = ps.password_chek(System.Convert.ToInt32(usid), pswd);
            if (bio_flag==0)
                { 
            PHelper ps = new PHelper();
            
            bool emp = employee.CheckUser(userID, password);

            cn = emp ? 1 : 0;
            }
           else
            { cn = 1; }

            if (System.Convert.ToInt32(branchID) == 636 | System.Convert.ToInt32(branchID) == 500)
                sql = "select a.emp_code from employee_master a,daily_attend b where a.emp_code=b.emp_code and a.emp_code="
                    + userID + " and a.status_id=1 union all select t.emp_code from employee_master t where  status_id=1 and t.emp_code="
                    + userID + " and t.emp_code in(select aa.area_head_id from area_master aa where aa.area_id in"
                    + "(select area_id from area_detail cc where cc.branch_id=" + branchID + ")) and post_id in(136,197,201)";
            else
                // ----change for auditors
                // house keeping asst bloc wef 4-dec-2010
                // sql = "select a.emp_code from employee_master a,daily_attend b where a.emp_code=b.emp_code and a.emp_code=" & usid & " and b.m_branch=" & brid & " and a.status_id=1 and  a.department_id not in (4,23,37,133,178,179,180,183,188,189,211) and (instr(b.m_time,':',1,2)>0 or (b.pay_id in(50,51,52)) or m_time='JOIN' or m_time='REG') union all select t.emp_code from employee_master t where  status_id=1 and t.emp_code=" & usid & " and t.emp_code in(select aa.area_head_id from area_master aa where aa.area_id in(select area_id from area_detail cc where cc.branch_id=" & brid & ")) and post_id in(136,197,201)"
                // permitted only for GL department in branch wef 12-jun-2011
                // sql = "select a.emp_code from employee_master a,daily_attend b where a.post_id<>53 and a.emp_code=b.emp_code and a.emp_code=" & usid & " and b.m_branch=" & brid & " and a.status_id=1 and  a.department_id not in (23,37,133,179,180,183,189,211,300) and (instr(b.m_time,':',1,2)>0 or (b.pay_id in(50,51,52)) or m_time='JOIN' or m_time='REG') union all select t.emp_code from employee_master t where  status_id=1 and t.emp_code=" & usid & " and t.emp_code in(select aa.area_head_id from area_master aa where aa.area_id in(select area_id from area_detail cc where cc.branch_id=" & brid & ")) and post_id in(136,197,201)"
                //sql = "select a.emp_code from employee_master a,daily_attend b where a.post_id<>53 and a.emp_code=b.emp_code and a.emp_code=" + userID + " and b.m_branch=" + branchID + " and a.status_id=1 and  a.department_id in (select department_id  from  goldloan_department) and (instr(b.m_time,':',1,2)>0 or (b.pay_id in(50,51,52)) or m_time='JOIN' or m_time='REG') union all select t.emp_code from employee_master t where  status_id=1 and t.emp_code=" + userID + " and t.emp_code in(select aa.area_head_id from area_master aa where aa.area_id in(select area_id from area_detail cc where cc.branch_id=" + branchID + ")) and post_id in(136,197,201)";
                //above line commented and added AH punching checking as per the observation of Deloitte added by Remya P. 
                sql ="select a.emp_code from employee_master a, aml_users.tbl_daily_attend b where a.post_id <> 53 and a.emp_code = b.emp_code and a.emp_code = " + userID + " and b.m_branch = " + branchID + " and a.status_id = 1 and a.department_id in (select department_id from goldloan_department) and(instr(b.m_time, ':', 1, 2) > 0 or(b.pay_id in (50, 51, 52)) or m_time = 'JOIN' or m_time = 'REG') union select t.emp_code from employee_master t, aml_users.tbl_daily_attend t1 where t.status_id = 1 and t.emp_code = " + userID + " and t1.emp_code = t.EMP_CODE and t.emp_code in (select aa.area_head_id from area_master aa where aa.area_id in (select area_id from area_detail cc where cc.branch_id = " + branchID + ")) and t.post_id in (136) and(instr(t1.m_time, ':', 1, 2) > 0 or(t1.pay_id in (50, 51, 52)) or t1.m_time = 'JOIN' or t1.m_time = 'REG')";
            DataSet dt;
            dt = helper.ExecuteDataSet(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (cn > 0)
                    ans = 1;
                else
                    ans = 0;
            }
            else
                ans = 0;

            return ans;
        }

        /// <Created>Uneesh - 100156</Created>
        /// <summary>Employee Validation ABH/BH</summary> 
        public int employeeValidation_ABH_BH(int userID, string password, int branchID,int biometric)//70009507-biometric
        {
            //DBAccessHelper helper = new DBAccessHelper();
            string sql;
            int ans = 0;
            // --change for server change 05-11-2010
            sql = "select a.emp_code from employee_master a,daily_attend b where a.emp_code=b.emp_code and a.emp_code=" 
                + userID + " and b.m_branch=" + branchID + " and a.status_id=1 and a.post_id in(1,10,198) and " +
                "(instr(m_time,':',1,2)>0 or (pay_id in(50,51,52)) or m_time='JOIN'or m_time='REG') union all " +
                "select t.emp_code from employee_master t where  status_id=1 and t.emp_code=" + userID 
                + " and post_id in(136,197,201)";
            // sql = "select a.emp_code from employee_master a where  a.emp_code=" & usid & "  and a.status_id=1 and a.post_id in(1,10,198)  union all select t.emp_code from employee_master t where  status_id=1 and t.emp_code=" & usid & " and post_id in(136,197,201)"

            DataSet dt;
            //     PHelper.passwdClass ps = new PHelper.passwdClass();        
            // int cn = 1;
            PHelper ps = new PHelper();
            long cn = 0;
            if (biometric == 0)
            {
                bool emp = employee.CheckUser(userID, password);

                cn = emp ? 1 : 0;
            }
            else
            {
                cn = 1;
            }

            dt = helper.ExecuteDataSet(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (cn > 0)
                    ans = 1;
                else
                    ans = 0;
            }
            else
                ans = 0;
            return ans;
        }

        /// <Created>Uneesh - 100156</Created>
        /// <summary>Employee Validation FormK</summary> 
        public int employeeValidation_FormK(int userID, string password, int branchID,int biometric)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            string sql;
            int ans = 0;

            sql = "select a.emp_code from employee_master a,daily_attend b where  a.post_id<>53 and a.emp_code=b.emp_code and a.emp_code=" + userID + " and b.m_branch=" + branchID + " and a.status_id=1 and a.department_id not in (4, 23, 37, 133, 178, 179, 180, 183, 188, 189, 211,300) and (instr(b.m_time,':',1,2)>0 or (b.pay_id in(50,51,52)) or m_time='JOIN'or m_time='REG')";

            DataTable dt;
            //PHelper.passwdClass ps = new PHelper.passwdClass();
            PHelper ps = new PHelper();
            long cn=0;
            if (biometric == 0)
            {
                bool emp = employee.CheckUser(userID, password);

                cn = emp ? 1 : 0;
            }
            else
            {
                cn = 1;
            }
            dt = helper.ExecuteDataSet(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (cn > 0)
                    ans = 1;
                else
                    ans = 0;
            }
            else
                ans = 0;
            return ans;
        }
        //two wheeler loan sticker updation authority verification
        //done by Sreerekha K 100006 --- 5-jul-2019
        public int employeeValidation_StickerUpdation(int userID, string password, int branchID)
        {
            string sql;
            int ans = 0;
             
            sql = "select t.emp_code from employee_master t where  t.emp_code=" + userID + " and t.status_id=1 and (t.post_id in(136, 197, 201,155,210,202,236, 346,204,247,199,321) or t.department_id in(4,490,466,23,37,178,179,183,188,189,211,330,517) or (branch_id=" + branchID + "  ))";
             
            DataSet dt;
             
            PHelper ps = new PHelper();
            long cn = 0;
            bool emp = employee.CheckUser(userID, password);

            cn = emp ? 1 : 0;

            dt = helper.ExecuteDataSet(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (cn > 0)
                    ans = 1;
                else
                    ans = 0;
            }
            else
                ans = 0;
            return ans;
        }


        #endregion

        public int employeeValidation_doorstep(int userID, string password, int branchID,int biometric)
        {
            int ans = 0;
            PHelper ps = new PHelper();
            long cn = 0;
            if(biometric==0)
            {
                bool emp = employee.CheckUser(userID, password);

                cn = emp ? 1 : 0;
            }

            else
            {
                cn = 1;
            }
                if (cn > 0)
                    ans = 1;
                else
                    ans = 0;
           
            return ans;
        }
    }
}
