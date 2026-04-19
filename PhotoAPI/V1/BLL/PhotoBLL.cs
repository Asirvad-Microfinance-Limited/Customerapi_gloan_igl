using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using PhotoAPI.V1.BLLDependency;
using PhotoAPI.V1.Models.Request;
using PhotoAPI.V1.Models.Response;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using Utilities;
using static GlobalValues.GlobalVariables;

namespace PhotoAPI.V1.BLL
{
    public class PhotoBLL : APIBaseBLL , IPhotoBLL
    {

        public PhotoBLL() : base()
        {

        }
        public IDBAccessHelper helper;
        IConfiguration configuration;
        public PhotoBLL(IDBAccessHelper _helper, IConfiguration iConfig)
        {
            helper = _helper;
            configuration = iConfig;
        }
        #region CustomerPhotoGet

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Get Customer Photo</summary> 
        public CustomerPhotoResponse getCustomerPhoto(CustomerPhotoRequest customerPhotoRequest)
        {
            CustomerPhotoResponse customerPhotoResponse = new CustomerPhotoResponse();
            DataSet ods = new DataSet();
            string customerPhoto = "";
            DataTable dt = new DataTable();
            try
            {
                #region CUSTOMER PHOTO
                ods = helper.ExecuteDataSet("select count(*) from aml_gloan.customer_photo where cust_id='" + customerPhotoRequest.custId + "' and pledge_photo is not null");
                if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                {
                    dt = helper.ExecuteDataSet("select pledge_photo from aml_gloan.customer_photo where cust_id='" + customerPhotoRequest.custId + "' and pledge_photo is not null").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        byte[] imagebyte = (byte[])dt.Rows[0][0];
                        customerPhoto = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                    }
                }
                else
                {
                    ods = helper.ExecuteDataSet("select count(*) from aml_customer.TBL_CUSTOMER_IMAGES where cust_id='" + customerPhotoRequest.custId + "' and type_id=1");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        dt = helper.ExecuteDataSet("select t.file_type,t.recording_id,g.collection_name from aml_customer.TBL_CUSTOMER_IMAGES t inner join aml_customer.TBL_IMAGE_TYPE g on t.type_id = g.type_id where t.cust_id = '" + customerPhotoRequest.custId + "' and t.type_id = 1").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ImageResponse objResponse = new ImageResponse();
                            ImageRequest objRequest = new ImageRequest();
                            objRequest.collectionName = Convert.ToString(dt.Rows[0][2]);
                            objRequest.recordingId = Convert.ToString(dt.Rows[0][1]);
                            objRequest.fileType = Convert.ToString(dt.Rows[0][0]);
                            objRequest.editFlag = 0;
                            objResponse = CallAsirvadGetImages(objRequest);
                            if (objResponse.data.isDataAvilable == true)
                            {
                                AddPhotoRequest req = new AddPhotoRequest();
                                AddPhotoResponse res = new AddPhotoResponse();
                                req.custId = customerPhotoRequest.custId;
                                req.custPhoto = objResponse.data.imageString;
                                req.status = 1;
                                res = addPhoto(req);
                                if (res.status.code == APIStatus.success)
                                {
                                    customerPhoto = req.custPhoto;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region KYC PHOTO
                ods = helper.ExecuteDataSet("select count(*) from aml_gloan.customer_photo where cust_id='" + customerPhotoRequest.custId + "' and kyc_photo is not null");
                if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) == 0)
                {
                    ods = helper.ExecuteDataSet("select count(*) from aml_customer.TBL_CUSTOMER_IMAGES where cust_id='" + customerPhotoRequest.custId + "' and type_id in(2,3)");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        dt = helper.ExecuteDataSet("select t.file_type,t.recording_id,g.collection_name from aml_customer.TBL_CUSTOMER_IMAGES t inner join aml_customer.TBL_IMAGE_TYPE g on t.type_id = g.type_id where t.cust_id = '" + customerPhotoRequest.custId + "' and t.type_id in(2,3)").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ImageResponse objResponse = new ImageResponse();
                            ImageRequest objRequest = new ImageRequest();
                            objRequest.collectionName = Convert.ToString(dt.Rows[0][2]);
                            objRequest.recordingId = Convert.ToString(dt.Rows[0][1]);
                            objRequest.fileType = Convert.ToString(dt.Rows[0][0]);
                            objRequest.editFlag = 0;
                            objResponse = CallAsirvadGetImages(objRequest);
                            if (objResponse.data.isDataAvilable == true)
                            {
                                AddPhotoRequest req = new AddPhotoRequest();
                                AddPhotoResponse res = new AddPhotoResponse();
                                req.custId = customerPhotoRequest.custId;
                                req.custPhoto = objResponse.data.imageString;
                                req.status = 2;
                                res = addPhoto(req);
                            }
                        }
                    }
                }
                #endregion

                #region BANK PHOTO
                ods = helper.ExecuteDataSet("select count(*) from aml_gloan.CUSTOMER_bank_passbook where cust_id='" + customerPhotoRequest.custId + "'");
                if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) == 0)
                {
                    ods = helper.ExecuteDataSet("select count(*) from aml_customer.TBL_CUSTOMER_IMAGES where cust_id='" + customerPhotoRequest.custId + "' and type_id=4");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        dt = helper.ExecuteDataSet("select t.file_type,t.recording_id,g.collection_name from aml_customer.TBL_CUSTOMER_IMAGES t inner join aml_customer.TBL_IMAGE_TYPE g on t.type_id = g.type_id where t.cust_id = '" + customerPhotoRequest.custId + "' and t.type_id = 4").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ImageResponse objResponse = new ImageResponse();
                            ImageRequest objRequest = new ImageRequest();
                            objRequest.collectionName = Convert.ToString(dt.Rows[0][2]);
                            objRequest.recordingId = Convert.ToString(dt.Rows[0][1]);
                            objRequest.fileType = Convert.ToString(dt.Rows[0][0]);
                            objRequest.editFlag = 0;
                            objResponse = CallAsirvadGetImages(objRequest);
                            if (objResponse.data.isDataAvilable == true)
                            {
                                AddPhotoRequest req = new AddPhotoRequest();
                                AddPhotoResponse res = new AddPhotoResponse();
                                req.custId = customerPhotoRequest.custId;
                                req.custPhoto = objResponse.data.imageString;
                                req.status = 3;
                                res = addPhoto(req);
                            }
                        }
                    }
                }

                //--ckyc data insertion-----------added wef 22-sep-2022-- sreerekha
                ods = helper.ExecuteDataSet("select count(*)  from  aml_gloan.TBL_CUST_CKYC_DTL where cust_id='" + customerPhotoRequest.custId + "'");
                if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) == 0)
                {
                    dt = helper.ExecuteDataSet("select a.fat_hus  from customer a where cust_id='" + customerPhotoRequest.custId + "'").Tables[0]; ;
                     helper.ExecuteNonQuery("insert into  TBL_CUST_CKYC_DTL (CUST_ID, FAT_PFX, FAT_F_NAME, FAT_SPOUSE_FLG, ADDR_TYPE, USER_ID, TRA_DT, STATUS_ID) values ('" + customerPhotoRequest.custId + "', 1,'" + Convert.ToString(dt.Rows[0][0]) + "' , 1, 3, 1111, sysdate, 1)");
                }
                //---ckyc
                #endregion

                if (customerPhoto != null && customerPhoto != "")
                {
                    customerPhotoResponse.custPhoto = customerPhoto;
                    customerPhotoResponse.status.code = APIStatus.success;
                    customerPhotoResponse.status.message = "Success";
                    customerPhotoResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    customerPhotoResponse.status.code = APIStatus.no_Data_Found;
                    customerPhotoResponse.status.message = "No data found for your search";
                    customerPhotoResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                customerPhotoResponse.status.flag = ProcessStatus.failed;
                customerPhotoResponse.status.code = APIStatus.exception;
                customerPhotoResponse.status.message = ex.Message;
            }

            return customerPhotoResponse;
        }


        public ImageResponse CallAsirvadGetImages(ImageRequest request)
        {
            PublicConfigManager appConfigManager = new PublicConfigManager();
            ImageResponse imageResponse = new ImageResponse();
            GlobalValues.GlobalMethods obj = new GlobalValues.GlobalMethods();
            try
            {
                //string baseUrl = configuration.GetSection("ServiceUrlSettings").GetSection("baseUrl").Value;
                string baseUrl = "https://appsbackend.asirvad.com/AsirvadGoldloan/customergeneralapi";
                // imageResponse = new ApiManager().InvokePostHttpClient<ImageResponse, ImageRequest>(request, appConfigManager.getgeneralAPIUrl + "/api/general/images").Item1;
                imageResponse = new ApiManager().InvokePostHttpClient<ImageResponse, ImageRequest>(request, baseUrl + "/api/general/images").Item1;
            }
            catch (Exception ex)
            {

            }
            return imageResponse;
        }

        public AddPhotoResponse addPhoto(AddPhotoRequest addPhotoRequest)
        {
            AddPhotoResponse addPhotoResponse = new AddPhotoResponse();
            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
            string msg = string.Empty;
            if (addPhotoRequest.status == 1)  //Customer Photo
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(addPhotoRequest.custPhoto);
                    ods = helper.ExecuteDataSet("select count(*) from aml_gloan.customer_photo where cust_id='" + addPhotoRequest.custId + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        helper.ExecuteNonQuery("insert into aml_gloan.customer_photo_his select t.cust_id,t.pledge_photo,t.kyc_photo,sysdate,t.kyc_adhar_status from aml_gloan.customer_photo t where cust_id='" + addPhotoRequest.custId + "'");
                        string sql1 = "update aml_gloan.customer_photo set pledge_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "ph";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql1, parm1);
                        msg = "Complete2";
                    }
                    else
                    {
                        helper.ExecuteNonQuery("insert into aml_gloan.customer_photo (cust_id) values ('" + addPhotoRequest.custId + "')");
                        string sql = "update aml_gloan.customer_photo set pledge_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql, parm);
                        msg = "Complete";
                    }
                    if (msg == "Complete2" || msg == "Complete")
                    {
                        OracleParameter[] parm_coll = new OracleParameter[1];
                        parm_coll[0] = new OracleParameter("custid", OracleDbType.Varchar2, 16);
                        parm_coll[0].Value = addPhotoRequest.custId;
                        parm_coll[0].Direction = ParameterDirection.Input;
                        helper.ExecuteNonQuery("Proc_Kyc_WorkAlert_Rmv", parm_coll);
                        addPhotoResponse.status.code = APIStatus.success;
                        addPhotoResponse.status.message = "Photo successfully added";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        addPhotoResponse.status.code = APIStatus.failed;
                        addPhotoResponse.status.message = "photo failed to add";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                }
                catch (Exception ex)
                {

                    addPhotoResponse.status.code = APIStatus.failed;
                    addPhotoResponse.status.message = ex.Message;
                    addPhotoResponse.status.flag = ProcessStatus.success;
                }
            }
            else if (addPhotoRequest.status == 2)  //KYC Photo
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(addPhotoRequest.custPhoto);
                    ods = helper.ExecuteDataSet("select count(*) from aml_gloan.customer_photo where cust_id='" + addPhotoRequest.custId + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        helper.ExecuteNonQuery("insert into aml_gloan.customer_photo_his select t.cust_id,t.pledge_photo,t.kyc_photo,sysdate,t.kyc_adhar_status from aml_gloan.customer_photo t where cust_id='" + addPhotoRequest.custId + "'");
                        string sql1 = "update aml_gloan.customer_photo set kyc_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "ph";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql1, parm1);
                        msg = "Complete2";
                    }
                    else
                    {
                        helper.ExecuteNonQuery("insert into aml_gloan.customer_photo (cust_id) values ('" + addPhotoRequest.custId + "')");
                        string sql = "update aml_gloan.customer_photo set kyc_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql, parm);
                        msg = "Complete";
                    }
                    if (msg == "Complete2" || msg == "Complete")
                    {
                        OracleParameter[] parm_coll = new OracleParameter[1];
                        parm_coll[0] = new OracleParameter("custid", OracleDbType.Varchar2, 16);
                        parm_coll[0].Value = addPhotoRequest.custId;
                        parm_coll[0].Direction = ParameterDirection.Input;
                        helper.ExecuteNonQuery("Proc_Kyc_WorkAlert_Rmv", parm_coll);
                        addPhotoResponse.status.code = APIStatus.success;
                        addPhotoResponse.status.message = "Photo successfully added";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        addPhotoResponse.status.code = APIStatus.failed;
                        addPhotoResponse.status.message = "photo failed to add";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }

                }
                catch (Exception ex)
                {
                    addPhotoResponse.status.code = APIStatus.failed;
                    addPhotoResponse.status.message = ex.Message;
                    addPhotoResponse.status.flag = ProcessStatus.success;
                }

            }
            else if (addPhotoRequest.status == 3)   //Bank Photo
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(addPhotoRequest.custPhoto);
                    ods = helper.ExecuteDataSet("select count(*) from customer_bank_passbook where cust_id='" + addPhotoRequest.custId + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        helper.ExecuteNonQuery("insert into customer_bank_passbook_his select t.cust_id,t.PASSBOOK_PHOTO,t.TRA_DT from customer_bank_passbook t where cust_id='" + addPhotoRequest.custId + "'");
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter("image", OracleDbType.Blob);
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        var query = "update customer_bank_passbook set PASSBOOK_PHOTO=:image where cust_id='" + addPhotoRequest.custId + "'";
                        helper.ExecuteNonQuery(query, parm);

                        OracleParameter[] parmAdd = new OracleParameter[1];
                        parmAdd[0] = new OracleParameter("imageAdd", OracleDbType.Blob);
                        parmAdd[0].Direction = ParameterDirection.Input;
                        parmAdd[0].Value = imageBytes;
                        var queryAdd = "update TBL_ADD_NEFT_CUSTOMER set id_proof=:imageAdd where cust_id='" + addPhotoRequest.custId + "' and STATUS=0 ";
                        helper.ExecuteNonQuery(queryAdd, parmAdd);
                        msg = "Complete2";
                    }
                    else
                    {
                        helper.ExecuteNonQuery("insert into customer_bank_passbook (cust_id,TRA_DT) values ('" + addPhotoRequest.custId + "',sysdate)");
                        helper.ExecuteNonQuery("insert into TBL_ADD_NEFT_CUSTOMER (cust_id) values ('" + addPhotoRequest.custId + "')");
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter("image", OracleDbType.Blob);
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        var query = "update customer_bank_passbook set PASSBOOK_PHOTO=:image where cust_id='" + addPhotoRequest.custId + "'";
                        helper.ExecuteNonQuery(query, parm);

                        OracleParameter[] parmAdd = new OracleParameter[1];
                        parmAdd[0] = new OracleParameter("imageAdd", OracleDbType.Blob);
                        parmAdd[0].Direction = ParameterDirection.Input;
                        parmAdd[0].Value = imageBytes;
                        var queryAdd = "update TBL_ADD_NEFT_CUSTOMER set id_proof=:imageAdd where cust_id='" + addPhotoRequest.custId + "' and STATUS=0 ";
                        helper.ExecuteNonQuery(queryAdd, parmAdd);
                        msg = "Complete";
                    }
                    if (msg == "Complete2" || msg == "Complete")
                    {
                        addPhotoResponse.status.code = APIStatus.success;
                        addPhotoResponse.status.message = "Photo successfully added";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        addPhotoResponse.status.code = APIStatus.failed;
                        addPhotoResponse.status.message = "photo failed to add";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }

                }
                catch (Exception ex)
                {
                    addPhotoResponse.status.code = APIStatus.failed;
                    addPhotoResponse.status.message = ex.Message;
                    addPhotoResponse.status.flag = ProcessStatus.success;
                }

            }
            return addPhotoResponse;
        }

        #endregion CustomerPhotoGet

        #region CustomerPhotoPost
        /// <Created>Aravind  - 100231</Created>
        /// <summary>Add Customer Photo</summary> 
        public AddPhotoResponse addCustomerPhoto(AddPhotoRequest addPhotoRequest)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            AddPhotoResponse addPhotoResponse = new AddPhotoResponse();
            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
            string msg = string.Empty;
            if (addPhotoRequest.status == 1)
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(addPhotoRequest.custPhoto);
                    ods = helper.ExecuteDataSet("select count(*) from aml_gloan.customer_photo where cust_id='" + addPhotoRequest.custId + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        helper.ExecuteNonQuery("insert into  aml_gloan.customer_photo_his select t.cust_id,t.pledge_photo,t.kyc_photo,sysdate,t.kyc_adhar_status from aml_gloan.customer_photo t where cust_id='" + addPhotoRequest.custId + "'");
                        string sql1 = "update aml_gloan.customer_photo set pledge_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "ph";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql1, parm1);
                        msg = "Complete2";
                    }
                    else
                    {
                        helper.ExecuteNonQuery("insert into aml_gloan.customer_photo (cust_id) values ('" + addPhotoRequest.custId + "')");
                        string sql = "update aml_gloan.customer_photo set pledge_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql, parm);
                        msg = "Complete";
                    }
                    if (msg == "Complete2" || msg == "Complete")
                    {
                        OracleParameter[] parm_coll = new OracleParameter[1];
                        parm_coll[0] = new OracleParameter("custid", OracleDbType.Varchar2, 16);
                        parm_coll[0].Value = addPhotoRequest.custId;
                        parm_coll[0].Direction = ParameterDirection.Input;
                        helper.ExecuteNonQuery("Proc_Kyc_WorkAlert_Rmv", parm_coll);
                        addPhotoResponse.status.code = APIStatus.success;
                        addPhotoResponse.status.message = "Photo successfully added";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        addPhotoResponse.status.code = APIStatus.failed;
                        addPhotoResponse.status.message = "photo failed to add";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                }
                catch (Exception ex)
                {
                    
                    addPhotoResponse.status.flag = ProcessStatus.failed;
                    addPhotoResponse.status.code = APIStatus.exception;
                    addPhotoResponse.status.message = ex.Message;
                }
            }
            else
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(addPhotoRequest.custPhoto);
                   // Image img = Base64ToImage(addPhotoRequest.custPhoto);
                    //byte[] imageBytes = ImageToByteArray(img);
                    ods = helper.ExecuteDataSet("select count(*) from aml_gloan.customer_photo where cust_id='" + addPhotoRequest.custId + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        helper.ExecuteNonQuery("insert into  aml_gloan.customer_photo_his select t.cust_id,t.pledge_photo,t.kyc_photo,sysdate,t.kyc_adhar_status from aml_gloan.customer_photo t where cust_id='" + addPhotoRequest.custId + "'");
                        string sql1 = "update aml_gloan.customer_photo set kyc_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "ph";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql1, parm1);
                        msg = "Complete2";
                    }
                    else
                    {
                        helper.ExecuteNonQuery("insert into aml_gloan.customer_photo (cust_id) values ('" + addPhotoRequest.custId + "')");
                        string sql = "update aml_gloan.customer_photo set kyc_photo=:ph where cust_id='" + addPhotoRequest.custId + "'";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql, parm);
                        msg = "Complete";
                    }
                    if (msg == "Complete2" || msg == "Complete")
                    {
                        OracleParameter[] parm_coll = new OracleParameter[1];
                        parm_coll[0] = new OracleParameter("custid", OracleDbType.Varchar2, 16);
                        parm_coll[0].Value = addPhotoRequest.custId;
                        parm_coll[0].Direction = ParameterDirection.Input;
                        helper.ExecuteNonQuery("Proc_Kyc_WorkAlert_Rmv", parm_coll);
                        addPhotoResponse.status.code = APIStatus.success;
                        addPhotoResponse.status.message = "Photo successfully added";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        addPhotoResponse.status.code = APIStatus.failed;
                        addPhotoResponse.status.message = "Photo failed to add";
                        addPhotoResponse.status.flag = ProcessStatus.success;
                    }
                   
                }
                catch (Exception ex)
                {
                    addPhotoResponse.status.flag = ProcessStatus.failed;
                    addPhotoResponse.status.code = APIStatus.exception;
                    addPhotoResponse.status.message = ex.Message;
                }

            }
            return addPhotoResponse;
        }

        #endregion CustomerPhotoPost


        #region NewCustomerPhotoPost

        /// <Created>Aravind  - 100231</Created>
        /// <summary>Add new Customer Photo</summary> 
        public AddNewCustomerPhotoResponse newcustomeraddPhoto(AddNewCustomerPhotoRequest addNewCustomerPhotoRequest)
        {
            string sql;
            AddNewCustomerPhotoResponse addPhotoResponse = new AddNewCustomerPhotoResponse();
            try
            {
              //  DBAccessHelper helper = new DBAccessHelper();
                if (!string.IsNullOrEmpty(addNewCustomerPhotoRequest.custPhoto))
                {
                    sql = "update aml_gloan.customer_photo set pledge_photo=:ph where cust_id='" + addNewCustomerPhotoRequest.custId + "'";
                    OracleParameter[] parm = new OracleParameter[1];
                    parm[0] = new OracleParameter();
                    parm[0].ParameterName = "ph";
                    parm[0].OracleDbType = OracleDbType.Blob;
                    parm[0].Direction = ParameterDirection.Input;
                    byte[] cust_photo = Convert.FromBase64String(addNewCustomerPhotoRequest.custPhoto);
                    parm[0].Value = cust_photo;
                    helper.ExecuteNonQuery(sql, parm);

                    addPhotoResponse.status.code = APIStatus.success;
                    addPhotoResponse.status.message = "Photo successfully added";
                    addPhotoResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    addPhotoResponse.status.code = APIStatus.failed;
                    addPhotoResponse.status.message = "Photo failed to add";
                    addPhotoResponse.status.flag = ProcessStatus.success;

                }
            }
            catch (Exception ex)
            {
                addPhotoResponse.status.flag = ProcessStatus.failed;
                addPhotoResponse.status.code = APIStatus.exception;
                addPhotoResponse.status.message = ex.Message;
            }
            return addPhotoResponse;
        }

        #endregion NewCustomerPhotoPost

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


        #region CustomerPhotoPut
        /// <Created>Aravind  - 100231</Created>
        /// <summary>Update new Customer Photo</summary> 
        public UpdatePhotoResponse updateCustomerPhoto(UpdatePhotoRequest updatePhotoRequest)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            UpdatePhotoResponse updatePhotoResponse = new UpdatePhotoResponse();
            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
            string msg = string.Empty;
           
                try
                {
                       byte[] imageBytes = Convert.FromBase64String(updatePhotoRequest.custPhoto);
                      //Image img = Base64ToImage(updatePhotoRequest.custPhoto);
                      //byte[] imageBytes = ImageToByteArray(img);

                      ods = helper.ExecuteDataSet("select count(*) from  kyc_pre_auth where cust_id='" + updatePhotoRequest.custId + "' and status_id=1 and change_type = 5");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        
                        string sql1 = "update  kyc_pre_auth set CUST_PHOTO=:ph, TRA_DT = sysdate, UPLOADED_BRANCH = " + updatePhotoRequest .branchId + ",UPLOADED_USER = " + updatePhotoRequest.userId + " where cust_id='" + updatePhotoRequest .custId + "' and status_id=1 and change_type = 5";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "ph";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql1, parm1);
                        msg = "Complete2";
                    }
                    else
                    {
                        helper.ExecuteNonQuery("insert into  kyc_pre_auth (cust_id,requested_by,req_dt,STATUS_ID, TRA_DT, change_type,UPLOADED_BRANCH, UPLOADED_USER) values ('" + updatePhotoRequest .custId + "','" + updatePhotoRequest .userId + "',sysdate,1,sysdate, 5," + updatePhotoRequest.branchId + "," + updatePhotoRequest.userId + " )");
                        string sql = "update  kyc_pre_auth set CUST_PHOTO=:ph where cust_id='" + updatePhotoRequest.custId + "' and status_id = 1 and change_type = 5";
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter();
                        parm[0].ParameterName = "ph";
                        parm[0].OracleDbType = OracleDbType.Blob;
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql, parm);
                        msg = "Complete";
                    }
                    if (msg == "Complete2" || msg == "Complete")
                    {

                        updatePhotoResponse.status.code = APIStatus.success;
                        updatePhotoResponse.status.message = "Photo successfully updated";
                        updatePhotoResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        updatePhotoResponse.status.code = APIStatus.failed;
                        updatePhotoResponse.status.message = "failed";
                        updatePhotoResponse.status.flag = ProcessStatus.success;
                    }
                }
                catch (Exception ex)
                {
                    updatePhotoResponse.status.flag = ProcessStatus.failed;
                    updatePhotoResponse.status.code = APIStatus.exception;
                    updatePhotoResponse.status.message = ex.Message;
                }
            
           
            return updatePhotoResponse;
        }

        #endregion CustomerPhotoPut


    }
}
