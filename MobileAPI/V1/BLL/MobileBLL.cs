using System;
using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GlobalValues;
using MobileAPI.V1.Models.Request;
using MobileAPI.V1.Models.Response;
using System.Data;
using static GlobalValues.GlobalVariables;
using Oracle.ManagedDataAccess.Client;
using APIBaseClassLibrary.V1.Models.Request;
using APIBaseClassLibrary.V1.Models.Response;
using static GlobalValues.GlobalVariables.Constants;
using MobileAPI.V1.BLLDependency;

namespace MoblieAPI.V1.BLL
{
    public class MobileBLL : APIBaseBLL , IMobileBLL
    {
        
      
        public IDBAccessHelper helper;
        public MobileBLL(IDBAccessHelper _helper)
        {
            helper = _helper;
        }


        public string generateOTP()
        {
                int lenthofpass = 4;
                string allowedChars = "";
                allowedChars = "1,2,3,4,5,6,7,8,9,0,";
                char[] sep = new[] { ',' };
                string[] arr = allowedChars.Split(sep);
                string passwordString = "";
                string temp = "";
                Random rand = new Random();
                for (int i = 0; i <= lenthofpass - 1; i++)
                {
                    temp = arr[rand.Next(0, arr.Length)];
                    passwordString += temp;
                }
                return passwordString;
        }


        /// <Created>Aravind  - 100231</Created>
        /// <summary>Send Otp</summary> 
        public SendOTPResponse sendOtp(SendOTPRequest sendOTPRequest)
        {
            SendOTPResponse sendOTPResponse = new SendOTPResponse();
            DataSet ds = new DataSet();
            string flg = string.Empty;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^([6-9]{1})([0-9]{9})$");
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();

                    flg = ChkExistOtpCust(sendOTPRequest.mobile);
                    if (flg != "0")
                    {
                        sendOTPResponse.status.code = APIStatus.failed;
                        sendOTPResponse.status.message = OtpValidationMessages.customerMobNoAlreadyExist;
                        sendOTPResponse.status.flag = ProcessStatus.success;
                        return sendOTPResponse;
                    }
                    ds = CheckCoMob(sendOTPRequest.mobile);
                    if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                    {
                        sendOTPResponse.status.code = APIStatus.failed;
                        sendOTPResponse.status.message = OtpValidationMessages.notAllowedCompanyNumber;
                        sendOTPResponse.status.flag = ProcessStatus.success;
                        return sendOTPResponse;
                    }
                    else
                    {
                        flg = EmpMobCheck(sendOTPRequest.mobile);
                    //if (flg == "0" || (sendOTPRequest.employeeCode!= "0" && flg == sendOTPRequest.employeeCode) )
                        if (flg == "0" ||  flg == sendOTPRequest.employeeCode)
                        {
                            string otp = generateOTP();
                            int formModeValue = 0;
                            if(sendOTPRequest.formMode== FormModeMobile.Add_Customer)
                            {
                                formModeValue = 1;
                            }
                            else
                            {
                                formModeValue = 2;
                            }
                            var SendSmsmessage = smsOTP(otp, Convert.ToInt64(sendOTPRequest.mobile), sendOTPRequest.type, formModeValue, Convert.ToInt64(sendOTPRequest.oldMobile));
                            InsertOtp(otp.ToString(), Convert.ToInt64(sendOTPRequest.mobile), sendOTPRequest.employeeCode);
                            sendOTPResponse.otp = otp;
                            sendOTPResponse.status.code = APIStatus.success;
                            sendOTPResponse.status.message = "Success";
                            sendOTPResponse.status.flag = ProcessStatus.success;

                        }
                        else if (flg != "0" & flg != sendOTPRequest.employeeCode & (sendOTPRequest.employeeCode == null || sendOTPRequest.employeeCode == ""))
                        {
                            sendOTPResponse.status.code = APIStatus.failed;
                            sendOTPResponse.status.message = OtpValidationMessages.customerAsEmployee;
                            sendOTPResponse.status.flag = ProcessStatus.success;
                           
                        }
                        else
                        {
                            sendOTPResponse.status.code = APIStatus.failed;
                            sendOTPResponse.status.message = OtpValidationMessages.employeeMobileNumberMissmatch;
                            sendOTPResponse.status.flag = ProcessStatus.success;
                           

                        }
                    }

                
            }
            catch (Exception ex)
            {
                sendOTPResponse.status.code = APIStatus.success;
                sendOTPResponse.status.message = "Success";
                sendOTPResponse.status.flag = ProcessStatus.success;

            }
            return sendOTPResponse;
        }

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Verify OTP</summary> 
        public VerifyOTPResponse verifyOTP(VerifyOTPRequest verifyOTPRequest)
        {
            VerifyOTPResponse verifyOTPResponse = new VerifyOTPResponse();
          //  DBAccessHelper helper = new DBAccessHelper();
            DataSet dt = new DataSet();
            try
            {
                if (verifyOTPRequest.checkOTP == OTPFlags.Customer_OTP)
                {

                    var query = "select count(*) from TBL_CUST_OTP_DETAILS_tmp where MOBILE='" + verifyOTPRequest.mobile + "' and OTP='" + verifyOTPRequest.otp + "' and  ENDTIME >= sysdate";

                    decimal validteOtp = helper.ExecuteScalar<decimal>(query);

                    if (validteOtp > 0)
                    {
                        string strOTPValue = verifyOTPRequest.otp;
                        string strMobileNumOTP = verifyOTPRequest.mobile;
                        dt = getBaDtls(verifyOTPRequest.mobile);
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            verifyOTPResponse.status.code = APIStatus.success;
                            verifyOTPResponse.status.message = "This mobile number has referred by '" + dt.Tables[0].Rows[0][1].ToString() + "' | BA Code : '" + dt.Tables[0].Rows[0][0].ToString() + "'";
                            verifyOTPResponse.status.flag = ProcessStatus.success;

                        }
                        else
                        {
                            verifyOTPResponse.status.code = APIStatus.success;
                            verifyOTPResponse.status.message = "OTP verified successfully";
                            verifyOTPResponse.status.flag = ProcessStatus.success;


                        }
                    }
                    else
                    {
                        verifyOTPResponse.status.code = APIStatus.failed;
                        verifyOTPResponse.status.message = "OTP not verified";
                        verifyOTPResponse.status.flag = ProcessStatus.success;
                    }
                }
                else if (verifyOTPRequest.checkOTP == OTPFlags.Employee_Code)
                {

                    string strOTPValue = verifyOTPRequest.otp;
                    string strMobileNumOTP = verifyOTPRequest.mobile;
                    if (verifyOTPRequest.otp == verifyOTPRequest.empCode) {
                        dt = getBaDtls(verifyOTPRequest.mobile);
                        if (dt.Tables[0].Rows.Count > 0 && dt.Tables[0].Rows[0][1] != null)
                        {
                            verifyOTPResponse.status.code = APIStatus.success;
                            verifyOTPResponse.status.message = "This mobile number has referred by '" + dt.Tables[0].Rows[0][1].ToString() + "' | BA Code : '" + dt.Tables[0].Rows[0][0].ToString() + "'";
                            verifyOTPResponse.status.flag = ProcessStatus.success;

                        }
                        else
                        {
                            verifyOTPResponse.status.code = APIStatus.success;
                            verifyOTPResponse.status.message = "Empcode verified successfully";
                            verifyOTPResponse.status.flag = ProcessStatus.success;

                        }
                    }
                    else
                    {
                        verifyOTPResponse.status.code = APIStatus.success;
                        verifyOTPResponse.status.message = "Empcode not matching";
                        verifyOTPResponse.status.flag = ProcessStatus.success;
                    }

                }
                else
                {
                    verifyOTPResponse.status.code = APIStatus.failed;
                    verifyOTPResponse.status.message = "Invalid request";
                    verifyOTPResponse.status.flag = ProcessStatus.success;

                }

            }
            catch (Exception ex)
            {
                verifyOTPResponse.status.flag = ProcessStatus.failed;
                verifyOTPResponse.status.code = APIStatus.exception;
                verifyOTPResponse.status.message = ex.Message;
            }

            return verifyOTPResponse;

        }


        /// <Created>Aravind  - 100231</Created>
        /// <summary>Get business agent details</summary> 
        private DataSet getBaDtls(string ConfirmMobileOTP)
        {
            DataSet dataSet = new DataSet();
           // DBAccessHelper helper = new DBAccessHelper();
            dataSet = helper.ExecuteDataSet("select t.ba_code, b.ba_name  from EMP_REFERENCE t, tbl_add_businessagent b where t.ba_code = b.ba_code  and t.ba_code is not null and t.ref_phn1 = '" + ConfirmMobileOTP + "'  and t.ref_dt <= (sysdate - 12 / 24)");
            return dataSet;
        }


        #region BusinessAssociates

        #region getbaMobile

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Get business agentMobile details</summary> 
        public MobileResponse getbaMobile(MobileRequest request)
        {
            MobileResponse response = new MobileResponse();
            try
            {
             //   DBAccessHelper helper = new DBAccessHelper();
                DataSet dt = new DataSet();
                if (request.mobileNo==string.Empty)
                {
                    response.message = "Enter Mobile Number..!!";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Failed";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }

                var query = "select count(*)  from CUSTOMER where phone2='"+ request.mobileNo + "' and isactive <> 3";
                decimal mobilecnt = helper.ExecuteScalar<decimal>(query);
                if (mobilecnt > 0)
                {
                    response.message = "This lead customer aleady existing with us..!!";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Failed";
                    response.status.flag = ProcessStatus.success;
                    return response;

                }

                dt = getRefferenceDtls(request.mobileNo);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    response.message = "This Mobile Number has reffered by " + dt.Tables[0].Rows[0][1].ToString() + " ! BA code : " + dt.Tables[0].Rows[0][0].ToString() + ". But not crossed 12 Hour";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Failed";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
                else
                {

                    var baquery = "select count(*) from EMP_REFERENCE t where ref_phn1 = '" + request.mobileNo + "' and t.ref_dt <= (sysdate - 12 / 24) and t.ba_code is not null";
                    decimal bacnt = helper.ExecuteScalar<decimal>(baquery);
                    if (bacnt == 0) // bug reported by mafil BA department... Bug resolved by SReerekha K 100006 on 17-mar-2020                    {
                    {   response.message = "There is no lead found for this customer..!!";
                        response.status.code = APIStatus.failed;
                        response.status.message = "Failed";
                        response.status.flag = ProcessStatus.success;
                        return response;
                    }
                    else
                    {
                        response.message = "Success";
                        response.status.code = APIStatus.success;
                        response.status.message = "Success";
                        response.status.flag = ProcessStatus.success;
                        return response;

                    }
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
        #endregion getbaMobile

        #region getbarefference


        /// <Created>Aravind  - 100231</Created>
        /// <summary>Get Refference details</summary> 
        private DataSet getRefferenceDtls(string mobile)
        {
            DataSet dataSet = new DataSet();
          //  DBAccessHelper helper = new DBAccessHelper();
            dataSet = helper.ExecuteDataSet("select t.ba_code, b.ba_name  from EMP_REFERENCE t, tbl_add_businessagent b where t.ba_code = b.ba_code  and t.ba_code is not null and t.ref_phn1 = '" + mobile + "'  and t.ref_dt > (sysdate - 12 / 24)");
            return dataSet;
        }
        #endregion getbarefference

        #endregion BusinessAssociates

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Check Existing otp customer </summary> 
        public string ChkExistOtpCust(string mob)
        {
            string  flg = string.Empty;
            try
            {
              //  DBAccessHelper helper = new DBAccessHelper();
                DataTable dt = new DataTable(), dt1 = new DataTable(), dt2 = new DataTable();
                dt = helper.ExecuteDataSet("select count(*)  from TBL_CUST_OTP_DETAILS t, customer c where t.cust_id = c.cust_id   and c.isactive in (1,0,null)  and t.otp is not null   and c.phone2 = t.mob   and c.phone2 = '" + mob + "'").Tables[0];
                dt1 = helper.ExecuteDataSet("select count(*) from customer c where c.isactive in (1,0,null) and c.phone2 = '" + mob + "'").Tables[0];
                dt2 = helper.ExecuteDataSet("select count(*) from KYC_PRE_AUTH t where t.change_type=3 and t.phone2='" + mob + "' and t.aprvd_by is not null and t.status_id=2").Tables[0];
                // 101092 | Putting additional control on using same number for multiple customer ID Creation -- 16/Dec/2020 --  repetitive use of phone numbers for customer creation preferably more than 5 times.
                if (Convert.ToInt32(dt.Rows[0][0]) > 0 || Convert.ToInt32(dt1.Rows[0][0]) >= 2 || Convert.ToInt32(dt2.Rows[0][0]) >=5)
                    flg = "1";
                else
                    flg = "0";
            }catch(Exception ex)
            {
                flg = "0";
            }
            return flg;
        }
        /// <Created>Aravind  - 100231</Created>
        /// <summary>Check company mobile </summary> 
        public DataSet CheckCoMob(string mob)
        {
            DataSet dt = new DataSet();
            try
            {
               // 101092 | Putting additional control on using same number for multiple customer ID Creation -- 16/Dec/2020 -- Removed device_type checking
                dt = helper.ExecuteDataSet("select count(*) from (select t.device_no mob from TBL_INST_ASSIGN t  union all select m.mobile_no mob  from mobile_master m) a where a.mob = '" + mob + "'");
                
            }catch(Exception ex)
            {
                
            }
            return dt;
        }

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Check employee mobile check </summary> 
        public string EmpMobCheck(string mob)
        {
            DataTable dt = new DataTable();
            string flg = "0";
            try
            {
               // DBAccessHelper helper = new DBAccessHelper();
                dt = helper.ExecuteDataSet("select g.mobile_no, g.emp_code  from emp_greeting_master g, emp_master e  where g.emp_code = e.EMP_CODE and e.STATUS_ID = 1 and g.mobile_no = '" + mob + "'").Tables[0];
            if (dt.Rows.Count > 0)
                flg = dt.Rows[0][1].ToString();
            else
                flg = "0";
            }
            catch (Exception ex)
            {
                flg = "0";
            }
            return flg;
        }

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Sms Otp</summary> 
        public string smsOTP(string strOTP, long strMobile, int Type,int formMode, long oldMobile)
        {
            string strResult = string.Empty;

            SmsRequest ds = new SmsRequest();

            SmsResponse dres = new SmsResponse();

           


           // solution_infini_flag.mana.SMSTool sms = new solution_infini_flag.mana.SMSTool();
            try
            {
                if (Type == 0)
                {
                    ds.Message = "Dear Customer, Your verification code is " + strOTP + ", for AsirvadGoldLoan";
                        //"Dear Client, Welcome to Asirvad Family!Your reg. code-" + strOTP + ".You can also visit www.manappuram.com or download our mobile app.for loan tracking& payments"; 
                    ds.mobileNo = Convert.ToString(strMobile);
                    ds.ApiDetail = "1";
                    ds.accountType = SMSAccMode.OtpSms;

                    dres = base.SendSMS(ds);
                    strResult = Convert.ToString(dres.Outmessage);
                    //sms.Message = "Dear Client, Welcome to Manappuram Family!Your reg. code-" + strOTP + ".You can also visit www.manappuram.com or download our mobile app.for loan tracking& payments";
                    //sms.mobileNumber = strMobile;
                    //sms.account_id = 3;
                    //sms.ser_flag = 0;
                    //strResult = sms.SendSms();

                    if(formMode==2)
                    {
                        ds.Message = "As requested your mobile number has been updated .If you  have not requested kindly contact our customer service cell Toll free No 18004202233";
                        ds.mobileNo = Convert.ToString(oldMobile);
                        ds.ApiDetail = "1";
                        ds.accountType = SMSAccMode.OtpSms;

                        dres = base.SendSMS(ds);
                        strResult = Convert.ToString(dres.Outmessage);
                    }

                
                }
                else
                {
                    ds.Message = "Dear Customer, Your verification code is " + strOTP + ", for AsirvadGoldLoan";

                    //ds.Message = "Dear Client, Welcome to Asirvad Family!Your reg. code-" + strOTP + "."; //You can also visit www.manappuram.com or download our mobile app.for loan tracking& payments
                    ds.mobileNo = Convert.ToString(strMobile);
                    ds.ApiDetail = "1";
                    ds.accountType = SMSAccMode.OtpSms;

                    dres = base.SendSMS(ds);
                    strResult = Convert.ToString(dres.Outmessage);
                    //sms.Message = "Dear Client, Welcome to Manappuram Family!Your reg. code-" + strOTP + ".You can also visit www.manappuram.com or download our mobile app.for loan tracking& payments";
                    //sms.mobileNumber = strMobile;
                    //sms.account_id = 3;
                    //sms.ser_flag = 1;
                    //strResult = sms.SendSms();

                    if (formMode == 2)
                    {
                        ds.Message = "As requested your mobile number has been updated .If you  have not requested kindly contact our customer service cell Toll free No 18004202233";
                        ds.mobileNo = Convert.ToString(oldMobile);
                        ds.ApiDetail = "1";
                        ds.accountType = SMSAccMode.OtpSms;

                        dres = base.SendSMS(ds);
                        strResult = Convert.ToString(dres.Outmessage);
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Insert Otp</summary> 
        public void InsertOtp(string strOTP, long strMobile,string custEmpCode)
        {
            try
            {
                DataSet dt = new DataSet();
              //  DBAccessHelper helper = new DBAccessHelper();
                var otpQuery = string.Empty;
                DateTime endTime = DateTime.Now.AddMinutes(Convert.ToDouble(Durations.LoginOTP));
                dt = helper.ExecuteDataSet("select count(*) from TBL_CUST_OTP_DETAILS_tmp where MOBILE='" + strMobile + "'");
                if (Convert.ToInt64(dt.Tables[0].Rows[0][0])>0)
                {
                    otpQuery = "UPDATE TBL_CUST_OTP_DETAILS_tmp SET OTP='"+ strOTP + "', UPDATEDBY ="+ custEmpCode + ", UPDATEDATE= sysdate , DURATION = "+ Convert.ToDouble(Durations.LoginOTP) + ", STARTTIME = sysdate , ENDTIME = (sysdate  + " + (.000694 * Convert.ToDouble(Durations.LoginOTP)) + ")  where MOBILE ='" + strMobile + "' ";
                    helper.ExecuteNonQuery(otpQuery);
                }
                else
                {
                    otpQuery = "INSERT INTO TBL_CUST_OTP_DETAILS_tmp(MOBILE,OTP,UPDATEDBY,UPDATEDATE,DURATION,STARTTIME,ENDTIME) VALUES(" + strMobile + ",'" + strOTP + "'," + custEmpCode + ",sysdate,"+ Convert.ToDouble(Durations.LoginOTP) + ",sysdate, (sysdate + " + (.000694 * Convert.ToDouble(Durations.LoginOTP)) + "))";
                    helper.ExecuteNonQuery(otpQuery);
                }
            }
            catch(Exception ex)
            {


            }

        }
    }
}
