using APIBaseClassLibrary.V1.Models.Request;
using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using static APIBaseClassLibrary.V1.Models.Response.SmsResponse;
using static GlobalValues.GlobalVariables;


namespace APIBaseClassLibrary.V1.BLL
{
    public class APIBaseBLL
    {
        protected SmsResponse SendSMS(SmsRequest request)
        {
               SmsResponse response = new SmsResponse();

            try
            {
               // ServiceReference1.Service1SoapClient.EndpointConfiguration endpoint = new ServiceReference1.Service1SoapClient.EndpointConfiguration();
               // ServiceReference1.Service1SoapClient myService = new ServiceReference1.Service1SoapClient(endpoint, "https://onpay.online.manappuram.com/sms_services/smsservice.asmx");
               //// ServiceReference1.Service1SoapClient myService = new ServiceReference1.Service1SoapClient(endpoint, "https://goldservices.manappuram.net/sms_services/SMSService.asmx");

               // //myService.sp_mchit_smsAsync(80892, "Mafil sms send form api through");


               // request.mobileNo = request.mobileNo;
               // //ServiceReference1.ServiceSoapClient.EndpointConfiguration serv = new ServiceReference1.ServiceSoapClient.EndpointConfiguration();
               // //serv.sp_mchit_smsAsync()
               
               // object messages = "";
               // // BHelper.SMS sms = new BHelper.SMS();
               // messages = myService.SendOTPAsync(request.Message, request.mobileNo, 0, Convert.ToString((int)request.accountType), "");
               // // messages = myService.SendOTPAsync(request.Message, "8089213243", (int)request.accountType, "", "");
               // // messages = sms.sendSMS(request.firmID, (int)request.accountType, request.Message, request.mobileNo, code, sender);
                response.Outmessage = SendOTP(request);
                response.status.message = "Success";
                response.status.code = APIStatus.success;
                response.status.flag = ProcessStatus.success;
            }
            catch (Exception e)
            {

                response.status.code = APIStatus.exception;
                response.status.message = e.ToString();
                response.status.flag = ProcessStatus.failed;
            }
            return response;
        }
        public SmsResponse SendOTP(SmsRequest request)
        {
            SmsResponse response = new SmsResponse();
            string language =  "";
            string baseUrl = "https://otp2.aclgateway.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=asirvotp&subEnterpriseid=asirvotp&pusheid=asirvotp&pushepwd=asirvotp18&msisdn=" + request.mobileNo + "&sender=AMLOTP&msgtext=" + request.Message + "&name=AMLOTP&ctype=1&language=" + language;
            HttpResponseMessage HttpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            SentStatus sent = new SentStatus();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    cts.CancelAfter(httpClient.Timeout);
                    httpClient.DefaultRequestHeaders.Clear();
                    HttpResponse = httpClient.PostAsync(baseUrl, null).Result;

                    if (HttpResponse.IsSuccessStatusCode)
                    {
                        jsonResponse = HttpResponse.Content.ReadAsStringAsync().Result.ToString();

                        response.Outmessage = jsonResponse;
                        response.Outmessage += "; Response: " + HttpResponse.ToString();
                        //sent.message = "Success";
                        //sent.code = "Success";
                        //sent.flag = "Success";
                        //response.status = sent;
                    }
                    else
                    {
                        jsonResponse = HttpResponse.Content.ReadAsStringAsync().Result.ToString();
                        response.Outmessage = jsonResponse;
                        //sent.code = "Failed";
                        //sent.message = "SMS Sent Failed";
                        //sent.flag = "Failed";
                        //response.status = sent;
                    }
                }

            }
            catch (Exception ex)
            {
                jsonResponse = HttpResponse.ToString();
                response.Outmessage = jsonResponse;
                sent.code = "Failed";
                sent.message = ex.Message;
                sent.flag = "Failed";
                //response.status = sent;
            }

            #region SMS Log
            //string sql = "insert into  TBL_SMS_LOGS_CGL(TYPE_ID,MOBILE_NO,OTP,SEND_DATE,SMS,SEND_RESULT,USER_ID,SMS_URL) values(0,'" + request.mobileNo + "'," + request.firmId + ",sysdate,'" + request.Message + "','" + jsonResponse.Replace("'", "") + "',0,'" + baseUrl + "')";
            //oh.ExecuteNonQuery(sql, null);
            #endregion

            return response;
        }
        protected bool isValidDataset(DataSet ds)
        {
            bool retVal = false;
            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            retVal = true;
                        }
                    }
                }
            }
            catch (Exception e) { }
            return retVal;
        }

        protected bool isValidResponse(BaseResponse response)
        {
            bool retVal = false;
            try
            {
                if (response != null)
                {
                    if (response.status != null)
                    {
                        if (response.status.flag == ProcessStatus.success)
                        {
                            if (response.status.code == APIStatus.success)
                            {
                                retVal = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e) { }
            return retVal;
        }
    }

}

