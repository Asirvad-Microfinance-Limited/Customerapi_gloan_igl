using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GlobalValues;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.BLL
{
    public class ConsentBLL : APIBaseBLL , IConsentBLL
    {
        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public ConsentBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Consent</summary>          
        public ConsentResponse getConsent(ConsentRequestlang request)
        {
            ConsentResponse response = new ConsentResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                DataTable dt = new DataTable();
                dt = helper.ExecuteDataSet("select t.consent from  TBL_EKYCONSENT t where t.SERIAL_NUMBER = " + request.language + "" ).Tables[0];
               // ConsentResponse consent = GlobalMethods.GetClassNew<ConsentResponse>(query);
                if (dt.Rows.Count > 0)
                {
                    byte[] imagebyte = (byte[])dt.Rows[0][0];
                    response.consentData = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
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

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Add Consent</summary>           
        public addConsentResponse addConsent(ConsentRequest Request)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            addConsentResponse Response = new addConsentResponse();


            string sql;
            DataTable dt = new DataTable(), dt1 = new DataTable();
            dt = helper.ExecuteDataSet("select count(*) from  TBL_EKYC_consent where CUSTOMER_ID = '" + Request.custId + "'").Tables[0];
            dt1 = helper.ExecuteDataSet("select count(*) from tbl_ekyc_log where cust_id = '" + Request.custId + "' and cust_id is not null").Tables[0];
            if (Convert.ToInt16(dt.Rows[0][0]) == 0 & Convert.ToInt16(dt1.Rows[0][0]) > 0)
            {
                sql = "insert into  TBL_EKYC_consent (CUSTOMER_ID,CONSENT) values (:customerid1,:ph)";
                OracleParameter[] conf_par = new OracleParameter[2];
                conf_par[0] = new OracleParameter("customerid1", OracleDbType.Varchar2, 50);
                conf_par[0].Direction = ParameterDirection.Input;
                conf_par[0].Value = Request.custId;

                conf_par[1] = new OracleParameter();
                conf_par[1].ParameterName = "ph";
                conf_par[1].OracleDbType = OracleDbType.Blob;
                conf_par[1].Direction = ParameterDirection.Input;
                byte[] kyc_photo = Convert.FromBase64String(Request.kycphoto);
                conf_par[1].Value = kyc_photo;
                //conf_par[1].Value = Request.kycphoto;
                helper.ExecuteNonQuery(sql, conf_par);
                // oh.ExecuteNonQuery(sql, conf_par)
                // return "success";

                // Response = consent;
                Response.status.code = APIStatus.success;
                Response.status.message = "Success";
                Response.status.flag = ProcessStatus.success;
            }
            else if (Convert.ToInt16(dt.Rows[0][0]) == 0)
            {   // return "Not an EKYC authenticated customer.";
                Response.status.code = APIStatus.no_Data_Found;
                Response.status.message = "Not an EKYC authenticated customer.";
                Response.status.flag = ProcessStatus.success;
            }
            else
            {   // return "Consent already uploaded.";
                Response.status.code = APIStatus.alreadyExist;
                Response.status.message = "Consent already uploaded.";
                Response.status.flag = ProcessStatus.success;
            }

            return Response;
        }



    }
}
