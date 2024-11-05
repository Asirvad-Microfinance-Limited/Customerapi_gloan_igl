using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GlobalValues;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Appsettings;
using KYCAPI.V1.Models.Properties;
using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.BLL
{
    public class EKYCBLL : APIBaseBLL , IEKYCBLL
    {
  
        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        IConfiguration configuration;
        IGlobalMethods globalMethods;
        public EKYCBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper, IConfiguration iConfig, IGlobalMethods _globalMethods)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
            configuration = iConfig;
            globalMethods = _globalMethods;
        }
        #region EKYCGet
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get  EKYC</summary>
        public EKYCResponse EKYCGet(EKYCGetRequest eKYCGetRequest)
        {

            EKYCResponse eKYCResponse = new EKYCResponse();
            try
            {

                if (eKYCGetRequest.custId != null)
                {
                    var ekycCustVerifyList = getVerifyCustId(eKYCGetRequest.custId).ekycCustVerifyList;
                    if (ekycCustVerifyList !=null)
                    {
                        eKYCResponse.ekycCustVerifyList = ekycCustVerifyList;
                        eKYCResponse.status.code = APIStatus.success;
                        eKYCResponse.status.message = "Success";
                        eKYCResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        eKYCResponse.status.code = APIStatus.no_Data_Found;
                        eKYCResponse.status.message = "No data found for your search";
                        eKYCResponse.status.flag = ProcessStatus.success;
                    }
                    eKYCResponse.ekycAdarList = getAdarcard(eKYCGetRequest.custId).ekycAdarList;
                    
                }
                if (eKYCGetRequest.uuId != null)
                {
                    eKYCResponse.ekycExistsList = getEKYCExists(eKYCGetRequest.uuId).ekycExistsList;                 
                    eKYCResponse.ekycAdarVerifyList = getVerifyUUID(eKYCGetRequest.uuId).ekycAdarVerifyList;                   
                    eKYCResponse.uuIdExistsList = getUUIDExists(eKYCGetRequest.uuId).uuIdExistsList;
                   

                }
                eKYCResponse.status.code = APIStatus.success;
                eKYCResponse.status.message = "Success";
                eKYCResponse.status.flag = ProcessStatus.success;
            }
            catch (Exception ex)
            {
                eKYCResponse.status.flag = ProcessStatus.failed;
                eKYCResponse.status.code = APIStatus.exception;
                eKYCResponse.status.message = ex.Message;

            }

            return eKYCResponse;
        }
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Check  EKYC Exists</summary> 
        public EKYCExistsResponse getEKYCExists(string uuId)
        {
            EKYCExistsResponse response = new EKYCExistsResponse();
           
            try
            {
              //  DBAccessHelper helper = new DBAccessHelper();
                string query = "select t.cust_id from TBL_EKYC_LOG t where t.cust_id is not null and t.uuid = '"+ uuId + "'";
                var ekycExistsList = DapperHelper.GetRecords<EKYCExistsProperties>(query,SQLMode.Query,null);
                if (ekycExistsList.Count > 0)
                {                    
                    response.ekycExistsList = ekycExistsList;
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
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Check  UUID Exists</summary> 
        public getUUIDExistsResponse getUUIDExists(string uuId)
        {
            getUUIDExistsResponse response = new getUUIDExistsResponse();

            try
            {
             //   DBAccessHelper helper = new DBAccessHelper();
                string query = "SELECT  LISTAGG(i.cust_id, ', ') WITHIN GROUP (ORDER BY i.cust_id) AS cust_id FROM   (select t.cust_id  from identity_dtl t  where t.id_number = '" + uuId  + "' union select u.cust_id   from tbl_cust_uuid u  where u.uuid = '" + uuId  + "') i";
                var uuIdExistsList = DapperHelper.GetRecords<UUIDExistsProperties>(query,SQLMode.Query,null);
                if (uuIdExistsList.Count > 0)
                {
                    response.uuIdExistsList = uuIdExistsList;
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
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Adarcard</summary> 
        public EKYCAdarResponse getAdarcard(string custId)
        {
            EKYCAdarResponse response = new EKYCAdarResponse();

            try
            {
              //  DBAccessHelper helper = new DBAccessHelper();
                string query = "select Idn.Id_Number from identity_dtl Idn where Idn.Identity_Id in (16,505,555)and Idn.Cust_Id = '" + custId + "'";
                var ekycAdarList = DapperHelper.GetRecords<EKYCCheckAdarProperties>(query,SQLMode.Query,null);
                if (ekycAdarList.Count > 0)
                {
                    response.ekycAdarList = ekycAdarList;
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
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Verify UUID</summary> 
        public EKYCAdarverifyUUIDResponse getVerifyUUID(string uuId)
        {
            EKYCAdarverifyUUIDResponse response = new EKYCAdarverifyUUIDResponse();

            try
            {
               // DBAccessHelper helper = new DBAccessHelper();
                string query = "SELECT E.UUID, to_char( MAX(E.verified_dt), 'dd-Mon-yyyy HH12:MI:SS') VERIFIED_DT,e.cust_id FROM TBL_EKYC_LOG E WHERE E.STATUS = 'Y' AND E.EKYC_MODE = 'KUA' AND E.UUID = '" + uuId + "' GROUP BY E.UUID, e.cust_id";
                var ekycAdarVerifyList = DapperHelper.GetRecords<EKYCVerifyUUIDProperties>(query,SQLMode.Query,null);
                if (ekycAdarVerifyList.Count > 0)
                {
                    response.ekycAdarVerifyList = ekycAdarVerifyList;
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
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Verify Customer Id</summary> 
        public EKYCVerifyCustIdResponse getVerifyCustId(string custId)
        {
            EKYCVerifyCustIdResponse response = new EKYCVerifyCustIdResponse();

            try
            {
               // DBAccessHelper helper = new DBAccessHelper();
                string query = "SELECT E.UUID, to_char( MAX(E.verified_dt), 'dd-Mon-yyyy HH12:MI:SS') VERIFIED_DT FROM TBL_EKYC_LOG E WHERE E.STATUS = 'Y' AND E.EKYC_MODE = 'KUA' AND E.Cust_Id = '" + custId + "' GROUP BY E.UUID";
                var ekycCustVerifyList = DapperHelper.GetRecords<EKYCVerifyCustProperties>(query,SQLMode.Query,null);
                if (ekycCustVerifyList.Count>0)
                {
                    response.ekycCustVerifyList = ekycCustVerifyList;
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
            catch (Exception e)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = e.Message;
            }

            return response;
        }
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Ekyc Count</summary>         
        public EKYCCountResponse getEkycCount(string uuId, string rrn)
        {
           // DBAccessHelper helper = new DBAccessHelper();
            EKYCCountResponse eKYCCountResponse = new EKYCCountResponse();
            try
            {
                var query = "select count(*) from tbl_ekyc_log TEL where TEL.uuid ='" + uuId + "' and TEL.rrn_no = '" + rrn + "'  and TEL.status ='Y'";

                decimal obj = helper.ExecuteScalar<decimal>(query);

                if (obj != -1)
                {
                    eKYCCountResponse.ekycCount = Convert.ToInt64(obj);
                    eKYCCountResponse.status.code = APIStatus.success;
                    eKYCCountResponse.status.message = "Success";
                    eKYCCountResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    eKYCCountResponse.status.code = APIStatus.no_Data_Found;
                    eKYCCountResponse.status.message = "No data found for your search";
                    eKYCCountResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                eKYCCountResponse.status.code = APIStatus.exception;
                eKYCCountResponse.status.message = ex.Message;
                eKYCCountResponse.status.flag = ProcessStatus.failed;
            }

            return eKYCCountResponse;
        }

        #endregion EKYCGet

        #region KYCIdTypesGet       
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get KYC Id Types</summary>          
        public KycIdTypesResponse KYCIdTypesGet(string authToken)
        {
            
            KycIdTypesResponse kycIdTypesResponse = new KycIdTypesResponse();
            KycIdTypesRequest kycIdTypesRequest = new KycIdTypesRequest();
            kycIdTypesRequest.optionId = 36;
            kycIdTypesRequest.moduleId = 1;
            try
            {
                
                string baseUrl = configuration.GetSection("ServiceUrlSettings").GetSection("baseUrl").Value;
                //string generalUrl =$"/statusmaster?optionId={kycIdTypesRequest.optionId}&moduleId={kycIdTypesRequest.moduleId}";
                string generalUrl = $"/generalAPI/api/v1/statusmaster?optionId={kycIdTypesRequest.optionId}&moduleId={kycIdTypesRequest.moduleId}";
                string Url =baseUrl + generalUrl;              
                var kycIdTypesUrl = Url.ToString();               
                //var kycIdTypesUrl = $"http://mafildev.mactech.net.in/generalAPI/api/v1/statusmaster?optionId={kycIdTypesRequest.optionId}&moduleId={kycIdTypesRequest.moduleId}";
                var generalList = globalMethods.InvokeGetHttpClientWithoutRequest<MastersApiResponse<KycIdTypesProperties>>(kycIdTypesUrl,authToken).statusList;
                if (generalList != null)
                {
                    kycIdTypesResponse.generalList = generalList;
                    kycIdTypesResponse.status.code = APIStatus.success;
                    kycIdTypesResponse.status.message = "Success";
                    kycIdTypesResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    kycIdTypesResponse.status.code = APIStatus.no_Data_Found;
                    kycIdTypesResponse.status.message = "No data found for your search";
                    kycIdTypesResponse.status.flag = ProcessStatus.success;
                }
               
            }
            catch (Exception ex)
            {
                kycIdTypesResponse.status.flag = ProcessStatus.failed;
                kycIdTypesResponse.status.code = APIStatus.exception;
                kycIdTypesResponse.status.message = ex.Message;

            }

            return kycIdTypesResponse;
        }


        #endregion KYCIdTypesGet

        #region EKYCPost
        /// <Created>Vidhya - 100101</Created>
        /// <summary>Update EKYC</summary>          
        public EkycPostResponse updateEKYC(EKYCRequest eKYCRequest)
        {
         //   DBAccessHelper helper = new DBAccessHelper();
            EkycPostResponse ekycPostResponse = new EkycPostResponse();
            EKYCExistsResponse eKYCExistsResponse = new EKYCExistsResponse();
            try
            {
                try
                {
                    long ekycCount = getEkycCount(eKYCRequest.uuId, eKYCRequest.rrn).ekycCount;

                    if (ekycCount == 0)
                    {
                        ekycPostResponse.message = "E-kyc is not correctly verified";
                        ekycPostResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        var ekycExistsList = getEKYCExists(eKYCRequest.uuId).ekycExistsList;
                        if (ekycExistsList.Count == 0)
                        {
                            string queryEkyc = "update tbl_ekyc_log t set t.cust_id ='" + eKYCRequest.custId + "', t.custid_status = 4 where t.uuid = '" + eKYCRequest.uuId + "' and t.rrn_no = '" + eKYCRequest.rrn + "' and t.status ='Y'";
                            helper.ExecuteNonQuery(queryEkyc);
                            string queryCust = "update tbl_customer_master t set t.uuid = '" + eKYCRequest.uuId + "', t.rrn_no = '" + eKYCRequest.rrn + "', t.cust_source = 8 where t.cust_id = '" + eKYCRequest.custId + "'";
                            helper.ExecuteNonQuery(queryCust);


                            ekycPostResponse.message = "E-kyc successfully verified";
                            ekycPostResponse.status.code = APIStatus.success;
                            ekycPostResponse.status.message = "Success";
                            ekycPostResponse.status.flag = ProcessStatus.success;

                        }
                        else
                        {
                            ekycPostResponse.message = "This EKYC already mapped with another customer ID";
                            ekycPostResponse.status.code = APIStatus.success;
                            ekycPostResponse.status.message = "Success";
                            ekycPostResponse.status.flag = ProcessStatus.success;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ekycPostResponse.status.code = APIStatus.exception;
                    ekycPostResponse.status.message = ex.Message;
                    ekycPostResponse.status.flag = ProcessStatus.failed;

                }

            }
            catch (Exception ex)
            {
                ekycPostResponse.status.code = APIStatus.exception;
                ekycPostResponse.status.message = ex.Message;
                ekycPostResponse.status.flag = ProcessStatus.failed;

            }

            return ekycPostResponse;
        }

        #endregion EKYCPost

        #region PawnBrokerGetImage

        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Pawn Broker Image</summary>    
        public PawnBrokerResponse PawnBrokerGetImage(PawnBrokerRequest pawnBrokerRequest)
        {

            PawnBrokerResponse pawnBrokerResponse = new PawnBrokerResponse();
            try
            {

                if (pawnBrokerRequest.identityId==1) // 1 --> Get license_image
                {
                    var licenseImageList = PawnBrokerGetLicenseImage(pawnBrokerRequest.custId).licenseImageList;
                    if (licenseImageList != null)
                    {
                        pawnBrokerResponse.licenseImageList = licenseImageList;
                        pawnBrokerResponse.status.code = APIStatus.success;
                        pawnBrokerResponse.status.message = "Success";
                        pawnBrokerResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        pawnBrokerResponse.status.code = APIStatus.no_Data_Found;
                        pawnBrokerResponse.status.message = "No data found for your search";
                        pawnBrokerResponse.status.flag = ProcessStatus.success;
                    }
                }
                else   // 2 --> Get form_image
                {
                    var formImageList = PawnBrokerGetFormImage(pawnBrokerRequest.custId).formImageList;
                    if (formImageList != null)
                    {
                        pawnBrokerResponse.formImageList = formImageList;
                        pawnBrokerResponse.status.code = APIStatus.success;
                        pawnBrokerResponse.status.message = "Success";
                        pawnBrokerResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        pawnBrokerResponse.status.code = APIStatus.no_Data_Found;
                        pawnBrokerResponse.status.message = "No data found for your search";
                        pawnBrokerResponse.status.flag = ProcessStatus.success;
                    }
                }

            }
            catch (Exception ex)
            {
                pawnBrokerResponse.status.flag = ProcessStatus.failed;
                pawnBrokerResponse.status.code = APIStatus.exception;
                pawnBrokerResponse.status.message = ex.Message;

            }

            return pawnBrokerResponse;
        }

        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Pawn Broker from  Image</summary>    
        public PawnBrokerResponse PawnBrokerGetFormImage(string custId)
        {

            PawnBrokerResponse pawnBrokerResponse = new PawnBrokerResponse();
         //   DBAccessHelper helper = new DBAccessHelper();
            try
            {
                var query = "select form_image from  pawncustomer_documents where  customer_id= '" + custId + "'";
                DataSet ds = helper.ExecuteDataSet(query);                
                List<PawnBrokerFormProperties> imageList = new List<PawnBrokerFormProperties>();               
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string base64String = null;
                        DataRow dRow = dt.Rows[i];
                        if (dRow["form_image"] != null)
                        {
                            byte[] imagebyte = (byte[])(dRow["form_image"] == DBNull.Value ? ImageToByte() : (dRow["form_image"]));
                            base64String = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                        }
                        imageList.Add(new PawnBrokerFormProperties
                        {

                            FORM_IMAGE = base64String,

                        });
                    }
                }             

                if (imageList.Count > 0)
                {
                    pawnBrokerResponse.formImageList = imageList;
                    pawnBrokerResponse.status.code = APIStatus.success;
                    pawnBrokerResponse.status.message = "Success";
                    pawnBrokerResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    pawnBrokerResponse.status.code = APIStatus.no_Data_Found;
                    pawnBrokerResponse.status.message = "No data found for your search";
                    pawnBrokerResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                pawnBrokerResponse.status.code = APIStatus.exception;
                pawnBrokerResponse.status.message = ex.Message;
                pawnBrokerResponse.status.flag = ProcessStatus.failed;
            }

            return pawnBrokerResponse;
        }

        /// <Created>Vidhya - 100101</Created>
        /// <summary>Get Pawn Broker License  Image</summary>    
        public PawnBrokerResponse PawnBrokerGetLicenseImage(string custId)
        {
            PawnBrokerResponse pawnBrokerResponse = new PawnBrokerResponse();
         //   DBAccessHelper helper = new DBAccessHelper();
            try
            {

                var query = "select license_image from  pawncustomer_documents where  customer_id= '" + custId + "'";
                DataSet ds = helper.ExecuteDataSet(query);               
                List<PawnBrokerLicenseProperties> imageList = new List<PawnBrokerLicenseProperties>();             
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string base64String = null;
                        DataRow dRow = dt.Rows[i];
                        if (dRow["license_image"] != null)
                        {
                            byte[] imagebyte = (byte[])(dRow["license_image"] == DBNull.Value ? ImageToByte() : (dRow["license_image"]));
                            base64String = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                        }
                        imageList.Add(new PawnBrokerLicenseProperties
                        {

                            LICENSE_IMAGE = base64String,

                        });
                    }
                }                
                if (imageList.Count > 0)
                {
                    pawnBrokerResponse.licenseImageList = imageList;
                    pawnBrokerResponse.status.code = APIStatus.success;
                    pawnBrokerResponse.status.message = "Success";
                    pawnBrokerResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    pawnBrokerResponse.status.code = APIStatus.no_Data_Found;
                    pawnBrokerResponse.status.message = "No data found for your search";
                    pawnBrokerResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                pawnBrokerResponse.status.code = APIStatus.exception;
                pawnBrokerResponse.status.message = ex.Message;
                pawnBrokerResponse.status.flag = ProcessStatus.failed;
            }

            return pawnBrokerResponse;
        }

        public static byte[] ImageToByte()
        {
            MemoryStream imgStream = new MemoryStream();
            byte[] byteArray = imgStream.ToArray();
            return byteArray;
        }

        #endregion PawnBrokerGetImage

        #region addAadhaar

        /// <Created>Aravind - 100231</Created>
        /// <summary>Add Aadhaar</summary>    
        public AddAadhaarResponse addAadhaar(AddAadhaarRequest addAadhaarRequest)
        {
            AddAadhaarResponse addAadhaarResponse = new AddAadhaarResponse();

            try
            {
               // DBAccessHelper helper = new DBAccessHelper();
                string SQL1 =string.Empty, sql2 = string.Empty, Auid = string.Empty;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                OracleParameter[] parm_coll = new OracleParameter[13];
                parm_coll[0] = new OracleParameter("branch", OracleDbType.Long, 5);
                parm_coll[0].Value = addAadhaarRequest.branchId;
                parm_coll[0].Direction = ParameterDirection.Input;
                parm_coll[1] = new OracleParameter("aadhar", OracleDbType.Varchar2, 20);
                parm_coll[1].Value = addAadhaarRequest.aadhaarId;
                parm_coll[1].Direction = ParameterDirection.Input;
                parm_coll[2] = new OracleParameter("V_Mode", OracleDbType.Varchar2, 50);
                parm_coll[2].Value = addAadhaarRequest.vMode;
                parm_coll[2].Direction = ParameterDirection.Input;
                parm_coll[3] = new OracleParameter("e_Mode", OracleDbType.Varchar2, 50);
                parm_coll[3].Value = addAadhaarRequest.eMode;
                parm_coll[3].Direction = ParameterDirection.Input;
                parm_coll[4] = new OracleParameter("e_Code", OracleDbType.Varchar2, 100);
                parm_coll[4].Value = addAadhaarRequest.eCode;
                parm_coll[4].Direction = ParameterDirection.Input;
                parm_coll[5] = new OracleParameter("e_Txn", OracleDbType.Varchar2, 50);
                parm_coll[5].Value = addAadhaarRequest.eTxn;
                parm_coll[5].Direction = ParameterDirection.Input;
                parm_coll[6] = new OracleParameter("e_Ts", OracleDbType.Varchar2, 50);
                parm_coll[6].Value = addAadhaarRequest.eTs;
                parm_coll[6].Direction = ParameterDirection.Input;
                parm_coll[7] = new OracleParameter("e_Status", OracleDbType.Varchar2, 50);
                parm_coll[7].Value = addAadhaarRequest.eStatus;
                parm_coll[7].Direction = ParameterDirection.Input;
                parm_coll[8] = new OracleParameter("User_id", OracleDbType.Varchar2, 50);
                parm_coll[8].Value = addAadhaarRequest.userId;
                parm_coll[8].Direction = ParameterDirection.Input;
                parm_coll[9] = new OracleParameter("Auid", OracleDbType.Varchar2, 100);
                parm_coll[9].Direction = ParameterDirection.Output;
                parm_coll[10] = new OracleParameter("custDtl", OracleDbType.Varchar2, 4000);
                parm_coll[10].Value = addAadhaarRequest.custDtl;
                parm_coll[10].Direction = ParameterDirection.Input;
                parm_coll[11] = new OracleParameter("ErrMessage", OracleDbType.Varchar2, 1000);
                parm_coll[11].Direction = ParameterDirection.Output;
                parm_coll[12] = new OracleParameter("rrn_n", OracleDbType.Varchar2, 500);
                parm_coll[12].Value = addAadhaarRequest.rrnN;
                helper.ExecuteNonQuery("proc_ekyc_customer", parm_coll);

                if (parm_coll[9].Value != null)
                {
                    Auid = Convert.ToString(parm_coll[9].Value);
                    string[] separators = new[] { ":" };
                    string[] custId = Convert.ToString(parm_coll[11].Value).Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    SQL1 = "UPDATE  TBL_EKYC_DTL SET tra_dt=sysdate, RESPONSE=:SBP WHERE ekyc_id='" + Auid + "'";
                    OracleParameter[] parm = new OracleParameter[1];
                    parm[0] = new OracleParameter();
                    parm[0].ParameterName = "SBP";
                    parm[0].OracleDbType = OracleDbType.Clob;
                    parm[0].Direction = ParameterDirection.Input;
                    parm[0].Value = addAadhaarRequest.retval;
                    helper.ExecuteNonQuery(SQL1, parm);


                    if (addAadhaarRequest.eMode == "KUA" && addAadhaarRequest.eStatus == "Y")
                    {
                        sql2 = "UPDATE  TBL_EKYC_DTL SET PHOTO=:SBP WHERE ekyc_id='" + Auid + "'";
                        OracleParameter[] parm1 = new OracleParameter[1];
                        parm1[0] = new OracleParameter();
                        parm1[0].ParameterName = "SBP";
                        parm1[0].OracleDbType = OracleDbType.Blob;
                        parm1[0].Direction = ParameterDirection.Input;
                        byte[] imageBytes = Convert.FromBase64String(addAadhaarRequest.photo);
                        parm1[0].Value = imageBytes;
                        helper.ExecuteNonQuery(sql2, parm1);

                    }
                    if (custId[0] == "Customer ID Recommended for SRM/RM Approval")
                    {
                        Srm_Customer_ApprovalMail(custId[1].Trim(), addAadhaarRequest.branchId);
                    }


                    addAadhaarResponse.serRet = Convert.ToString(parm_coll[11].Value);
                    addAadhaarResponse.status.code = APIStatus.success;
                    addAadhaarResponse.status.message = "Success";
                    addAadhaarResponse.status.flag = ProcessStatus.success;
                }
                else
                {

                    addAadhaarResponse.serRet = Convert.ToString(parm_coll[11].Value);
                    addAadhaarResponse.status.code = APIStatus.failed;
                    addAadhaarResponse.status.message = "failed";
                    addAadhaarResponse.status.flag = ProcessStatus.success;

                }
               
            }
            catch (Exception ex)
            {
                addAadhaarResponse.status.flag = ProcessStatus.failed;
                addAadhaarResponse.status.code = APIStatus.exception;
                addAadhaarResponse.status.message = ex.Message;
            }

            return addAadhaarResponse;
        }

        #endregion addAadhaar

        #region MailApproval

        /// <Created>Aravind - 100231</Created>
        /// <summary>Srm Customer Approval Mail</summary>   
        public void Srm_Customer_ApprovalMail(string cust, string br)
        {
            try
            {
             //   DBAccessHelper helper = new DBAccessHelper();
                DataTable dt = new DataTable();
                string srm_email;
                string mbody;
                string subj;
                string cc, tow;
                string str = "select rm_mailid,b.BRANCH_NAME,e.email_address from region_master m,branch_detail b,branch_email_address e where m.reg_id=b.reg_id and b.BRANCH_ID = e.branch_id and  b.BRANCH_ID =" + br;
                dt = helper.ExecuteDataSet(str).Tables[0];
                srm_email = dt.Rows[0][0].ToString();
                cc = dt.Rows[0][2].ToString();
                //MailHelper.MailHelper mh = new MailHelper.MailHelper("formkrisk", "outlookexpress");
                //subj = "Customer ID " + cust + " pending for SRM Approval Request";
                //mbody = " Hi Sir,<br/><br/> Customer ID " + cust + " from  " + dt.Rows[0][1].ToString() + " branch is pending for SRM Approval. <br/><br/> This is a System Generated Mail Please do Not Reply.";
                //mh.SendMail("Formkrisk@in.manappuram.com", srm_email, "", cc, subj, mbody, "");
                //mh.SendMail("Formkrisk@in.manappuram.com", "srmrisk@in.manappuram.com", "", "", subj, mbody, "");
            }
            catch (Exception ex)
            {
            }
        }

        #endregion MailApproval

        #region KYCDisplaydata

        /// <Created>Aravind - 100231</Created>
        /// <summary>Customer KYC Display data</summary>   
        public AadharDataResponse customerKYCDisplaydata(AadharDataRequest aadharDataRequest)
        {
            AadharDataResponse aadharDataResponse = new AadharDataResponse();
          //  DBAccessHelper helper = new DBAccessHelper();
            try
            {
              
                string sql = "SELECT ELEMENT(T.CUST_DTL, 1, '©') CUS_NAME,  to_char(to_date(ELEMENT(T.CUST_DTL, 2, '©'), 'dd-mm-yyyy'),  'dd-MON-yyyy') CUS_DOB,  DECODE(ELEMENT(T.CUST_DTL, 3, '©'), 'M', 'MALE', 'F', 'FEMALE') CUS_GENDER, DECODE(ELEMENT(T.CUST_DTL, 8, '©'),  '--',  '',  ELEMENT(T.CUST_DTL, 8, '©')) CUS_HOUSE,  DECODE(ELEMENT(T.CUST_DTL, 10, '©'),  '--',  '',  ELEMENT(T.CUST_DTL, 10, '©')) CUS_STREET,  DECODE(ELEMENT(T.CUST_DTL, 12, '©'),  '--',  '',  ELEMENT(T.CUST_DTL, 12, '©')) CUS_LOCAL,  ELEMENT(T.CUST_DTL, 13, '©') CUST_DIST,  ELEMENT(T.CUST_DTL, 14, '©') CUST_STATE,  ELEMENT(T.CUST_DTL, 4, '©') CUST_MOBILE,  ELEMENT(T.CUST_DTL, 5, '©') CUST_MAIL,  ELEMENT(T.CUST_DTL, 6, '©') CUST_FAT,  ELEMENT(T.CUST_DTL, 15, '©') CUST_PIN  FROM TBL_EKYC_LOG T  WHERE T.VERIFY_ID = '" + aadharDataRequest.rrN + "'";

                EKYCAadharCustomerProperties eKYCAadharCustomerProperties = DapperHelper.GetRecord<EKYCAadharCustomerProperties>(sql,SQLMode.Query,null);

                if (eKYCAadharCustomerProperties.CUST_PIN != null)
                {
                    string PINCODE = eKYCAadharCustomerProperties.CUST_PIN;
                    string SqlCountry = "select distinct s.country_id,s.state_id, s.state_name,d.district_id,d.district_name from state_master s, district_master d, POST_MASTER t where s.state_id = d.state_id and t.district_id = d.district_id and t.pin_code = " + PINCODE + "";
                    EKYCCustomerProperties eKYCCustomerProperties = DapperHelper.GetRecord< EKYCCustomerProperties>(SqlCountry,SQLMode.Query,null);
                    string QueryPostOffice = "select PM.pin_code , PM.sr_number ,PM.Post_Office from POST_MASTER PM where PM.PIN_CODE = " + PINCODE + "";
                    EKYCPostProperties eKYCPostProperties = DapperHelper.GetRecord<EKYCPostProperties>(QueryPostOffice,SQLMode.Query,null);
                    aadharDataResponse.eKYCAadharCustomer = eKYCAadharCustomerProperties;
                    aadharDataResponse.eKYCCustomer = eKYCCustomerProperties;
                    aadharDataResponse.eKYCPostOffice = eKYCPostProperties;
                    aadharDataResponse.status.code = APIStatus.success;
                    aadharDataResponse.status.message = "Success";
                    aadharDataResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    aadharDataResponse.status.code = APIStatus.no_Data_Found;
                    aadharDataResponse.status.message = "No Data Found";
                    aadharDataResponse.status.flag = ProcessStatus.success;

                }
            }
            catch (Exception ex)
            {
                aadharDataResponse.status.flag = ProcessStatus.failed;
                aadharDataResponse.status.code = APIStatus.exception;
                aadharDataResponse.status.message = ex.Message;
            }
            return aadharDataResponse;
        }

        #endregion KYCDisplaydata
        

    }
}
  