using System;
using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GlobalValues;
using KYCAPI.V1.Models.Response;
using KYCAPI.V1.Models.Request;
using static GlobalValues.GlobalVariables;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using KYCAPI.V1.BLLDependency;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KYCAPI.V1.BLL
{
    public class KYCBLL : APIBaseBLL, IKYCBLL
    {

        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public KYCBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Add Kyc</summary>    
        public KYCResponse addKyc(KYCRequest request)
        {
            KYCResponse response = new KYCResponse();
           
                //DBAccessHelper helper = new DBAccessHelper();

            int vals;
            DataSet ods = new DataSet();
                DataTable dt = new DataTable();
                string message = string.Empty;
            try
            {
                //System.Drawing.Image img = Base64ToImage(request.kycPhoto);
                //byte[] imageBytes = ImageToByteArray(img);
                byte[] imageBytes = Convert.FromBase64String(request.kycPhoto);
                ods = helper.ExecuteDataSet("select count(*) from customer_photo where cust_id='" + request.custID + "' and kyc_photo is null ");
                if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                {
                    // helper.ExecuteNonQuery("insert into  customer_photo_his select t.*,sysdate from customer_photo t where cust_id='" + addPhotoRequest.custId + "'");
                    string sql1 = "update customer_photo set kyc_photo=:ph where cust_id='" + request.custID + "'";
                    OracleParameter[] parm1 = new OracleParameter[1];
                    parm1[0] = new OracleParameter();
                    parm1[0].ParameterName = "ph";
                    parm1[0].OracleDbType = OracleDbType.Blob;
                    parm1[0].Direction = ParameterDirection.Input;
                    parm1[0].Value = imageBytes;
                    helper.ExecuteNonQuery(sql1, parm1);

                    response.status.code = APIStatus.success;
                    response.status.message = "Photo successfully added";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
                else
                {

                    // ods = oh.ExecuteDataSet("select count(*) from  kyc_pre_auth where cust_id='" & custid & "' and status_id=1")
                    // Modified by Tijo For Customer name and other related approval change
                    ods = helper.ExecuteDataSet("select count(*) from  kyc_pre_auth where cust_id='" + request.custID + "' and status_id=1 and change_type = 5");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        if (request.kycPhoto != null)
                        {
                            string sql = "update  kyc_pre_auth set kyc_photo=:ph, uploaded_branch=" + request.branchID + ",uploaded_user=" + request.userID + ", TRA_DT = sysdate where cust_id='" + request.custID + "' and status_id = 1 and change_type = 5";
                            OracleParameter[] parm = new OracleParameter[1];
                            parm[0] = new OracleParameter();
                            parm[0].ParameterName = "ph";
                            parm[0].OracleDbType = OracleDbType.Blob;
                            parm[0].Direction = ParameterDirection.Input;
                            byte[] kyc_photo = imageBytes;
                            parm[0].Value = kyc_photo;

                            helper.ExecuteNonQuery(sql, parm);
                        }
                        message = "Your request to modify the kyc is still pending for verification.";
                        response.status.code = APIStatus.success;
                        response.status.message = message;
                        response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        // oh.ExecuteNonQuery("insert into  kyc_pre_auth (cust_id,tra_dt,uploaded_branch,uploaded_user,status_id,kyc_type,kyc_id) values ('" & custid & "',sysdate," & branch_id & "," & userID & ",1," & IIf(String.IsNullOrEmpty(kyc_type), "null", kyc_type) & ",'" & kyc_id & "')")
                        // Modified by Tijo For Customer name and other related approval change
                        string sqlins = "insert into  kyc_pre_auth (cust_id,tra_dt,uploaded_branch,uploaded_user,kyc_type,kyc_id,requested_by,req_dt,STATUS_ID, change_type) values ('" + request.custID + "',sysdate," + request.branchID + "," + request.userID + "," + (request.kycType == null ? "null" : request.kycType.ToString()) + ",'" + request.kycID + "','" + request.userID + "',sysdate" + ",1, 5)";
                        helper.ExecuteNonQuery(sqlins);
                        if (request.kycPhoto != null)
                        {
                            string sql = "update  kyc_pre_auth set kyc_photo=:ph where cust_id='" + request.custID + "' and STATUS_ID=1 and change_type = 5";
                            OracleParameter[] parm = new OracleParameter[1];
                            parm[0] = new OracleParameter();
                            parm[0].ParameterName = "ph";
                            parm[0].OracleDbType = OracleDbType.Blob;
                            parm[0].Direction = ParameterDirection.Input;
                            byte[] kyc_photo = imageBytes;
                            parm[0].Value = kyc_photo;
                            helper.ExecuteNonQuery(sql, parm);
                        }

                    }

                    if (request.chkAddress == CheckStatus.Checked && request.Addressboxview == CheckStatus.Checked)
                    {
                        updateKycDocuments("UPDADDPROOFID", request.custID, Convert.ToString(request.AddressProf), "", Convert.ToString(request.userID), out vals, out message);
                    }
                    else if (request.chkAddress == CheckStatus.Checked && request.chkSameAsId == CheckStatus.Checked)
                    {
                        updateKycDocuments("UPDADDPROOFID", request.custID, "0", "", Convert.ToString(request.userID), out vals, out message);
                    }
                    else
                    {
                        updateKycDocuments("UPDADDPROOFID", request.custID, "", "", Convert.ToString(request.userID), out vals, out message);
                    }

                    if (vals == 1)
                    {
                        response.status.code = APIStatus.success;
                        response.status.message = message;
                        response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        response.status.code = APIStatus.failed;
                        response.status.message = "Failed";
                        response.status.flag = ProcessStatus.failed;
                    }

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
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Kyc Doc</summary>    
        public KYCDocResponse getKYCDoc(KYCDocRequest request)
        {
            KYCDocResponse response = new KYCDocResponse();
            try
            {

            //DBAccessHelper helper = new DBAccessHelper();
                DataTable dt = new DataTable();
                dt = helper.ExecuteDataSet("select kyc_photo from customer_photo where cust_id='" + request.custId + "'").Tables[0];
                if ((dt != null) &&(dt.Rows.Count > 0))
                {
                    byte[] imagebyte = (byte[])dt.Rows[0][0];

                    try
                    {
                        response.tiffDoc = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                        var type = GeMimeTypeFromImageByteArray(imagebyte);
                        if (type == "image/tiff")
                        {
                            using (MemoryStream inStream = new MemoryStream(imagebyte))
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imagebyte = outStream.ToArray();
                            }

                        }
                    }catch(Exception ex)
                    {
                         imagebyte = (byte[])dt.Rows[0][0];
                    }
                    
                    response.kycDoc = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;

                }
                else
                {
                    response.status.code = APIStatus.no_Data_Found;
                    response.status.message = "No data found for your search";
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

        public static string GeMimeTypeFromImageByteArray(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            using (Image image = Image.FromStream(stream))
            {
                return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == image.RawFormat.Guid).MimeType;
            }
        }

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Update Kyc Documents</summary>    
        private void  updateKycDocuments(string as_flag, string custId,string addproofID, string param, string UserID,out int val, out string strResult)
        {
           
            try
            {

                //DBAccessHelper helper = new DBAccessHelper();
                OracleParameter[] Params = new OracleParameter[6];
                string DataStr = string.Empty;
                Params[0] = new OracleParameter("as_flag", OracleDbType.Varchar2, 100);
                Params[0].Value = as_flag;
                Params[1] = new OracleParameter("custid", OracleDbType.Varchar2, 200);
                Params[1].Value = custId;
                Params[2] = new OracleParameter("addproofid", OracleDbType.Varchar2, 100);
                Params[2].Value = addproofID;
                Params[3] = new OracleParameter("param", OracleDbType.Varchar2);
                Params[3].Value = param;
                Params[4] = new OracleParameter("userid", OracleDbType.Varchar2);
                Params[4].Value = UserID;
                Params[5] = new OracleParameter("out_msg", OracleDbType.Varchar2, 100);
                Params[5].Direction = ParameterDirection.Output;
                helper.ExecuteNonQuery("UpdateKYCInfo", Params);

               // DataStr = Convert.ToString(Params[4].Value);


                if (Params[5].Value != DBNull.Value)
                {
                    strResult = Convert.ToString(Params[5].Value);
                    val = 1;

                }
                else
                {
                    strResult = "Failed";
                    val = 0;
                }
            }
            catch (Exception e)
            {
                strResult = e.Message;
                val = 0;
            }

            
        }
        //Adding visa details of customers wef 10-dec-2019 done by Sreerekha K 100006
    public VisaResponse addVisaDetails(VisaRequest request)
    {
       VisaResponse response = new VisaResponse();

        DataSet ods = new DataSet();
        DataTable dt = new DataTable();
        string message = string.Empty;

            OracleParameter[] Params = new OracleParameter[9];
            string DataStr = string.Empty;
            Params[0] = new OracleParameter("p_Cust_ID", OracleDbType.Varchar2, 14);
            Params[0].Value = request.custID;
            Params[1] = new OracleParameter("p_Visa_No", OracleDbType.Varchar2, 200);
            Params[1].Value = request.visaNumber;
            Params[2] = new OracleParameter("p_Visa_Issue_Dt", OracleDbType.Date, 100);
            Params[2].Value = request.visaIssueDate;
            Params[3] = new OracleParameter("p_Visa_Exp_dt", OracleDbType.Date, 100);
            Params[3].Value = request.visaExpiryDate;
            Params[4] = new OracleParameter("p_Cntry1", OracleDbType.Varchar2,100);
            Params[4].Value = request.Country1;
            Params[5] = new OracleParameter("p_Cntry2", OracleDbType.Varchar2, 100);
            Params[5].Value = request.Country2;
            Params[6] = new OracleParameter("p_Cntry3", OracleDbType.Varchar2, 100);
            Params[6].Value = request.Country3;
            Params[7] = new OracleParameter("p_ERR_MSG", OracleDbType.Varchar2, 100);
            Params[7].Direction = ParameterDirection.Output;
            Params[8] = new OracleParameter("p_ERR_FLG", OracleDbType.Varchar2, 100);
            Params[8].Direction = ParameterDirection.Output;

            helper.ExecuteNonQuery("proc_cust_visa", Params);
            

            try
            {
            byte[] imageBytes = Convert.FromBase64String(request.visaDocument);
            ods = helper.ExecuteDataSet("select count(*) from tbl_cust_visa_dtl where cust_id='" + request.custID + "' and visa_attach is null ");
            if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
            {
                string sql1 = "update tbl_cust_visa_dtl set visa_attach=:ph where cust_id='" + request.custID + "'";
                OracleParameter[] parm1 = new OracleParameter[1];
                parm1[0] = new OracleParameter();
                parm1[0].ParameterName = "ph";
                parm1[0].OracleDbType = OracleDbType.Blob;
                parm1[0].Direction = ParameterDirection.Input;
                parm1[0].Value = imageBytes;
                helper.ExecuteNonQuery(sql1, parm1);

                response.status.code = APIStatus.success;
                response.status.message = "Visa documents successfully added";
                response.status.flag = ProcessStatus.success;
                return response;
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

        public visadetailsGetResponse getVisaDetails(visadetailsGetRequest request)
        {
            visadetailsGetResponse response = new visadetailsGetResponse();
            try
            {
 
                DataTable dt = new DataTable();
                dt = helper.ExecuteDataSet("select t.visa_no,to_char(t.visa_issue_dt),to_char(t.visa_exp_dt),t.visit_cntry1,t.visit_cntry2,t.visit_cntry3,t.visa_attach from tbl_cust_visa_dtl t   where t.cust_id='" + request.custID + "'").Tables[0];
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    byte[] imagebyte = (byte[])dt.Rows[0][6];

                    try
                    {
                        response.visaNumber = Convert.ToString(dt.Rows[0][0]);
                        response.visaIssueDate= Convert.ToString(dt.Rows[0][1]);
                        response.visaExpiryDate = Convert.ToString(dt.Rows[0][2]);
                        response.Country1 = Convert.ToString(dt.Rows[0][3]);
                        response.Country2 = Convert.ToString(dt.Rows[0][4]);
                        response.Country3 = Convert.ToString(dt.Rows[0][5]);

                        response.visaDocument  = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                        var type = GeMimeTypeFromImageByteArray(imagebyte);
                        if (type == "image/tiff")
                        {
                            using (MemoryStream inStream = new MemoryStream(imagebyte))
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imagebyte = outStream.ToArray();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        imagebyte = (byte[])dt.Rows[0][6];
                        response.status.flag = ProcessStatus.failed;
                        response.status.code = APIStatus.exception;
                        response.status.message = ex.Message;
                    }

                    
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;

                }
                else
                {
                    response.status.code = APIStatus.no_Data_Found;
                    response.status.message = "No data found for your search";
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

        public AadharConsentResponse uploadAadharConsent(AadharConsentRequest request)
        {
            AadharConsentResponse response = new AadharConsentResponse();

            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
            string message = string.Empty;

           

            try
            {
                byte[] imageBytes = Convert.FromBase64String(request.aadharConsent);
                ods = helper.ExecuteDataSet("select count(*) from  tbl_cust_aadhaar_consent where cust_id='" + request.custID + "'");
                if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                {
                    string sqlins = "update  tbl_cust_aadhaar_consent set tra_dt=sysdate,user_id=" + request.userID + "";
                    helper.ExecuteNonQuery(sqlins);

                    string sql1 = "update  tbl_cust_aadhaar_consent set consent_doc=:ph where cust_id='" + request.custID + "'";
                    OracleParameter[] parm1 = new OracleParameter[1];
                    parm1[0] = new OracleParameter();
                    parm1[0].ParameterName = "ph";
                    parm1[0].OracleDbType = OracleDbType.Blob;
                    parm1[0].Direction = ParameterDirection.Input;
                    parm1[0].Value = imageBytes;
                    helper.ExecuteNonQuery(sql1, parm1);

                    response.status.code = APIStatus.success;
                    response.status.message = "Aadhar Consent successfully uploaded";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
                else
                {
                    string sqlins = "insert into  tbl_cust_aadhaar_consent (cust_id, consent_doc, tra_dt, user_id) values ('" + request.custID + "','',sysdate," + request.userID +")";
                    helper.ExecuteNonQuery(sqlins);
                    if (request.aadharConsent != null)
                    {
                        string sql = "update  tbl_cust_aadhaar_consent set consent_doc=:ph where cust_id='" + request.custID + "'";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        byte[] kyc_photo = imageBytes;
                        parm[0].Value = kyc_photo;
                        helper.ExecuteNonQuery(sql, parm);

                        response.status.code = APIStatus.success;
                        response.status.message = "Aadhar Consent successfully uploaded";
                        response.status.flag = ProcessStatus.success;
                        return response;
                    }

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


        public AdharDocResponse getAadharDoc(AdharDocRequest request)
        {
            AdharDocResponse response = new AdharDocResponse();
            try
            {

                //DBAccessHelper helper = new DBAccessHelper();
                DataTable dt = new DataTable();
                dt = helper.ExecuteDataSet("select consent_doc from  tbl_cust_aadhaar_consent where cust_id='" + request.custId + "'").Tables[0];
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    byte[] imagebyte = (byte[])dt.Rows[0][0];
                    
                    response.aadharConsent = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;

                }
                else
                {
                    response.status.code = APIStatus.no_Data_Found;
                    response.status.message = "No data found for your search";
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


        //---- Address Proof Upload --- KYC master directions implementation----

        public UploadAddressproofResponse uploadAddressProof(UploadAddressproofRequest request)
        {
            UploadAddressproofResponse response = new UploadAddressproofResponse();

            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
            string message = string.Empty;

            try
            {
                if (request.addressProofDocument != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(request.addressProofDocument);
                    ods = helper.ExecuteDataSet("select count(*) from  tbl_cust_addr_verify where cust_id='" + request.custID + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        string sql1 = "update  tbl_cust_addr_verify set addr_attach=:ph where cust_id='" + request.custID + "'";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "ph";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql1, parm1);

                        response.status.code = APIStatus.success;
                        response.status.message = "Address Proof successfully uploaded";
                        response.status.flag = ProcessStatus.success;
                        return response;
                    }
                    else
                    {
                        string sqlins = "insert into  TBL_CUST_ADDR_VERIFY (cust_id, ADDR_TYP, BILL_DATE,TRA_DT, user_id) values ('" + request.custID + "','" + request.addressProof_Type + "',to_date('" +  Convert.ToDateTime(request.BillDate).ToShortDateString() + "','MM-dd-yyyy'),sysdate," + request.userID + ")";
                        helper.ExecuteNonQuery(sqlins);
                        string sql = "update  TBL_CUST_ADDR_VERIFY set ADDR_ATTACH=:ph where cust_id='" + request.custID + "'";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        byte[] kyc_photo = imageBytes;
                        parm[0].Value = kyc_photo;
                        helper.ExecuteNonQuery(sql, parm);

                        response.status.code = APIStatus.success;
                        response.status.message = "Address Proof successfully uploaded";
                        response.status.flag = ProcessStatus.success;
                        return response;


                    }
                }
                else
                {
                    response.status.flag = ProcessStatus.failed;
                    response.status.code = APIStatus.failed ;
                    response.status.message = "Pls attach Image";
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


        public AddressProofGetResponse getAddressProofDoc(AdressProofGetRequest request)
        {
            AddressProofGetResponse response = new AddressProofGetResponse();
            try
            {

                //DBAccessHelper helper = new DBAccessHelper();
                DataTable dt = new DataTable();
                dt = helper.ExecuteDataSet("select ADDR_ATTACH from  TBL_CUST_ADDR_VERIFY where cust_id='" + request.custId + "'").Tables[0];
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    byte[] imagebyte = (byte[])dt.Rows[0][0];

                    response.aadharConsent = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;

                }
                else
                {
                    response.status.code = APIStatus.no_Data_Found;
                    response.status.message = "No data found for your search";
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
        //De - dupe based on KYC ID - 17-oct-2020
        public KycDeDupeResponse getKYCdeDupe(KycDeDupeRequest request)
        {
            KycDeDupeResponse response = new KycDeDupeResponse();
            try
            { 
                DataTable dt = new DataTable();//new table "TBL_CUSTOMER_VALIDATION" created for KYC number validation--21/Dec/2020
                string KycIdnumber = Regex.Replace(request.kycnumber, @"(\s+|@|&|'|\(|\)|<|>|#|/|-)", "");
                dt = helper.ExecuteDataSet("select t.cust_id from TBL_CUSTOMER_VALIDATION t where   t.identity_id not in (505,555,16) and t.identity_id= " + request.kyctype + " and t.regex_id_num ='" + KycIdnumber + "'").Tables[0];
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    string CustIDs = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == (dt.Rows.Count - 1))
                        { CustIDs += dt.Rows[i][0]; }
                        else
                        { CustIDs += dt.Rows[i][0] + ","; }

                    }
                    response.status.code = APIStatus.failed;
                    response.status.message = "KYC ID exists in another customer ID/IDs. Customer IDs matched are as follows : " + CustIDs  + "";
                    response.status.flag = ProcessStatus.success;

                }
                else
                {
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
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
        //De - dupe based on KYC ID - 17-oct-2020
        //KYC control implementation 2nd phase 12-Jan-2021
        public KycSelfCertifyResponse KycSelfCertify(KycSelfCertifyRequest request)
        {
            KycSelfCertifyResponse response = new KycSelfCertifyResponse();
            try
            {
                string query;
                query = "insert into TBL_KYC_SELF_CERTIFY (cust_id,TRA_DT,USER_ID) values('" + request.custID + "',sysdate, " + request.empCode + " )";
                helper.ExecuteNonQuery(query);

                byte[] imageByte = Convert.FromBase64String(request.SelfCertifyImage);
                OracleParameter[] parm = new OracleParameter[1];
                parm[0] = new OracleParameter("image", OracleDbType.Blob);
                parm[0].Direction = ParameterDirection.Input;
                parm[0].Value = imageByte;
                var query1 = "update TBL_KYC_SELF_CERTIFY set ATTACH =:image where cust_id ='" + request.custID + "'";
                helper.ExecuteNonQuery(query1, parm);

                response.status.code = APIStatus.success;
                response.status.message = "Success";
                response.status.flag = ProcessStatus.success;
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }
            return response;
        }
        //KYC control implementation 2nd phase 12-Jan-2021
    }

}