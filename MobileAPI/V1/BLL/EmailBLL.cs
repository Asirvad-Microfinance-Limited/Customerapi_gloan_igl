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
//using EASendMail;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;

using System.Net.Mail;

namespace MoblieAPI.V1.BLL
{
    public class EmailBLL : APIBaseBLL , IEmailBLL
    {
        
      
        public IDBAccessHelper helper;
        public EmailBLL(IDBAccessHelper _helper)
        {
            helper = _helper;
        }
        
        public string generateOTP()
        {
                int lenthofpass = 6;
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


        /// <Created>Sreerekha  - 100006  1-feb-2020</Created>
        /// <summary>Send Otp to emil id</summary> 
        public SendEmailOTPResponse sendEmailOtp(SendEmailOTPRequest sendOTPRequest)

        {
            SendEmailOTPResponse sendOTPResponse = new SendEmailOTPResponse();
            
            try
            {//CRF-Pop up message shall be displayed and  not allowed to register the new customer with e-mail address of another existing customer.
             //done by Sreerekha K 100006  on  17-Oct-2020
                DataSet dt = new DataSet();
                dt = helper.ExecuteDataSet("select count(*)  from CUSTOMER_DETAIL t where t.email_id ='"+ sendOTPRequest.emailID +"'");
                if (Convert.ToInt64(dt.Tables[0].Rows[0][0]) > 0)
                {
                    string alert = "This e-mail address is already existing, update the correct e-mail address";
                    sendOTPResponse.otp = null;
                    sendOTPResponse.status.code = APIStatus.alreadyExist; //12
                    sendOTPResponse.status.message = alert;
                    sendOTPResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    string otp = generateOTP();

                    var SendSmsmessage = SendmailNew(sendOTPRequest.emailID, otp);
                    sendOTPResponse.otp = otp;
                    sendOTPResponse.status.code = SendSmsmessage.status.code;
                    sendOTPResponse.status.message = SendSmsmessage.status.message;
                    sendOTPResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                sendOTPResponse.status.code = APIStatus.failed;
                sendOTPResponse.status.message =  ex.Message;

            }
            return sendOTPResponse;
        }
        
        public SaveEmailOTPResponse saveEmailOTP(SaveEmailOTPRequest saveEmailOTPRequest)

        {
            SaveEmailOTPResponse saveEmailOTPResponse = new SaveEmailOTPResponse();
            try
            {               
                    DataSet dt = new DataSet();
                    //  DBAccessHelper helper = new DBAccessHelper();
                    var otpQuery = string.Empty;
                    DateTime endTime = DateTime.Now.AddMinutes(Convert.ToDouble(Durations.LoginOTP));
                    dt = helper.ExecuteDataSet("select count(*) from TBL_CUST_MAIL_OTP_DTL where cust_id='" + saveEmailOTPRequest.custID + "'");
                    if (Convert.ToInt64(dt.Tables[0].Rows[0][0]) > 0)
                    {
                        otpQuery = "UPDATE TBL_CUST_MAIL_OTP_DTL SET OTP='" + saveEmailOTPRequest.otp + "', user_id =" + saveEmailOTPRequest.userID + ", tra_dt = sysdate , mail_id  = '" + saveEmailOTPRequest.emailID  + "' where cust_id ='" + saveEmailOTPRequest.custID  + "' ";
                        helper.ExecuteNonQuery(otpQuery);
                    }
                    else
                    {
                        otpQuery = "INSERT INTO TBL_CUST_MAIL_OTP_DTL (cust_id ,mail_id,tra_dt,otp,user_id) VALUES(" + saveEmailOTPRequest.custID + ",'" + saveEmailOTPRequest.emailID  + "',sysdate," + saveEmailOTPRequest.otp  + "," + saveEmailOTPRequest.userID  + ")";
                        helper.ExecuteNonQuery(otpQuery);
                    }
                }
                catch (Exception ex)
                {
                saveEmailOTPResponse.status.code = APIStatus.exception ;
                saveEmailOTPResponse.status.message = ex.Message ;
                saveEmailOTPResponse.status.flag = ProcessStatus.success;

            }


                saveEmailOTPResponse.status.code = APIStatus.success;
                saveEmailOTPResponse.status.message = "Success";
                saveEmailOTPResponse.status.flag = ProcessStatus.success;
                saveEmailOTPResponse.message = "Success";

            return saveEmailOTPResponse;

        }
           
           
        public EmailResponse Sendmail (string emailID, string Otp)
        {

            EmailResponse response = new EmailResponse();

            MailMessage msg = new MailMessage();
            msg.To.Add(new  MailAddress(emailID, "Tomail"));
            msg.From = new MailAddress("mailcommunication@manappuram.com", "FromMail");
            msg.Subject = "OTP for Email ID Verification";
            // without fb twitter String htmlString = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border - bottom: 1px solid #f2eeed;} .subhead {font - size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font - size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font - size: 16px; line-height: 22px;} .button {text - align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font - family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo].hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/ </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header'> <table width='70' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='h2'> Dear Customer, </td> </tr> <tr> <td class='bodycopy'> <p> Welcome aboard!! Your OTP for Email ID Verification is <b> " + Otp + " </b> Visit www.manappuram.com or download our mobile app for availing 24*7 anytime,anywhere Services.</p> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center;'> <b> Manappuram House </b> <br />Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567 , 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <img class='fix' src='E:/mail_images/fb.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <img class='fix' src='E:/mail_images/twitter.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <img class='fix' src='E:/mail_images/instagram.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <img class='fix' src='E:/mail_images/youtube.png' width='32' height='32' border='0' alt='' /> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";
            //String htmlString = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border-bottom: 1px solid #f2eeed;} .subhead {font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font-size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font-size: 13.5px; line-height: 24px;} .button {text-align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font-family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo] .hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/p {color: #153643; font-family: sans-serif; font-size: 15px;} a:link { background-color: yellow; color:red; } a:visited { background-color: yellow; color: red; } .myButton { background-color: #ffea00; border-radius: 28px; border: 1px solid #fabc00; display: inline-block; cursor: pointer; color: #ff0000; font-family: Arial; font-size: 17px; padding: 16px 31px; text-decoration: none; text-shadow: 0px 1px 0px #fff200; } .myButton:hover { background-color: #dbc115; } .myButton:active { position: relative; top: 1px; } </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header' colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> <td align='right'> Toll Free Number: 1800-420-22-33 (24x7) </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='h2'> Greeting from, <br /> <span style='background-color: #FFFF00'> 'Manappuram' </span>! </td> </tr> <tr> <td class='bodycopy'> <p> Dear Sir/Madam, <br /> We take this opportunity to first of all thank you for choosing Manappuram Finance for servicing gold loan and other products. Through this mail, we would like to extend our warm welcome to you as a new customer in our Manappuram family. You have chosen a India’s largest gold loan company also the largest wealth creator for the year of 2019 as listed in ET 500 companies with market capitalisation of over Rs 5000 Crores. </p> <h3> List of features: </h3> <ul> <li> You can avail our online gold loan 24x7 anywhere anytime. </li> <li> Day wise Interest calculation. </li> <li> Part payment and partially settlement facility </li> <li> Lowest interest rate and maximum loan disbursement </li> <li> Complete onetime registration by providing your bank details in our branch for getting online gold loan. </li> <li> Weekly twice renew your loan and you can enjoy Rs 1% interest rate slab. </li> <li> Door-step gold loan service available. </li> <li> Avail gold loan in Manappuram and get free gold locker facility. </li> <li> <a href='https://play.google.com/store/apps/details?id=com.manappuram.b2c' target='_blank'> Download </a> Manappuram OGL application in your android smartphone and start transaction. </li> <li> You can pay your loan payment through Net banking, Debit card, Paytm, Google Pay, PhonePe etc… <a href='https://online.manappuram.com/' target='_blank'> Click here </a> </li> <li> Also you can enjoy our other services like Money transfer, DMTS, Vehicle loan, Home loan, Personal loan, business loan etc… </li> <li> Join as Business Associates and earn monthly income. <a href='https://play.google.com/store/apps/details?id=com.manappuram.goldloan' target='_blank'> Click here </a> </li> </ul> <p> We would like to thank you once again for choosing Manappuram Finance and we assure you of excellent service. If you have any kind of queries please visit <a href='https://www.manappuram.com/' target='_blank'> www.manappuram.com. </a> Thanking you. </p> <p> Sincerely,<br /> Joshy V K <br /> GM & National Head Sales </p> </td> </tr> <tr> <td> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> &nbsp; </td> <td align='right'> &nbsp; </td> </tr> </table> </td> </tr> <tr> <td colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a href='https://www.manappuram.com/' target='_blank' class='myButton'> Call Back </a> </td> <td align='center'> <a href='https://play.google.com/store/apps/details?id=com.mgc.ogl' target='_blank' class='myButton'> Mob app </a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center; line-height:20px;'> <b> Manappuram House </b> <br /> Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567, 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'><img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /></a> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </a> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";

            String htmlString = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border - bottom: 1px solid #f2eeed;} .subhead {font - size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font - size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font - size: 16px; line-height: 22px;} .button {text - align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font - family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo].hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/ </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header'> <table width='70' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='h2'> Dear Customer, </td> </tr> <tr> <td class='bodycopy'> <p> Welcome aboard!! Your OTP for Email ID Verification is <b> " + Otp + " </b> Visit www.manappuram.com or download our mobile app for availing 24*7 anytime,anywhere Services.</p> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center;'> <b> Manappuram House </b> <br />Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567 , 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'><a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'><a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";

            msg.IsBodyHtml = true;
            msg.Body = htmlString;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("mailcommunication@manappuram.com", "SH@123ho");
            client.Port = 587; 
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl  = true;
            try
            {
                 
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.Send(msg);
                response.status.code = APIStatus.success;
                response.status.message = "Message Send Successfully";
            }
            catch (Exception ex)
            {
                response.status.code = APIStatus.exception;
                response.status.message =  ex.Message;
            }
            return response;

        }

        public EmailResponse SendmailNew(string emailID, string Otp)
        {
            EmailResponse response = new EmailResponse();
           
            try
            {
                EmailURL.ServiceSoapClient.EndpointConfiguration endpoint = new EmailURL.ServiceSoapClient.EndpointConfiguration();
                EmailURL.ServiceSoapClient myService = new EmailURL.ServiceSoapClient(endpoint, "https://online.manappuram.com/SendMail/Service.asmx");

                object messages = "";
                string fromMail = "mailcommunication@manappuram.biz";
                string Subject = "OTP for Email ID Verification";
                String mailbody = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border - bottom: 1px solid #f2eeed;} .subhead {font - size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font - size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font - size: 16px; line-height: 22px;} .button {text - align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font - family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo].hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/ </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header'> <table width='70' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='h2'> Dear Customer, </td> </tr> <tr> <td class='bodycopy'> <p> Welcome aboard!! Your OTP for Email ID Verification is <b> " + Otp + " </b> Visit www.manappuram.com or download our mobile app for availing 24*7 anytime,anywhere Services.</p> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center;'> <b> Manappuram House </b> <br />Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567 , 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'><a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </td> <td align='center'><a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";
                messages = myService.sendemailAsync(emailID, "MAFIL", fromMail, "MANAPPURAM", Subject, mailbody);

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
    }
}


     
 
