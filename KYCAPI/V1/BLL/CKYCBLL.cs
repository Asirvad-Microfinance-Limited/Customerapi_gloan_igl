using System;
using System.Data;
using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using GlobalValues;
using KYCAPI.V1.Controllers;
using KYCAPI.V1.Models.Properties;
using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using Oracle.ManagedDataAccess.Client;
using static GlobalValues.GlobalVariables;
using System.Linq;
using KYCAPI.V1.BLLDependency;

namespace KYCAPI.V1.BLL
{
    public class CKYCBLL : APIBaseBLL , ICKYCBLL
    {

        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public CKYCBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }
        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get CKYC Details</summary> 
        public CKYCMasterDataResponse getCKYCDetails(CKYCMasterDataRequest cKYCMasterDataRequest)
        {
            CKYCMasterDataResponse response = new CKYCMasterDataResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string statusQuery = "select s.status_id, s.description,s.module_id ,s.option_id ,s.order_by , s.Type from(" +
                "select t.status_id, t.description, t.module_id , t.option_id , t.order_by, 1 as Type from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.order_by = 1" +
                "union all " +
                "select t.status_id, t.description, t.module_id , t.option_id , t.order_by, 2 as Type from STATUS_MASTER t where module_id = 899 and t.option_id = 2 and t.order_by = 1" +
                "union all " +
                "select t.status_id, t.description,t.module_id ,t.option_id ,t.order_by, 3 as Type from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.status_id in (1,4) and t.order_by = 1" +
                "union all " +
                "select t.status_id, t.description,t.module_id ,t.option_id ,t.order_by , 4 as Type from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.status_id in (2,3,4) and t.order_by = 1" +
                "union all " +
                "select t.status_id, t.description,t.module_id ,t.option_id ,t.order_by , 5 as Type from STATUS_MASTER t where module_id = 899 and t.option_id = 3 and t.order_by = 1" +
                ")s";

                var KYCPStatusList = DapperHelper.GetRecords<CKYCStatusMasterProperties>(statusQuery,SQLMode.Query,null);
                long[] status_id1 = { 1, 3 };
                long[] status_id2 = { 2, 3, 4 };
                if (KYCPStatusList != null && KYCPStatusList.Count > 0)
                {
                    response.namePFXList = KYCPStatusList.Where(a => a.TYPE==1).Select(c => new CKYCPStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).OrderBy(a=>a.DESCRIPTION).ToList();
                    response.fatSpouseList = KYCPStatusList.Where(a => a.TYPE == 2).Select(c => new CKYCPStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).OrderBy(a => a.DESCRIPTION).ToList();
                    response.malePFXList = KYCPStatusList.Where(a =>  a.TYPE == 3).Select(c => new CKYCPStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).OrderBy(a => a.DESCRIPTION).ToList();
                    response.femalePFXList = KYCPStatusList.Where(a =>  a.TYPE == 4).Select(c => new CKYCPStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).OrderBy(a => a.DESCRIPTION).ToList();
                    response.spousePFXList = KYCPStatusList.Where(a =>  a.TYPE == 4).Select(c => new CKYCPStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).OrderBy(a => a.DESCRIPTION).ToList();
                    response.addresstypeList = KYCPStatusList.Where(a =>  a.TYPE == 5).Select(c => new CKYCPStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).OrderBy(a => a.DESCRIPTION).ToList();
                }

                string countryQuery = "select country_id,country_name from country_dtl  order by country_id ";
                response.countryList= DapperHelper.GetRecords<CKYCCountriesProperties>(countryQuery,SQLMode.Query,null);
                string customerdetailsQuery = "select t.resident, t.fathus_pre, c.country_id from CUSTOMER_detail t, customer c where t.cust_id = '" + cKYCMasterDataRequest.custId + "' and c.cust_id = t.cust_id";
                response.customerDetails = DapperHelper.GetRecord<CustomerDetailsProperties>(customerdetailsQuery,SQLMode.Query,null);
                string ckyccustomerdetailsQuery = "select CUST_ID, NAME_PFX, FIRST_NAME, MIDDLE_NAME, LAST_NAME, FAT_PFX, FAT_F_NAME, FAT_M_NAME, FAT_L_NAME, FAT_SPOUSE_FLG, MOTHER_PFX, MOTH_F_NAME, MOTH_M_NAME, MOTH_L_NAME, JURIS_RESID, TAX_ID_NUM, BIRTH_COUNTRY, ADDR_TYPE, USER_ID, TRA_DT, STATUS_ID from TBL_CUST_CKYC_DTL t where t.cust_id = '" + cKYCMasterDataRequest.custId + "'";
                response.CKYCcustomerDetails = DapperHelper.GetRecord<CKYCCustomerDetailsProperties>(ckyccustomerdetailsQuery,SQLMode.Query,null);

                response.status.code = APIStatus.success;
                response.status.message = "Success";
                response.status.flag = ProcessStatus.success;



                //{
                //    response = consent;
                //    response.status.code = APIStatus.success;
                //    response.status.message = "Success";
                //    response.status.flag = ProcessStatus.success;
                //}
                //else
                //{
                //    response.status.code = APIStatus.no_Data_Found;
                //    response.status.message = "No data found for your search";
                //    response.status.flag = ProcessStatus.success;
                //}
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
        /// <summary>Add CKYC Details</summary> 
        public CKYCResponse addCKYCDetails(CKYCRequest request)
        {
            CKYCResponse response = new CKYCResponse();
            //DBAccessHelper helper = new DBAccessHelper();
            
            try
            {
                OracleParameter[] arParms = new OracleParameter[21];

                arParms[0] = new OracleParameter("CustId", OracleDbType.Varchar2, 100);
                arParms[0].Value = request.custID;
                arParms[0].Direction = ParameterDirection.Input;

                arParms[1] = new OracleParameter("CustPfx", OracleDbType.Int64);
                arParms[1].Value = request.CustPfx;
                arParms[1].Direction = ParameterDirection.Input;

                arParms[2] = new OracleParameter("CustFName", OracleDbType.Varchar2, 100);
                arParms[2].Value = request.CustFName;
                arParms[2].Direction = ParameterDirection.Input;

                arParms[3] = new OracleParameter("CustMName", OracleDbType.Varchar2, 100);
                arParms[3].Value = request.CustMName;
                arParms[3].Direction = ParameterDirection.Input;

                arParms[4] = new OracleParameter("CustLName", OracleDbType.Varchar2, 100);
                arParms[4].Value = request.CustLName;
                arParms[4].Direction = ParameterDirection.Input;

                arParms[5] = new OracleParameter("FatSpouse", OracleDbType.Int32);
                arParms[5].Value = request.FatSpouse;
                arParms[5].Direction = ParameterDirection.Input;

                arParms[6] = new OracleParameter("FatPfx", OracleDbType.Int32);
                arParms[6].Value = request.FatPfx;
                arParms[6].Direction = ParameterDirection.Input;

                arParms[7] = new OracleParameter("FatFname", OracleDbType.Varchar2, 100);
                arParms[7].Value = request.FatFname;
                arParms[7].Direction = ParameterDirection.Input;

                arParms[8] = new OracleParameter("FatMname", OracleDbType.Varchar2, 100);
                arParms[8].Value = request.FatMname;
                arParms[8].Direction = ParameterDirection.Input;

                arParms[9] = new OracleParameter("FatLname", OracleDbType.Varchar2, 100);
                arParms[9].Value = request.FatLname;
                arParms[9].Direction = ParameterDirection.Input;

                arParms[10] = new OracleParameter("MotPfx", OracleDbType.Int32);
                arParms[10].Value = request.MotPfx;
                arParms[10].Direction = ParameterDirection.Input;

                arParms[11] = new OracleParameter("MotFname", OracleDbType.Varchar2, 100);
                arParms[11].Value = request.MotFname;
                arParms[11].Direction = ParameterDirection.Input;

                arParms[12] = new OracleParameter("MotMname", OracleDbType.Varchar2, 100);
                arParms[12].Value = request.MotMname;
                arParms[12].Direction = ParameterDirection.Input;

                arParms[13] = new OracleParameter("MotLname", OracleDbType.Varchar2, 100);
                arParms[13].Value = request.MotLname;
                arParms[13].Direction = ParameterDirection.Input;

                arParms[14] = new OracleParameter("JurisRes", OracleDbType.Int32);
                arParms[14].Value = request.JurisRes;
                arParms[14].Direction = ParameterDirection.Input;

                arParms[15] = new OracleParameter("TaxId", OracleDbType.Varchar2,100);
                arParms[15].Value = request.TaxId;
                arParms[15].Direction = ParameterDirection.Input;

                arParms[16] = new OracleParameter("BirCtry", OracleDbType.Int32);
                arParms[16].Value = request.BirCtry;
                arParms[16].Direction = ParameterDirection.Input;

                arParms[17] = new OracleParameter("AddrTyp", OracleDbType.Varchar2,100);
                arParms[17].Value = request.AddrTyp;
                arParms[17].Direction = ParameterDirection.Input;

                arParms[18] = new OracleParameter("UserId", OracleDbType.Int32);
                arParms[18].Value = request.UserID;
                arParms[18].Direction = ParameterDirection.Input;

                arParms[19] = new OracleParameter("UserFlg", OracleDbType.Int32);
                arParms[19].Value = request.UserFlg;
                arParms[19].Direction = ParameterDirection.Input;

                arParms[20] = new OracleParameter("OutMessage", OracleDbType.Varchar2, 1000);
                arParms[20].Direction = ParameterDirection.Output;

                helper.ExecuteNonQuery("proc_add_ckyc_dtl", arParms);
                if (arParms[20].Value != DBNull.Value)
                {
                    string strResult = Convert.ToString(arParms[20].Value);
                    response.consentData = strResult;
                    response.status.code = APIStatus.success;
                    response.status.message = strResult;
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.consentData = "Adding CKYC details failed";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Adding CKYC details failed";
                    response.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {
                response.consentData = ex.Message;
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }
            return response;
        }

    }

   
}
