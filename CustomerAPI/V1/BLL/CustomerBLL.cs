using CustomerAPI.V1.Models.Request;
using DataAccessLibrary;
using CustomerAPI.V1.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using static GlobalValues.GlobalVariables;
using APIBaseClassLibrary.V1.BLL;
using CustomerAPI.V1.Models.Response;
using GlobalValues;
using CustomerAPI.V1.Models.Properties;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dapper;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Net.Mail;
using APIBaseClassLibrary.V1.Models.Request;
using static GlobalValues.GlobalVariables.Constants;

namespace CustomerAPI.V1.BLL
{
    public class CustomerBLL : APIBaseBLL, ICustomerBLL
    {

        string invalidParamMessage = "";
        public IDBAccessHelper helper;
        IGlobalMethods globalMethods;
        public IDBAccessDapperHelper DapperHelper;
        public CustomerBLL(IDBAccessHelper _helper, IGlobalMethods _globalMethods, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            invalidParamMessage = "";
            globalMethods = _globalMethods;
            DapperHelper = _DapperHelper;
        }

        /// <Created>Uneesh - 100156</Created>
        /// <summary>Add Customer Details</summary> 
        public CustomerResponse addCustomer(CustomerAddRequest customer)
        {
            CustomerResponse response = response = new CustomerResponse();

            if (customer.kycIDType == 506) //Money transfer PanCard
            {
                Regex regex = new Regex(Constants.Strings.panNoFormat);
                if (!regex.IsMatch(customer.kycIdNo))
                {
                    response.customerID = "";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Please enter valid pancard number";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }            
            }
            if (customer.kycIDType == 1 || customer.kycIDType == 503 || customer.kycIDType == 553) //Money transfer PanCard
            {
                Regex regex = new Regex(Constants.Strings.passportFormat);
                if (!regex.IsMatch(customer.kycIdNo))
                {
                    response.customerID = "";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Please enter valid passport number";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
            }
            if (customer.kycIDType == 16 || customer.kycIDType == 505 || customer.kycIDType == 555) //Money transfer PanCard
            {
                Regex regex = new Regex(Constants.Strings.adharCardFormat);
                if (!regex.IsMatch(customer.kycIdNo) || customer.kycIdNo.Length != 12) //Adhar number validation added on 28-jan-2020 --Sreerekha ---100006
                {
                    response.customerID = "";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Please enter valid UIDAI CARD (AADHAR) number";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
                
            }
            //While updating KYC ID number, symbols other than “-“ and “/” should not be allowed to be updated against KYC ID 
            else
            {
                Regex regex1 = new Regex(Constants.Strings.kycNumberFormat);
                if (!regex1.IsMatch(customer.kycIdNo))
                {
                    response.customerID = "";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Pls enter KYC number as shown in the document(Symbols other than “-“ and “/” should not be allowed )";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
            }

            if (customer.kycIDType == 2 ) //Driving License Length Validation
            {
                if (customer.kycIdNo.Length>= 10 && customer.kycIdNo.Length <=20)
                {
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.customerID = "";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Pls enter KYC number as shown in the document(Minimum 10 characters,Maximum 20)";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
            }
            if (customer.kycIDType == 3) //Voters ID card Length Validation
            {
                if (customer.kycIdNo.Length >= 8 && customer.kycIdNo.Length <= 20)
                {
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.customerID = "";
                    response.status.code = APIStatus.failed;
                    response.status.message = "Pls enter KYC number as shown in the document(Minimum 8 characters,Maximum 20)";
                    response.status.flag = ProcessStatus.success;
                    return response;
                }
            }


            try
            {
                response = new CustomerResponse();
               //DBAccessHelper helper = new DBAccessHelper();
                OracleParameter[] parm_coll = new OracleParameter[72];
                string message;
                bool validMethod = true;
                // ---------------------------------------------------------------------------------------------------
                try
                {
                    
                    if (validateAddCustomerRequest(customer))
                    {
                        parm_coll[0] = new OracleParameter("fmno", OracleDbType.Long, 5);
                        parm_coll[0].Value = customer.firmID;
                        parm_coll[0].Direction = ParameterDirection.Input;

                        parm_coll[1] = new OracleParameter("brno", OracleDbType.Long, 5);
                        parm_coll[1].Value = customer.branchID;
                        parm_coll[1].Direction = ParameterDirection.Input;

                        parm_coll[2] = new OracleParameter("cusname", OracleDbType.Varchar2, 40);
                        parm_coll[2].Value = customer.custName;
                        parm_coll[2].Direction = ParameterDirection.Input;

                        parm_coll[3] = new OracleParameter("tele", OracleDbType.Varchar2, 15);
                        parm_coll[3].Value = customer.phoneno;
                        parm_coll[3].Direction = ParameterDirection.Input;

                        parm_coll[4] = new OracleParameter("mob", OracleDbType.Varchar2, 15);
                        parm_coll[4].Value = customer.mobileNo;
                        parm_coll[4].Direction = ParameterDirection.Input;

                        parm_coll[5] = new OracleParameter("fathus", OracleDbType.Varchar2, 40);
                        parm_coll[5].Value = customer.fatHusName;
                        parm_coll[5].Direction = ParameterDirection.Input;

                        parm_coll[6] = new OracleParameter("housenm", OracleDbType.Varchar2, 40);
                        parm_coll[6].Value = customer.houseName;
                        parm_coll[6].Direction = ParameterDirection.Input;

                        parm_coll[7] = new OracleParameter("loca", OracleDbType.Varchar2, 100);
                        parm_coll[7].Value = customer.location;
                        parm_coll[7].Direction = ParameterDirection.Input;

                        //------------
                        parm_coll[8] = new OracleParameter("pinsrl", OracleDbType.Long, 7);
                        parm_coll[8].Value = customer.pinsrl;
                        parm_coll[8].Direction = ParameterDirection.Input;

                        parm_coll[9] = new OracleParameter("email_id", OracleDbType.Varchar2, 35);
                        parm_coll[9].Value = customer.email;
                        parm_coll[9].Direction = ParameterDirection.Input;

                        parm_coll[10] = new OracleParameter("occ_id", OracleDbType.Long, 2);
                        parm_coll[10].Value = customer.occupationID;
                        parm_coll[10].Direction = ParameterDirection.Input;

                        parm_coll[11] = new OracleParameter("pass_id", OracleDbType.Varchar2, 35);
                        parm_coll[11].Value = "";
                        parm_coll[11].Direction = ParameterDirection.Input;

                        parm_coll[12] = new OracleParameter("pan", OracleDbType.Varchar2, 35);
                        parm_coll[12].Value = customer.panNo;
                        parm_coll[12].Direction = ParameterDirection.Input;

                        parm_coll[13] = new OracleParameter("dob", OracleDbType.Date, 11);
                        if (!string.IsNullOrEmpty(customer.dob))
                        {
                            //parm_coll[13].Value = String.Format(Constants.Strings.dateFormat, Convert.ToDateTime(customer.dob));
                            parm_coll[13].Value = customer.dob;
                        }
                        else
                        {
                            parm_coll[13].Value = "";
                        }
                        parm_coll[13].Direction = ParameterDirection.Input;

                        parm_coll[14] = new OracleParameter("cust_type", OracleDbType.Long, 2);
                        parm_coll[14].Value = customer.custStatus;
                        parm_coll[14].Direction = ParameterDirection.Input;

                        parm_coll[15] = new OracleParameter("cntid", OracleDbType.Long, 4);
                        parm_coll[15].Value = customer.countryID;
                        parm_coll[15].Direction = ParameterDirection.Input;

                        parm_coll[16] = new OracleParameter("custid", OracleDbType.Varchar2, 200);
                        parm_coll[16].Direction = ParameterDirection.Output;

                        parm_coll[17] = new OracleParameter("id", OracleDbType.Long, 3);
                        parm_coll[17].Value = customer.kycIDType;
                        parm_coll[17].Direction = ParameterDirection.Input;

                        parm_coll[18] = new OracleParameter("id_no", OracleDbType.Varchar2, 150);
                        parm_coll[18].Value = customer.kycIdNo;
                        parm_coll[18].Direction = ParameterDirection.InputOutput;

                        parm_coll[19] = new OracleParameter("is_dt", OracleDbType.Date, 11);
                        if (!string.IsNullOrEmpty(customer.kycIssueDate))
                        {
                            //parm_coll[19].Value = String.Format(Constants.Strings.dateFormat, Convert.ToDateTime(customer.kycIssueDate));
                            parm_coll[19].Value = customer.kycIssueDate;
                        }
                        else
                        {
                            parm_coll[19].Value = DBNull.Value;
                        }
                        parm_coll[19].Direction = ParameterDirection.Input;

                        parm_coll[20] = new OracleParameter("ex_dt", OracleDbType.Date, 11);
                        if (!string.IsNullOrEmpty(customer.kycExpiryDate))
                        {
                            //parm_coll[20].Value = String.Format(Constants.Strings.dateFormat, Convert.ToDateTime(customer.kycExpiryDate));
                            parm_coll[20].Value = customer.kycExpiryDate;
                        }
                        else
                        {
                            parm_coll[20].Value = DBNull.Value;
                        }
                        parm_coll[20].Direction = ParameterDirection.Input;

                        parm_coll[21] = new OracleParameter("is_plce", OracleDbType.Varchar2, 40);
                        parm_coll[21].Value = customer.kycIssuePlace;
                        parm_coll[21].Direction = ParameterDirection.Input;

                        parm_coll[22] = new OracleParameter("gen", OracleDbType.Long, 2);
                        parm_coll[22].Value = customer.gender;
                        parm_coll[22].Direction = ParameterDirection.Input;

                        parm_coll[23] = new OracleParameter("p_street", OracleDbType.Varchar2, 40);
                        parm_coll[23].Value = customer.location;
                        parm_coll[23].Direction = ParameterDirection.Input;

                        parm_coll[24] = new OracleParameter("p_media_id", OracleDbType.Long, 5);
                        parm_coll[24].Value = customer.mediaID;
                        parm_coll[24].Direction = ParameterDirection.Input;

                        parm_coll[25] = new OracleParameter("p_module_id", OracleDbType.Long, 8);
                        parm_coll[25].Value = customer.moduleID;// "0";
                        parm_coll[25].Direction = ParameterDirection.Input;

                        parm_coll[26] = new OracleParameter("userid", OracleDbType.Varchar2, 40);
                        parm_coll[26].Value = customer.empCode;
                        parm_coll[26].Direction = ParameterDirection.Input;

                        parm_coll[27] = new OracleParameter("typeid", OracleDbType.Varchar2, 25);
                        parm_coll[27].Value = customer.mediaTypeID;
                        parm_coll[27].Direction = ParameterDirection.Input;

                        parm_coll[28] = new OracleParameter("cardNo", OracleDbType.Varchar2, 40);
                        parm_coll[28].Value = customer.loyaltyCardNo;
                        parm_coll[28].Direction = ParameterDirection.Input;

                        parm_coll[29] = new OracleParameter("shareflag", OracleDbType.Varchar2, 2);
                        parm_coll[29].Value = customer.shareFlag;
                        parm_coll[29].Direction = ParameterDirection.Input;

                        parm_coll[30] = new OracleParameter("err_msg", OracleDbType.Varchar2, 300);
                        parm_coll[30].Direction = ParameterDirection.Output;

                        parm_coll[31] = new OracleParameter("err_stat", OracleDbType.Long, 1);
                        parm_coll[31].Direction = ParameterDirection.Output;

                        parm_coll[32] = new OracleParameter("neftdetails", OracleDbType.Varchar2, 1000);
                        parm_coll[32].Direction = ParameterDirection.Input;
                        if (string.IsNullOrEmpty(customer.neftData))
                        {
                            parm_coll[32].Value = DBNull.Value;
                        }
                        else
                        {
                            parm_coll[32].Value = customer.neftData;
                        }

                        parm_coll[33] = new OracleParameter("LandHLD", OracleDbType.Long, 10);
                        parm_coll[33].Direction = ParameterDirection.Input;
                        parm_coll[33].Value = customer.landDtlID;

                        parm_coll[34] = new OracleParameter("EX_STATUS", OracleDbType.Long, 2);
                        parm_coll[34].Direction = ParameterDirection.Input;
                        parm_coll[34].Value = customer.exServiceStatus;

                        parm_coll[35] = new OracleParameter("EX_NO", OracleDbType.Varchar2, 40);
                        parm_coll[35].Direction = ParameterDirection.Input;
                        parm_coll[35].Value = customer.exServiceNo;

                        parm_coll[36] = new OracleParameter("relgn", OracleDbType.Long, 2);
                        parm_coll[36].Value = customer.religionID;
                        parm_coll[36].Direction = ParameterDirection.Input;

                        parm_coll[37] = new OracleParameter("cst", OracleDbType.Long, 2);
                        parm_coll[37].Value = customer.casteID;
                        parm_coll[37].Direction = ParameterDirection.Input;

                        parm_coll[38] = new OracleParameter("purofloan", OracleDbType.Long, 2);
                        parm_coll[38].Value = customer.loanPurpose;
                        parm_coll[38].Direction = ParameterDirection.Input;

                        parm_coll[39] = new OracleParameter("alt_hname", OracleDbType.Varchar2, 40);
                        parm_coll[39].Direction = ParameterDirection.Input;
                        parm_coll[39].Value = customer.alt_Housename;

                        parm_coll[40] = new OracleParameter("alt_loca", OracleDbType.Varchar2, 40);
                        parm_coll[40].Direction = ParameterDirection.Input;
                        parm_coll[40].Value = customer.alt_Locality;

                        parm_coll[41] = new OracleParameter("alt_pin", OracleDbType.Long, 7);
                        parm_coll[41].Direction = ParameterDirection.Input;
                        parm_coll[41].Value = customer.alt_Post;

                        if(customer.kycType==3)
                        {
                            customer.kycType = 1;
                        }
                        else
                        {
                            customer.kycType = 0;
                        }

                        parm_coll[42] = new OracleParameter("kyc_ml", OracleDbType.Long, 2);
                        parm_coll[42].Direction = ParameterDirection.Input;
                        parm_coll[42].Value = customer.kycType;

                        parm_coll[43] = new OracleParameter("kyc_of", OracleDbType.Long, 2);
                        parm_coll[43].Direction = ParameterDirection.Input;
                        parm_coll[43].Value = customer.kycOf;

                   
                        parm_coll[44] = new OracleParameter("p_isactive", OracleDbType.Long, 2);
                        parm_coll[44].Direction = ParameterDirection.Input;

                        if (customer.isActive != Constants.Ints.customer_Recommend)
                        {
                            if ((customer.relationIdentity == 23) || (customer.residentialStatus == 2) || (customer.countryID != 1))
                            {
                                customer.isActive = Constants.Ints.customer_NRI;
                            }
                            else {
                                customer.isActive = Constants.Ints.customer_Normal;
                            }
                        }
                        parm_coll[44].Value = customer.isActive;



                        parm_coll[45] = new OracleParameter("cust_cat", OracleDbType.Long, 2);
                        parm_coll[45].Direction = ParameterDirection.Input;
                        parm_coll[45].Value = customer.customerCategory;

                        parm_coll[46] = new OracleParameter("DSA_BA_USER", OracleDbType.Long, 10);
                        parm_coll[46].Direction = ParameterDirection.Input;
                        parm_coll[46].Value = customer.baCode;

                        parm_coll[47] = new OracleParameter("Preflang", OracleDbType.Long, 2);
                        parm_coll[47].Direction = ParameterDirection.Input;
                        parm_coll[47].Value = customer.prefLang;

                        parm_coll[48] = new OracleParameter("KycRem", OracleDbType.Varchar2, 500);
                        parm_coll[48].Direction = ParameterDirection.Input;
                        parm_coll[48].Value = customer.kycRemark;

                        parm_coll[49] = new OracleParameter("PhotoRem", OracleDbType.Varchar2, 500);
                        parm_coll[49].Direction = ParameterDirection.Input;
                        parm_coll[49].Value = customer.photoRemark;

                        parm_coll[50] = new OracleParameter("Cust_PEP", OracleDbType.Long, 2);
                        parm_coll[50].Direction = ParameterDirection.Input;
                        parm_coll[50].Value = customer.pep;

                        parm_coll[51] = new OracleParameter("CustMom", OracleDbType.Varchar2, 38);
                        parm_coll[51].Direction = ParameterDirection.Input;
                        parm_coll[51].Value = customer.mothername;

                        parm_coll[52] = new OracleParameter("Citizenship", OracleDbType.Long, 2);
                        parm_coll[52].Direction = ParameterDirection.Input;
                        parm_coll[52].Value = customer.citizen;

                        parm_coll[53] = new OracleParameter("Nation", OracleDbType.Long, 2);
                        parm_coll[53].Direction = ParameterDirection.Input;
                        parm_coll[53].Value = customer.nationality;

                        parm_coll[54] = new OracleParameter("Resident", OracleDbType.Long, 2);
                        parm_coll[54].Direction = ParameterDirection.Input;
                        parm_coll[54].Value = customer.residentialStatus;

                        parm_coll[55] = new OracleParameter("MaritalStat", OracleDbType.Long, 2);
                        parm_coll[55].Direction = ParameterDirection.Input;
                        parm_coll[55].Value = customer.marital;

                        parm_coll[56] = new OracleParameter("FatHusPre", OracleDbType.Long, 2);
                        parm_coll[56].Direction = ParameterDirection.Input;
                        parm_coll[56].Value = customer.relationIdentity;

                        parm_coll[57] = new OracleParameter("Prename", OracleDbType.Long, 2);
                        parm_coll[57].Direction = ParameterDirection.Input;
                        parm_coll[57].Value = customer.preName;

                        parm_coll[58] = new OracleParameter("AddrFlg", OracleDbType.Long, 2);
                        parm_coll[58].Direction = ParameterDirection.Input;
                        parm_coll[58].Value = customer.address_Flg;

                        parm_coll[59] = new OracleParameter("EduQual", OracleDbType.Long, 2);
                        parm_coll[59].Direction = ParameterDirection.Input;
                        parm_coll[59].Value = customer.qualification;

                        parm_coll[60] = new OracleParameter("NeedForLoan", OracleDbType.Long, 2);
                        parm_coll[60].Direction = ParameterDirection.Input;
                        parm_coll[60].Value = customer.reason;

                        parm_coll[61] = new OracleParameter("MonthIncome", OracleDbType.Long, 2);
                        parm_coll[61].Direction = ParameterDirection.Input;
                        parm_coll[61].Value = customer.income;

                        parm_coll[62] = new OracleParameter("FirstGL", OracleDbType.Long, 2);
                        parm_coll[62].Direction = ParameterDirection.Input;
                        parm_coll[62].Value = customer.firstGL;

                        parm_coll[63] = new OracleParameter("Rrn", OracleDbType.Varchar2, 50);
                        parm_coll[63].Direction = ParameterDirection.Input;
                        parm_coll[63].Value = customer.RRN;

                        parm_coll[64] = new OracleParameter("Uu_id", OracleDbType.Varchar2, 150);
                        parm_coll[64].Direction = ParameterDirection.Input;
                        parm_coll[64].Value = customer.UUID;

                        parm_coll[65] = new OracleParameter("Ckycid", OracleDbType.Varchar2, 150);
                        parm_coll[65].Direction = ParameterDirection.Input;
                        parm_coll[65].Value = customer.CKYCNumber;

                        parm_coll[66] = new OracleParameter("CustSource", OracleDbType.Varchar2, 150);
                        parm_coll[66].Direction = ParameterDirection.Input;
                        parm_coll[66].Value = customer.CustSource;

                        parm_coll[67] = new OracleParameter("BusiCat", OracleDbType.Long, 2);
                        parm_coll[67].Direction = ParameterDirection.Input;
                        parm_coll[67].Value = customer.businessCategory;

                        parm_coll[68] = new OracleParameter("BusiSubCat", OracleDbType.Long, 2);
                        parm_coll[68].Direction = ParameterDirection.Input;
                        parm_coll[68].Value = customer.businessSubCategory;

                        parm_coll[69] = new OracleParameter("facebookId", OracleDbType.Varchar2 , 100);
                        parm_coll[69].Direction = ParameterDirection.Input;
                        parm_coll[69].Value = customer.facebookID;
                        //- new parameter added for KYC master directions implementation on 2-mar-2020---Sreerekha K 100006
                        parm_coll[70] = new OracleParameter("AdrPrfTyp", OracleDbType.Long, 2);
                        parm_coll[70].Direction = ParameterDirection.Input;
                        parm_coll[70].Value = customer.addressProof_Type;

                        parm_coll[71] = new OracleParameter("billDate", OracleDbType.Date, 15);
                       
                        if (!string.IsNullOrEmpty(customer.addressProof_Billdate))
                        {
                            parm_coll[71].Value = customer.addressProof_Billdate;
                        }
                        else
                        {
                            parm_coll[71].Value = DBNull.Value;
                        }
                        parm_coll[71].Direction = ParameterDirection.Input;

                       
                        try
                        {
                            if (validMethod)
                            {
                                helper.ExecuteNonQuery("add_customer", parm_coll);



                                //if (parm_coll[16].Value != null)
                                //{
                                //    if (parm_coll[16].Value.ToString().Length > 0)//String.IsNullOrEmpty(Convert.ToString(parm_coll[16].Value)))
                                //    {
                                //        message = parm_coll[30].Value.ToString() + "+" + parm_coll[31].Value.ToString() + "+" + parm_coll[16].Value.ToString();
                                //        if (parm_coll[30].Value.ToString().ToLower() == "customer id recommended for srm/rm approval")
                                //        {
                                //            Srm_Customer_ApprovalMail(Convert.ToString(parm_coll[16].Value), customer.branchID);

                                //            response.customerID = parm_coll[16].Value.ToString();
                                //            response.status.code = APIStatus.SRM_RM_Approval_Required;
                                //            response.status.message = "Customer ID recommended for SRM/RM approval";
                                //            response.status.flag = ProcessStatus.success;
                                //        }
                                //        else
                                //        {
                                //            response.customerID = parm_coll[16].Value.ToString();
                                //            response.status.code = APIStatus.success;
                                //            response.status.message = "Success";
                                //            response.status.flag = ProcessStatus.success;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        response.customerID = "";
                                //        response.status.code = APIStatus.failed;
                                //        response.status.message = "Failed to insert the customer details";
                                //        response.status.flag = ProcessStatus.success;
                                //    }
                                //}
                                //else
                                //{
                                //    response.customerID = "";
                                //    response.status.code = APIStatus.failed;
                                //    response.status.message = "Failed to insert the customer details";
                                //    response.status.flag = ProcessStatus.success;
                                //}
                                if (parm_coll[31].Value.ToString() != null)
                                {

                                    if (parm_coll[31].Value.ToString() == "1")
                                    {
                                        response.customerID = parm_coll[16].Value.ToString();
                                        response.status.code = APIStatus.success;
                                        response.status.message = parm_coll[30].Value.ToString();
                                        response.status.flag = ProcessStatus.success;
                                        
                                        if (customer.email != "")
                                        {
                                            //Add Customer Survey Link in welcome mail --Done by sreerekha 100006---28-may-2020
                                            OracleParameter[] parm_coll1 = new OracleParameter[5];
                                           
                                            parm_coll1[0] = new OracleParameter("p_flag", OracleDbType.Long, 2);
                                            parm_coll1[0].Value = 50;
                                            parm_coll1[0].Direction = ParameterDirection.Input;
                                            parm_coll1[1] = new OracleParameter("p_indata", OracleDbType.Varchar2, 100);
                                            parm_coll1[1].Value = "GETSURVAYLINKµMAILIDµCUSTOMERNAMEµCUSTOMERIDµ1";
                                            parm_coll1[1].Direction = ParameterDirection.Input;
                                            parm_coll1[2] = new OracleParameter("as_outresult", OracleDbType.RefCursor, 500);
                                            parm_coll1[2].Direction = ParameterDirection.Output;
                                            parm_coll1[3] = new OracleParameter("p_ErrorStat", OracleDbType.Long, 5);
                                            parm_coll1[3].Direction = ParameterDirection.Output;
                                            parm_coll1[4] = new OracleParameter("p_errormsg", OracleDbType.Varchar2, 500);
                                            parm_coll1[4].Direction = ParameterDirection.Output;
                                            DataSet custMail= helper.ExecuteDataSet("proc_doorstep_queries", parm_coll1);
                                            string mailllll = custMail.Tables[0].Rows[0][0].ToString();
                                            
                                            //Welcome message while adding customer--- Sreerekha K 100006--- 24-feb-2020
                                            //var SendSmsmessage = SendEMAil(customer.email, mailllll, parm_coll[16].Value.ToString(), customer.custName);

                                        }
                                        //Welcome SMS while adding customer --- Sreerekha K 100006 ---22-may-2020
                                        //Sending welcome message (SMS) to our new customers in vernacular languages-- 8-Oct-2020
                                        string mailsql;
                                        mailsql = "select a.content from state_master s, TBL_LOCAL_LANGUAGE  t,TBL_AUTO_MAIL_CONTENT a, customer cs ,post_master pm, district_master dm where s.state_id = dm.state_id  and s.state_name = t.col_state_name and a.lang_id = t.col_lang_id   and a.content_id = 501  and cs.cust_id = '" +parm_coll[16].Value.ToString()+ "'  and cs.pin_serial = pm.sr_number  and dm.district_id = pm.district_id";
                                        DataTable dtmail = helper.ExecuteDataSet(mailsql).Tables[0];
                                        SmsRequest smsRequest = new SmsRequest();
                                        smsRequest.Message = Convert.ToString(dtmail.Rows[0][0]);
                                        smsRequest.mobileNo = customer.mobileNo;
                                        smsRequest.accountType = SMSAccMode.transactionsms;
                                        base.SendSMS(smsRequest);
                                        /// Business associates --send MAIL & SMS -- done by Sreerekha K ---20-Jun-2020
                                        //if (customer.mediaTypeID == 4 && customer.baCode != "")
                                        //{
                                        //    string sql;
                                        //    sql = "select t.email from tbl_ba_mailid t where t.bacode= "+ customer.baCode+"";
                                        //    DataTable dt = helper.ExecuteDataSet(sql).Tables[0];
                                        //    string sql1;
                                        //    sql1 = "select t.phone ,upper(t.ba_name) from tbl_add_businessagent t where t.ba_code=" + customer.baCode + "";
                                        //    DataTable dt1 = helper.ExecuteDataSet(sql1).Tables[0];
                                        //    if (dt.Rows.Count > 0 )
                                        //    {
                                        //        if (Convert.ToString(dt.Rows[0][0]) != "")
                                        //        { 
                                        //        var SendSmsmessage = SendEMAilToBA(Convert.ToString(dt.Rows[0][0]), parm_coll[16].Value.ToString(), customer.custName, customer.baCode, Convert.ToString(dt1.Rows[0][1]));
                                        //        }
                                        //    }
                                        //    //Send SMS to Business associates while adding customer--- 20-jun-2020
                                        //    if (dt1.Rows.Count > 0 && Convert.ToString(dt1.Rows[0][0]) != "")
                                        //    {
                                        //        string customerurl = "https://online.manappuram.com/custsurvey/CustomerSurveyBA.aspx?cid=1~" + Convert.ToString(customer.baCode) + " ";
                                        //        smsRequest.Message = "Dear "+ Convert.ToString(dt1.Rows[0][1]) + ",Hearty congratulations for mobilizing a new customer!We hope the same good work will continue in the coming days too.Also, please share your valuable feedback by clicking on the below link:"+ customerurl +"";
                                        //        smsRequest.mobileNo = Convert.ToString(dt1.Rows[0][0]);
                                        //        smsRequest.accountType = SMSAccMode.transactionsms;
                                        //        base.SendSMS(smsRequest);
                                        //    }
                                        //}
                                        /// Business associates --send MAIL & SMS -- done by Sreerekha K ---20-Jun-2020
                                        //Save wedding date of customer --12/nov/2020
                                        if (!string.IsNullOrEmpty(customer.weddingDate) && customer.marital == 2)
                                        {
                                            helper.ExecuteNonQuery("insert into TBL_MULTIWAY_LEAD (CUST_NAME,PHONE_NO,DOB,MRT_STS,MRG_DT,MAIL_ID,FB_ID,CUST_ID,BRANCH_ID,ENTERED_BY,ENTERED_DT) values ('" + customer.custName + "','" + customer.mobileNo  + "',to_date('" + Convert.ToDateTime(customer.dob).ToShortDateString() + "','MM-dd-yyyy'),'" + customer.maritalStatus + "',to_date('" + Convert.ToDateTime(customer.weddingDate).ToShortDateString() + "','MM-dd-yyyy'),'" + customer.email + "','" + customer.facebookID  + "','" + parm_coll[16].Value.ToString() + "'," + customer.branchID  + ",'" + customer.empCode +"',to_date(sysdate))");
                                        }
                                        //Save wedding date of customer --12/nov/2020
                                    }
                                    else
                                    {
                                        if (parm_coll[30].Value.ToString() != null)
                                        {

                                            if (parm_coll[30].Value.ToString().ToLower() == "customer id recommended for srm/rm approval")
                                            {
                                                Srm_Customer_ApprovalMail(Convert.ToString(parm_coll[16].Value), customer.branchID);

                                                response.customerID = parm_coll[16].Value.ToString();
                                                response.status.code = APIStatus.failed;
                                                response.status.message = "Customer ID recommended for SRM/RM approval";
                                                response.status.flag = ProcessStatus.success;
                                            }
                                            else
                                            {
                                                response.customerID = parm_coll[16].Value.ToString();
                                                response.status.code = APIStatus.failed;
                                                response.status.message = parm_coll[30].Value.ToString();
                                                response.status.flag = ProcessStatus.success;

                                            }
                                        }
                                        else
                                        {
                                            response.customerID = "";
                                            response.status.code = APIStatus.failed;
                                            response.status.message = "Failed to insert the customer details";
                                            response.status.flag = ProcessStatus.success;
                                        }

                                    }
                                }
                                else
                                {
                                    response.customerID = "";
                                    response.status.code = APIStatus.failed;
                                    response.status.message = "Failed to insert the customer details";
                                    response.status.flag = ProcessStatus.success;
                                }

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
                        response.status.flag = ProcessStatus.success;
                        response.status.code = APIStatus.badRequest;
                        response.status.message = invalidParamMessage;
                    }
                }
                catch (Exception ex)
                {
                    response.status.flag = ProcessStatus.failed;
                    response.status.code = APIStatus.exception;
                    response.status.message = "Exception :- " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = "Exception :- " + ex.Message;
            }
            return response;
        }
        /// <Created>Uneesh - 100156</Created>
        /// <summary>Validate Add Customer Request</summary> 
        private bool validateAddCustomerRequest(CustomerAddRequest customer) // mode=0 for add, mode=1 for update
        {
           //DBAccessHelper helper = new DBAccessHelper();
            bool retVal = true;
            invalidParamMessage = "";             try
            {
                if (customer.mediaID == 14 || customer.mediaID == 29)
                {
                    if (string.IsNullOrEmpty(customer.baCode))
                    {
                        invalidParamMessage = invalidParamMessage + "Please Enter BA/DSA Code or Select Other Media. \r\n";
                        retVal = false;
                    }
                }
 
                if (!customer.customerImageFlag)
                {
                    try
                    {
                        string sql;
                        sql = "select count(*) from pledge_oldconf_mst a,pledge_oldconf_transaction b where a.branch_id= " + customer.branchID +
                            " and a.request_id=b.request_id and to_date(a.request_dt)=to_date(sysdate) and b.status <> 0";
                        DataTable dt = helper.ExecuteDataSet(sql).Tables[0];
                        if (dt.Rows.Count > 0 && Convert.ToInt64(dt.Rows[0][0]) > 0) { }
                        else
                        {
                            invalidParamMessage = invalidParamMessage + "Photo catpturing can be skipped only for entering manual pledge customer. \r\nNo approved back date pledges are available to use this option. \r\n";
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }

                if (customer.custStatus == 4)
                {
                    if (string.IsNullOrEmpty(customer.empCode))
                    {
                        invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                        retVal = false;
                    }
                    else {
                        if (customer.empCode.Contains("*"))
                        {
                            string[] empcodes = customer.empCode.Split("*");
                            if (!string.IsNullOrEmpty(empcodes[1]))
                            {
                                //invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                                //retVal = false;
                            }
                            else
                            {
                                invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                                retVal = false;
                            }
                        }
                        else
                        {
                            invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                            retVal = false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(customer.email))
                {
                    Match rex = Regex.Match(customer.email.Trim(), @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,3})$", RegexOptions.IgnoreCase);
                    if (rex.Success == false)
                    {
                        invalidParamMessage = invalidParamMessage + "Please enter a valid email address. \r\n";
                        retVal = false;
                    }
                    else
                    {
                        DataSet dtmail = new DataSet();
                        string Custmail = customer.email.ToUpper();
                        dtmail = helper.ExecuteDataSet("select count(*)  from TBL_CUSTOMER_VALIDATION t where t.email_id ='" + Custmail + "'");
                        if (Convert.ToInt64(dtmail.Tables[0].Rows[0][0]) > 0)
                        {
                            invalidParamMessage = invalidParamMessage + "This e-mail address is already existing, update the correct e-mail address. \r\n";
                            retVal = false;
                        }
                    }
                }
                if (customer.mediaTypeID>0)
                {
                    if (customer.mediaID<=0)
                    {
                        invalidParamMessage = invalidParamMessage + "Please select the media. \r\n";
                        retVal = false;
                    }
                }

                if (!string.IsNullOrEmpty(customer.panNo))
                {
                    if (customer.panNo.Trim().Length == Constants.Ints.panNumberLength)
                    {
                        string[] panChars = Constants.Strings.validPanChars.Split(",");
                        bool validPan = false;
                        for (int i = 0; i < panChars.Length; i++) {
                            if (panChars[i] == customer.panNo.Substring(3, 1)) {
                                validPan = true;
                                break;
                            }
                        }
                        if (!validPan)
                        {
                            invalidParamMessage = invalidParamMessage + CustomerValidationMessages.Invalid.panNo + " \r\n";
                            retVal = false;
                        }
                    }
                    else
                    {
                        invalidParamMessage = invalidParamMessage + CustomerValidationMessages.Invalid.panNo + " \r\n";
                        retVal = false;
                    }
                }
                // De dupe checking based on Aadhar - 17-oct-2020
                if (customer.kycIDType == 505 || customer.kycIDType == 16 || customer.kycIDType == 555)
                {
                    DataTable dt = new DataTable();
                    string customerdob = Convert.ToDateTime(customer.dob).ToString("dd/MMM/yyyy");  
                    dt = helper.ExecuteDataSet("select b.cust_id  from IDENTITY_DTL t,customer b,customer_detail c where  t.identity_id   in (505,555,16)  and substr(t.id_number,9,4)=substr('" + customer.kycIdNo + "',9,4)  and t.cust_id=b.cust_id and b.name = UPPER('" + customer.custName + "') and c.cust_id = b.cust_id  and to_date(c.date_of_birth) = '" + customerdob + "' and c.gender = " + customer.gender + "").Tables[0];
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
                        invalidParamMessage = invalidParamMessage + "Customer ID already exists with the same Aadhaar Number.Customer IDs matched are as follows "+ CustIDs + " \r\n";
                        retVal = false;
                    }
                }
                // De dupe checking based on Aadhar - 17-oct-2020
                //duplicate, age,check exist are pending

            }
            catch (Exception e)
            {
                invalidParamMessage = invalidParamMessage + e.Message + " \r\n";
                retVal = false;
            }

            return retVal;
        }

        /// <Created>Uneesh - 100156</Created>
        /// <summary>Update Customer Details</summary> 
        public CustomerResponse updateCustomer(CustomerUpdateRequest customer)
        {
            CustomerResponse response = response = new CustomerResponse();
            try
            {
                response = new CustomerResponse();
               //DBAccessHelper helper = new DBAccessHelper();
                OracleParameter[] parm_coll = new OracleParameter[57];
                string message;
                bool validMethod = true;
                //Added wef 28-jan-2020 --- mail confirmation from Bilumon --- Aadhar number validation
                if (customer.key_Id_Flag == 1) 
                {
                    if (customer.kycIDType == 16 || customer.kycIDType == 505 || customer.kycIDType == 555)
                    {
                        Regex regex = new Regex(Constants.Strings.adharCardFormat);
                        if (!regex.IsMatch(customer.kycIdNo) || customer.kycIdNo.Length != 12)
                        {
                            response.customerID = "";
                            response.status.code = APIStatus.failed;
                            response.status.message = "Please enter valid UIDAI CARD (AADHAR) number";
                            response.status.flag = ProcessStatus.success;
                            return response;
                        }
                    }
               //While updating KYC ID number, symbols other than “-“ and “/” should not be allowed to be updated against KYC ID 
                    else
                    {
                        Regex regex = new Regex(Constants.Strings.kycNumberFormat);
                        if (!regex.IsMatch(customer.kycIdNo))
                        {
                            response.customerID = "";
                            response.status.code = APIStatus.failed;
                            response.status.message = "Pls enter KYC number as shown in the document(Symbols other than “-“ and “/” should not be allowed )";
                            response.status.flag = ProcessStatus.success;
                            return response;
                        }
                    }

                    if (customer.kycIDType == 2) //Driving License Length Validation
                    {
                        if (customer.kycIdNo.Length >= 10 && customer.kycIdNo.Length <= 20)
                        {
                            response.status.flag = ProcessStatus.success;
                        }
                        else
                        {
                            response.customerID = "";
                            response.status.code = APIStatus.failed;
                            response.status.message = "Pls enter KYC number as shown in the document(Minimum 10 characters,Maximum 20)";
                            response.status.flag = ProcessStatus.success;
                            return response;
                        }
                    }
                    if (customer.kycIDType == 3) //Voters ID card Length Validation
                    {
                        if (customer.kycIdNo.Length >= 8 && customer.kycIdNo.Length <= 20)
                        {
                            response.status.flag = ProcessStatus.success;
                        }
                        else
                        {
                            response.customerID = "";
                            response.status.code = APIStatus.failed;
                            response.status.message = "Pls enter KYC number as shown in the document(Minimum 8 characters,Maximum 20)";
                            response.status.flag = ProcessStatus.success;
                            return response;
                        }
                    }

                    if (customer.kycIDType == 1 || customer.kycIDType == 503 || customer.kycIDType == 553) //Passport
                    {
                        Regex regex = new Regex(Constants.Strings.passportFormat);
                        if (!regex.IsMatch(customer.kycIdNo))
                        {
                            response.customerID = "";
                            response.status.code = APIStatus.failed;
                            response.status.message = "Please enter valid passport number";
                            response.status.flag = ProcessStatus.success;
                            return response;
                        }
                    }
                }

                // ---------------------------------------------------------------------------------------------------
                try
                {
                    if (validateUpdateCustomerRequest(customer))
                    {
                        parm_coll[0] = new OracleParameter("custid", OracleDbType.Varchar2, 16);
                        parm_coll[0].Value = customer.custID;
                        parm_coll[0].Direction = ParameterDirection.Input;

                        // fathus in varchar2,
                        parm_coll[1] = new OracleParameter("fathus", OracleDbType.Varchar2, 100);
                        parm_coll[1].Value = customer.fatHusName;
                        parm_coll[1].Direction = ParameterDirection.Input;

                        parm_coll[2] = new OracleParameter("cust_namepar", OracleDbType.Varchar2, 40);
                        parm_coll[2].Value = customer.custName;
                        parm_coll[2].Direction = ParameterDirection.Input;

                        // housenm in varchar2,
                        parm_coll[3] = new OracleParameter("housenm", OracleDbType.Varchar2, 100);
                        parm_coll[3].Value = customer.houseName;
                        parm_coll[3].Direction = ParameterDirection.Input;

                        // loca in varchar2,
                        parm_coll[4] = new OracleParameter("loca", OracleDbType.Varchar2, 40);
                        parm_coll[4].Value = customer.location;
                        parm_coll[4].Direction = ParameterDirection.Input;
                        
                        // pinsrl in number,
                        parm_coll[5] = new OracleParameter("pinsrl", OracleDbType.Long, 15);
                        parm_coll[5].Value = customer.pinsrl;
                        parm_coll[5].Direction = ParameterDirection.Input;

                        // custtype in number,
                        parm_coll[6] = new OracleParameter("custtype", OracleDbType.Long, 15);
                        parm_coll[6].Value = customer.cust_Type;
                        parm_coll[6].Direction = ParameterDirection.Input;

                        // occ_id in number,
                        parm_coll[7] = new OracleParameter("occ_id", OracleDbType.Long, 5);
                        parm_coll[7].Value = customer.occupation;
                        parm_coll[7].Direction = ParameterDirection.Input;

                        // tele in varchar2,
                        parm_coll[8] = new OracleParameter("tele", OracleDbType.Varchar2, 40);
                        parm_coll[8].Value = customer.phoneno;
                        parm_coll[8].Direction = ParameterDirection.Input;

                        // mob in varchar2,
                        parm_coll[9] = new OracleParameter("mob", OracleDbType.Varchar2, 40);
                        parm_coll[9].Value = customer.mobileNo;
                        parm_coll[9].Direction = ParameterDirection.Input;

                        // emailid in varchar2,
                        parm_coll[10] = new OracleParameter("emailid", OracleDbType.Varchar2, 40);
                        if (string.IsNullOrEmpty(customer.loyaltyCardNo))
                            parm_coll[10].Value = "NILL";
                        else
                            parm_coll[10].Value = customer.loyaltyCardNo;

                        parm_coll[10].Direction = ParameterDirection.Input;

                        // id_type in number,
                        parm_coll[11] = new OracleParameter("id_type", OracleDbType.Long, 60);
                        parm_coll[11].Value = customer.kycIDType;
                        parm_coll[11].Direction = ParameterDirection.Input;

                        // id_no in varchar2,
                        parm_coll[12] = new OracleParameter("id_no", OracleDbType.Varchar2, 80);
                        parm_coll[12].Value = customer.kycIdNo;
                        parm_coll[12].Direction = ParameterDirection.InputOutput;

                        // date_of_issue in date,
                        parm_coll[13] = new OracleParameter("date_of_issue", OracleDbType.Date, 22);
                        if (!string.IsNullOrEmpty(customer.kycIssueDate))
                        {                         
                            parm_coll[13].Value = customer.kycIssueDate;
                        }
                        else
                        {
                            parm_coll[13].Value = DBNull.Value;
                        }                     
                        parm_coll[13].Direction = ParameterDirection.Input;

                        //date_of_expiry in date,
                        parm_coll[14] = new OracleParameter("date_of_expiry", OracleDbType.Date, 22);
                        if (!string.IsNullOrEmpty(customer.kycExpiryDate))
                        {
                            parm_coll[14].Value = customer.kycExpiryDate;
                        }
                        else
                        {
                            parm_coll[14].Value = DBNull.Value;
                        }                      
                        parm_coll[14].Direction = ParameterDirection.Input;

                        // place_of_issue in varchar2, 
                        parm_coll[15] = new OracleParameter("place_of_issue", OracleDbType.Varchar2, 40);
                        parm_coll[15].Value = customer.kycIssuePlace;
                        parm_coll[15].Direction = ParameterDirection.Input;

                        // dob in date,
                        parm_coll[16] = new OracleParameter("dob", OracleDbType.Date, 11);
                        if (!string.IsNullOrEmpty(customer.dob))
                        {
                            // parm_coll[16].Value = String.Format(Constants.Strings.dateFormat, Convert.ToDateTime(customer.dob));
                            parm_coll[16].Value = customer.dob;
                        }
                        else
                        {
                            parm_coll[16].Value = "";
                        }
                        parm_coll[16].Direction = ParameterDirection.Input;

                        // descr_modi in varchar2,date_of_issue
                        parm_coll[17] = new OracleParameter("descr_modi", OracleDbType.Varchar2, 60);
                        parm_coll[17].Value = customer.modificationDescription;
                        parm_coll[17].Direction = ParameterDirection.Input;

                        // out_result out varchar2
                        parm_coll[18] = new OracleParameter("out_result", OracleDbType.Varchar2, 100);
                        parm_coll[18].Direction = ParameterDirection.Output;

                        parm_coll[19] = new OracleParameter("update_modi", OracleDbType.Varchar2, 60);

                        //string modi_key = "0!" + customer.fatHus_Flag + "!" + customer.house_Flag + "!" + customer.locality_Flag + "!" +
                        //    customer.pinSerial_Flag + "!" + customer.cust_Status_Flag + "!" + customer.occupation_Flag + "!" +
                        //    customer.phone1_Flag + "!" + customer.phone2_Flag + "!" + customer.loyaltiCardno_Flag + "!" +
                        //    customer.kyc_Type_Flag + "!" + customer.key_Id_Flag + "!" + customer.kyc_Issue_Place_Flag + "!" +
                        //    customer.descr_Flag + "!" +
                        //    "0!" + "0!" + "0!" +
                        //    customer.street_Flag + "!" +
                        //    "0!" + "0!" + "0!" + "0!" + "0!" +
                        //    customer.cust_Name_Flag + "!" +
                        //    "0!" +
                        //    customer.email_Flag;

                        string modi_key = customer.fatHus_Flag + "!" + customer.house_Flag + "!" + customer.locality_Flag + "!" +
                        customer.pinSerial_Flag + "!" + customer.cust_Status_Flag + "!" + customer.occupation_Flag + "!" +
                        customer.phone1_Flag + "!" + customer.phone2_Flag + "!" + customer.loyaltiCardno_Flag + "!" +
                        customer.kyc_Type_Flag + "!" + customer.key_Id_Flag + "!" + customer.kyc_Issue_Place_Flag + "!" + customer.descr_Flag + "!" +
                        "1" + "!" + "1!" + "1!" + "0!" +
                        "1!" + "1!" + "1!" + "1!" + "1!" + customer.cust_Name_Flag + "!"+"1" + "!"+ customer.email_Flag+"!";



                        parm_coll[19].Value = modi_key;
                        parm_coll[19].Direction = ParameterDirection.Input;

                        // street in varchar2,
                        parm_coll[20] = new OracleParameter("p_street", OracleDbType.Varchar2, 40);
                        parm_coll[20].Value = customer.location;
                        parm_coll[20].Direction = ParameterDirection.Input;

                        //p_media_id in Number,
                        parm_coll[21] = new OracleParameter("p_media_id", OracleDbType.Long, 8);
                        parm_coll[21].Value = customer.mediaID;
                        parm_coll[21].Direction = ParameterDirection.Input;

                        // p_module_id in Number,
                        parm_coll[22] = new OracleParameter("p_module_id", OracleDbType.Long, 8);
                        parm_coll[22].Value = customer.moduleID;
                        parm_coll[22].Direction = ParameterDirection.Input;

                        parm_coll[23] = new OracleParameter("ex_status", OracleDbType.Long, 2);
                        parm_coll[23].Value = customer.exServiceStatus;
                        parm_coll[23].Direction = ParameterDirection.Input;

                        parm_coll[24] = new OracleParameter("ex_no", OracleDbType.Varchar2, 40);
                        parm_coll[24].Value = customer.exServiceNo;
                        parm_coll[24].Direction = ParameterDirection.Input;

                        parm_coll[25] = new OracleParameter("Empcode", OracleDbType.Long, 7);
                        parm_coll[25].Value = customer.empCode;
                        parm_coll[25].Direction = ParameterDirection.Input;

                        parm_coll[26] = new OracleParameter("Empname", OracleDbType.Varchar2, 40);
                        parm_coll[26].Value = customer.empName;
                        parm_coll[26].Direction = ParameterDirection.Input;

                        parm_coll[27] = new OracleParameter("typeid", OracleDbType.Long);
                        parm_coll[27].Value = customer.mediaTypeID;
                        parm_coll[27].Direction = ParameterDirection.Input;
                        // ----------------------7022 req
                        parm_coll[28] = new OracleParameter("althouse", OracleDbType.Varchar2, 40);
                        parm_coll[28].Value = customer.alt_Housename;
                        parm_coll[28].Direction = ParameterDirection.Input;

                        parm_coll[29] = new OracleParameter("altlocal", OracleDbType.Varchar2, 40);
                        parm_coll[29].Value = customer.alt_Locality;
                        parm_coll[29].Direction = ParameterDirection.Input;

                        parm_coll[30] = new OracleParameter("altpin", OracleDbType.Long, 7);
                        if (!string.IsNullOrEmpty(customer.alt_Pin))
                            parm_coll[30].Value = customer.alt_Pin;
                        else
                            parm_coll[30].Value = DBNull.Value;
                        // added for KYC chenges

                        parm_coll[31] = new OracleParameter("userid", OracleDbType.Varchar2, 10);
                        parm_coll[31].Value = customer.updating_empCode;
                        parm_coll[31].Direction = ParameterDirection.Input;

                        parm_coll[32] = new OracleParameter("kyc_of", OracleDbType.Long, 2);
                        parm_coll[32].Value = customer.kycOf;
                        parm_coll[32].Direction = ParameterDirection.Input;

                        parm_coll[33] = new OracleParameter("cust_gender", OracleDbType.Long, 2);
                        parm_coll[33].Value = customer.gender;
                        parm_coll[33].Direction = ParameterDirection.Input;

                        parm_coll[34] = new OracleParameter("preflang", OracleDbType.Long, 2);
                        parm_coll[34].Value = customer.prefLang;
                        parm_coll[34].Direction = ParameterDirection.Input;

                        parm_coll[35] = new OracleParameter("pep", OracleDbType.Long, 2);
                        parm_coll[35].Value = customer.pep;
                        parm_coll[35].Direction = ParameterDirection.Input;

                        parm_coll[36] = new OracleParameter("mom_name", OracleDbType.Varchar2, 38);
                        parm_coll[36].Value = customer.mothername;
                        parm_coll[36].Direction = ParameterDirection.Input;

                        parm_coll[37] = new OracleParameter("marital", OracleDbType.Long, 2);
                        parm_coll[37].Value = customer.marital;
                        parm_coll[37].Direction = ParameterDirection.Input;

                        parm_coll[38] = new OracleParameter("p_citizen", OracleDbType.Long, 2);
                        parm_coll[38].Value = customer.citizen;
                        parm_coll[38].Direction = ParameterDirection.Input;

                        parm_coll[39] = new OracleParameter("nation", OracleDbType.Long, 2);
                        parm_coll[39].Value = customer.nationality;
                        parm_coll[39].Direction = ParameterDirection.Input;

                        parm_coll[40] = new OracleParameter("resid", OracleDbType.Long, 2);
                        parm_coll[40].Value = customer.residentialStatus;//////////////////////
                        parm_coll[40].Direction = ParameterDirection.Input;

                        parm_coll[41] = new OracleParameter("prename", OracleDbType.Long, 2);
                        parm_coll[41].Value = customer.preName;
                        parm_coll[41].Direction = ParameterDirection.Input;

                        parm_coll[42] = new OracleParameter("FatHusPre", OracleDbType.Long, 2);
                        parm_coll[42].Value = customer.relationIdentity;
                        parm_coll[42].Direction = ParameterDirection.Input;

                        parm_coll[43] = new OracleParameter("MEmail", OracleDbType.Varchar2, 40);
                        parm_coll[43].Value = customer.email;
                        parm_coll[43].Direction = ParameterDirection.Input;

                        parm_coll[44] = new OracleParameter("Rel", OracleDbType.Long, 2);
                        parm_coll[44].Value = customer.religion;
                        parm_coll[44].Direction = ParameterDirection.Input;

                        parm_coll[45] = new OracleParameter("cas", OracleDbType.Long, 2);
                        parm_coll[45].Value = customer.caste;
                        parm_coll[45].Direction = ParameterDirection.Input;

                        parm_coll[46] = new OracleParameter("EduQual", OracleDbType.Long, 2);
                        parm_coll[46].Value = customer.qualification;
                        parm_coll[46].Direction = ParameterDirection.Input;

                        parm_coll[47] = new OracleParameter("NeedForLoan", OracleDbType.Long, 2);
                        parm_coll[47].Value = customer.loanPurpose;
                        parm_coll[47].Direction = ParameterDirection.Input;

                        parm_coll[48] = new OracleParameter("MIncome", OracleDbType.Long, 2);
                        parm_coll[48].Value = customer.income;
                        parm_coll[48].Direction = ParameterDirection.Input;

                        parm_coll[49] = new OracleParameter("FirstGL", OracleDbType.Long, 2);
                        parm_coll[49].Value = customer.firstGL;
                        parm_coll[49].Direction = ParameterDirection.Input;

                        parm_coll[50] = new OracleParameter("BusiCat", OracleDbType.Long, 2);
                        parm_coll[50].Value = customer.businessCategory;
                        parm_coll[50].Direction = ParameterDirection.Input;

                        parm_coll[51] = new OracleParameter("BusiSubCat", OracleDbType.Long, 2);
                        parm_coll[51].Value = customer.businessSubCategory;
                        parm_coll[51].Direction = ParameterDirection.Input;

                        parm_coll[52] = new OracleParameter("FacebookId", OracleDbType.Varchar2, 100);
                        parm_coll[52].Value = customer.facebookID;  
                        parm_coll[52].Direction = ParameterDirection.Input;

                        //- new parameter added for KYC master directions implementation on 2-mar-2020---Sreerekha K 100006
                        parm_coll[53] = new OracleParameter("AdrPrfTyp", OracleDbType.Long, 2);
                        parm_coll[53].Direction = ParameterDirection.Input;
                        parm_coll[53].Value = customer.addressProof_Type;

                        parm_coll[54] = new OracleParameter("billDate", OracleDbType.Date, 15);
                        if (!string.IsNullOrEmpty(customer.addressProof_Billdate))
                        {
                            parm_coll[54].Value = customer.addressProof_Billdate;
                        }
                        else
                        {
                            parm_coll[54].Value = DBNull.Value;
                        }
                        parm_coll[54].Direction = ParameterDirection.Input;

                        
                        parm_coll[55] = new OracleParameter("AdrPrfFlg", OracleDbType.Long, 2);
                        parm_coll[55].Direction = ParameterDirection.Input;
                        parm_coll[55].Value = customer.addressUpdation_Flag;

                        parm_coll[56] = new OracleParameter("ckyc_id", OracleDbType.Varchar2, 20);
                        parm_coll[56].Direction = ParameterDirection.Input;
                        parm_coll[56].Value = customer.CKYCNumber;


                        //if (parm_coll[17].Value == "0")
                        //            return (0);
                        //        else
                        //            return parm_coll[17].Value;

                        try
                        {
                            if (validMethod)
                            {
                                helper.ExecuteNonQuery("customer_modification", parm_coll);
                                if (parm_coll[18].Value != null)
                                {
                                    if (parm_coll[18].Value.ToString() == "1")//String.IsNullOrEmpty(Convert.ToString(parm_coll[16].Value)))
                                    {
                                        response.customerID = customer.custID;
                                        response.status.code = APIStatus.success;
                                        response.status.message = "Customer has been modified in the Database";
                                        response.status.flag = ProcessStatus.success;
                                        //Wedding Date capture ---16/nov/2020
                                        if (!string.IsNullOrEmpty(customer.weddingDate) && customer.marital==2)
                                        {
                                            DataTable dt = new DataTable();
                                            dt = helper.ExecuteDataSet("select count(*) from TBL_MULTIWAY_LEAD t where t.cust_id='" + customer.custID + "'").Tables[0];
                                            if ((dt != null) && (Convert.ToInt64(dt.Rows[0][0]) > 0))
                                            {
                                                helper.ExecuteNonQuery("update TBL_MULTIWAY_LEAD set MRT_STS ='" + customer.maritalStatus + "', MRG_DT =to_date('" + Convert.ToDateTime(customer.weddingDate).ToShortDateString() + "','MM-dd-yyyy') where CUST_ID = '" + customer.custID + "'");
                                            }
                                            else
                                            {
                                                dt = helper.ExecuteDataSet("select branch_id from customer t where t.cust_id='" + customer.custID + "'").Tables[0];
                                                helper.ExecuteNonQuery("insert into TBL_MULTIWAY_LEAD (CUST_NAME,PHONE_NO,DOB,MRT_STS,MRG_DT,MAIL_ID,FB_ID,CUST_ID,BRANCH_ID,ENTERED_BY,ENTERED_DT) values ('" + customer.custName + "','" + customer.mobileNo + "',to_date('" + Convert.ToDateTime(customer.dob).ToShortDateString() + "','MM-dd-yyyy'),'" + customer.maritalStatus + "',to_date('" + Convert.ToDateTime(customer.weddingDate).ToShortDateString() + "','MM-dd-yyyy'),'" + customer.email + "','" + customer.facebookID + "','" + customer.custID + "'," + Convert.ToString(dt.Rows[0][0]) + ",'" + customer.updating_empCode + "',to_date(sysdate))");
                                            }
                                        }
                                        //Wedding Date capture ---16/nov/2020
                                    }
                                    else
                                    {
                                        response.customerID = "";
                                        response.status.code = APIStatus.failed;
                                        response.status.message = "Customer has not been Modfied in the database. Please inform IT";
                                        response.status.flag = ProcessStatus.success;
                                    }
                                }
                                else
                                {
                                    response.customerID = "";
                                    response.status.code = APIStatus.failed;
                                    response.status.message = "Failed to update the customer details";
                                    response.status.flag = ProcessStatus.success;
                                }
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
                        response.status.flag = ProcessStatus.success;
                        response.status.code = APIStatus.badRequest;
                        response.status.message = invalidParamMessage;
                    }
                }
                catch (Exception ex)
                {
                    response.status.flag = ProcessStatus.failed;
                    response.status.code = APIStatus.exception;
                    response.status.message = "Exception :- " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = "Exception :- " + ex.Message;
            }
            return response;
        }


        /// <Created>Uneesh - 100156</Created>
        /// <summary>Validate Update Customer Request</summary> 
        private bool validateUpdateCustomerRequest(CustomerUpdateRequest customer) // mode=0 for add, mode=1 for update
        {
           //DBAccessHelper helper = new DBAccessHelper();
            bool retVal = true;
            invalidParamMessage = "";
            try
            {
                //if (customer.mediaID == 14 || customer.mediaID == 29)
                //{
                //    if (string.IsNullOrEmpty(customer.baCode))
                //    {
                //        invalidParamMessage = invalidParamMessage + "Please Enter BA/DSA Code or Select Other Media. \r\n";
                //        retVal = false;
                //    }
                //}

                //if (mode == 0)
                //{
                //    if (!customer.customerImageFlag)
                //    {
                //        try
                //        {
                //            string sql;
                //            sql = "select count(*) from pledge_oldconf_mst a,pledge_oldconf_transaction b where a.branch_id= " + customer.branchID +
                //                " and a.request_id=b.request_id and to_date(a.request_dt)=to_date(sysdate) and b.status <> 0";
                //            DataTable dt = helper.ExecuteDataSet(sql).Tables[0];
                //            if (dt.Rows.Count > 0 && Convert.ToInt64(dt.Rows[0][0]) > 0) { }
                //            else
                //            {
                //                invalidParamMessage = invalidParamMessage + "Photo catpturing can be skipped only for entering manual pledge customer. \r\nNo approved back date pledges are available to use this option. \r\n";
                //                retVal = false;
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            return false;
                //        }
                //    }
                //}

                //if (customer.custStatus == 4)
                //{
                //    if (string.IsNullOrEmpty(customer.empCode))
                //    {
                //        invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                //        retVal = false;
                //    }
                //    else
                //    {
                //        if (customer.empCode.Contains("*"))
                //        {
                //            string[] empcodes = customer.empCode.Split("*");
                //            if (string.IsNullOrEmpty(empcodes[1]))
                //            {
                //                invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                //                retVal = false;
                //            }
                //            else
                //            {
                //                invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                //                retVal = false;
                //            }
                //        }
                //        else
                //        {
                //            invalidParamMessage = invalidParamMessage + "Please enter the employee code. \r\n";
                //            retVal = false;
                //        }
                //    }
                //}
                if (customer.email_Flag == 1)
                {
                    if (!string.IsNullOrEmpty(customer.email))
                    {
                        Match rex = Regex.Match(customer.email.Trim(), @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,3})$", RegexOptions.IgnoreCase);
                        if (rex.Success == false)
                        {
                            invalidParamMessage = invalidParamMessage + "Please enter a valid email address. \r\n";
                            retVal = false;
                        }
                        else
                        {
                            DataSet dtmail = new DataSet();
                            string custmail = customer.email.ToUpper();
                            dtmail = helper.ExecuteDataSet("select count(*)  from TBL_CUSTOMER_VALIDATION t where t.email_id ='" + custmail + "'");
                            if (Convert.ToInt64(dtmail.Tables[0].Rows[0][0]) > 0)
                            {
                                invalidParamMessage = invalidParamMessage + "This e-mail address is already existing, update the correct e-mail address. \r\n";
                                retVal = false;
                            }
                        }
                    }
                }

                if (customer.mediaTypeID > 0)
                {
                    if (customer.mediaID <= 0)
                    {
                        invalidParamMessage = invalidParamMessage + "Please select the media. \r\n";
                        retVal = false;
                    }
                }

                // De dupe checking based on Aadhar - 17-oct-2020
                if (customer.key_Id_Flag == 1)
                {
                    if (customer.kycIDType == 505 || customer.kycIDType == 16 || customer.kycIDType == 555)
                    {
                        DataTable dt = new DataTable();
                        string customerdob = Convert.ToDateTime(customer.dob).ToString("dd/MMM/yyyy");
                        dt = helper.ExecuteDataSet("select b.cust_id  from IDENTITY_DTL t,customer b,customer_detail c where  t.identity_id   in (505,555,16)  and substr(t.id_number,9,4)=substr('" + customer.kycIdNo + "',9,4)  and t.cust_id=b.cust_id and b.name = UPPER('" + customer.custName + "') and c.cust_id = b.cust_id  and to_date(c.date_of_birth) = '" + customerdob + "' and c.gender = " + customer.gender + " and b.cust_id <> '" + customer.custID + "'").Tables[0];
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
                            invalidParamMessage = invalidParamMessage + "Customer ID already exists with the same Aadhaar Number.Customer IDs matched are as follows " + CustIDs + " \r\n";
                            retVal = false;
                        }
                    }
                }
                // De dupe checking based on Aadhar - 17-oct-2020

                //if (!string.IsNullOrEmpty(customer.pan))
                //{
                //    if (customer.panNo.Trim().Length == Constants.Ints.panNumberLength)
                //    {
                //        string[] panChars = Constants.Strings.validPanChars.Split(",");
                //        bool validPan = false;
                //        for (int i = 0; i < panChars.Length; i++)
                //        {
                //            if (panChars[i] == customer.panNo.Substring(3, 1))
                //            {
                //                validPan = true;
                //                break;
                //            }
                //        }
                //        if (!validPan)
                //        {
                //            invalidParamMessage = invalidParamMessage + CustomerValidationMessages.Invalid.panNo + " \r\n";
                //            retVal = false;
                //        }
                //    }
                //    else
                //    {
                //        invalidParamMessage = invalidParamMessage + CustomerValidationMessages.Invalid.panNo + " \r\n";
                //        retVal = false;
                //    }
                //}

                //duplicate, age,checkexist are pending

            }
            catch (Exception e)
            {
                invalidParamMessage = invalidParamMessage + e.Message + " \r\n";
                retVal = false;
            }

            return retVal;
        }

        /// <Created>Uneesh - 100156</Created>
        /// <summary>Srm Customer Approval Mail</summary> 
        public void Srm_Customer_ApprovalMail(string cust, int br)
        {
            try
            {
               //DBAccessHelper helper = new DBAccessHelper();
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
                subj = "Customer ID " + cust + " pending for SRM Approval Request";
                mbody = " Hi Sir,<br/><br/> Customer ID " + cust + " from  " + dt.Rows[0][1].ToString() + " branch is pending for SRM approval. <br/><br/> This is a system generated mail please do not reply.";
                //mh.SendMail("Formkrisk@in.manappuram.com", srm_email, "", cc, subj, mbody, "");
                //mh.SendMail("Formkrisk@in.manappuram.com", "srmrisk@in.manappuram.com", "", "", subj, mbody, "");
            }
            catch (Exception ex)
            {
            }
        }

        //private CustomerResponse isValidParameter(CustomerResponse response, string filedName)
        //{

        //    response.status.flag = ProcessStatus.success;
        //    response.status.code = APIStatus.invalidParameterValue;
        //    response.status.message = "invalid " + filedName;
        //    return response;

        //}

        //private CustomerResponse isValidParameter(CustomerResponse response, string filedName, int value)
        //{

        //    response.status.flag = ProcessStatus.success;
        //    response.status.code = APIStatus.invalidParameterValue;
        //    response.status.message = "invalid " + filedName + ":= " + value;
        //    return response;

        //}



        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get Customer Master Data</summary>
        /// <returns><see cref="CustomerMasterDataResponse"/></returns>

        public CustomerMasterDataResponse getMasterDataForCustomer()
        {
            CustomerMasterDataResponse customerMasterDataResponse = new CustomerMasterDataResponse();
            try
            {
               //DBAccessHelper helper = new DBAccessHelper();
                OracleParameter[] parm_coll = new OracleParameter[12];

                parm_coll[0] = new OracleParameter("CUR_CUSTOMER_STATUS", OracleDbType.RefCursor);
                parm_coll[0].Direction = ParameterDirection.Output;
                parm_coll[1] = new OracleParameter("CUR_CUSTOMER_TYPES", OracleDbType.RefCursor);
                parm_coll[1].Direction = ParameterDirection.Output;
                parm_coll[2] = new OracleParameter("CUR_MEDIA_TYPES", OracleDbType.RefCursor);
                parm_coll[2].Direction = ParameterDirection.Output;
                parm_coll[3] = new OracleParameter("CUR_MEDIA_MASTER", OracleDbType.RefCursor);
                parm_coll[3].Direction = ParameterDirection.Output;
                parm_coll[4] = new OracleParameter("CUR_RELIGION", OracleDbType.RefCursor);
                parm_coll[4].Direction = ParameterDirection.Output;
                parm_coll[5] = new OracleParameter("CUR_CASTEMASTER", OracleDbType.RefCursor);
                parm_coll[5].Direction = ParameterDirection.Output;
                parm_coll[6] = new OracleParameter("CUR_PEP", OracleDbType.RefCursor);
                parm_coll[6].Direction = ParameterDirection.Output;
                parm_coll[7] = new OracleParameter("CUR_RELATIONS", OracleDbType.RefCursor);
                parm_coll[7].Direction = ParameterDirection.Output;
                parm_coll[8] = new OracleParameter("CUR_BUSINESS", OracleDbType.RefCursor);
                parm_coll[8].Direction = ParameterDirection.Output;
                parm_coll[9] = new OracleParameter("CUR_STATUS", OracleDbType.RefCursor);
                parm_coll[9].Direction = ParameterDirection.Output;
                parm_coll[10] = new OracleParameter("CUR_PRENAME", OracleDbType.RefCursor);
                parm_coll[10].Direction = ParameterDirection.Output;
                parm_coll[11] = new OracleParameter("CUR_KYCIDTypes", OracleDbType.RefCursor);
                parm_coll[11].Direction = ParameterDirection.Output;

                var ds = helper.ExecuteDataSet("get_customer_master_data", parm_coll);

                customerMasterDataResponse.customerStatusList = globalMethods.ConvertDataTable<CustomerStatusProperties>(ds.Tables[0]);
                customerMasterDataResponse.customerTypesList = globalMethods.ConvertDataTable<CustomerTypeProperties>(ds.Tables[1]);
                customerMasterDataResponse.mediaTypesList = globalMethods.ConvertDataTable<MediaTypeProperties>(ds.Tables[2]);
                customerMasterDataResponse.mediaMasterList = globalMethods.ConvertDataTable<MediaMasterProperties>(ds.Tables[3]);
                customerMasterDataResponse.religionsList = globalMethods.ConvertDataTable<ReligionProperties>(ds.Tables[4]);
                customerMasterDataResponse.casteMasterList = globalMethods.ConvertDataTable<CasteMasterProperties>(ds.Tables[5]);
                customerMasterDataResponse.customerPepsList = globalMethods.ConvertDataTable<CustomerPEPProperties>(ds.Tables[6]);
                customerMasterDataResponse.relationsList = globalMethods.ConvertDataTable<RelationProperties>(ds.Tables[7]);
                customerMasterDataResponse.businessList = globalMethods.ConvertDataTable<OccupationMasterProperties>(ds.Tables[8]);
                var statusMaster = globalMethods.ConvertDataTable<StatusMasterProperties>(ds.Tables[9]);
                customerMasterDataResponse.preNameList = globalMethods.ConvertDataTable<GeneralPreNameProperties>(ds.Tables[10]);
                customerMasterDataResponse.kycIDTypes = globalMethods.ConvertDataTable<GeneralKYCIDTypesProperties>(ds.Tables[11]);

                customerMasterDataResponse.EducationalQualificationList = statusMaster.Where(a => a.MODULE_ID == 896 && a.OPTION_ID == 1).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.incomeList = statusMaster.Where(a => a.MODULE_ID == 896 && a.OPTION_ID == 2).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.STATUS_ID).ToList();
                customerMasterDataResponse.maritalStatus = statusMaster.Where(a => a.MODULE_ID == 892 && a.OPTION_ID == 1).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.citizenships = statusMaster.Where(a => a.MODULE_ID == 893 && a.OPTION_ID == 1).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.nationality = statusMaster.Where(a => a.MODULE_ID == 894 && a.OPTION_ID == 1).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.residentialStatus = statusMaster.Where(a => a.MODULE_ID == 895 && a.OPTION_ID == 1).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.languages = statusMaster.Where(a => a.MODULE_ID == 890 && a.OPTION_ID == 1).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.addressProofs = statusMaster.Where(a => a.MODULE_ID == 1 && a.OPTION_ID == 33).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.LoanReason = statusMaster.Where(a => a.MODULE_ID == 896 && a.OPTION_ID == 3).Select(c => new GeneralStatusProperties { STATUS_ID = c.STATUS_ID, DESCRIPTION = c.DESCRIPTION }).OrderBy(a => a.DESCRIPTION).ToList();
                customerMasterDataResponse.moneyTransferKyc = DapperHelper.GetRecords<GeneralKYCIDTypesProperties>("select identity_id ,identity_name from  identity id where identity_id between 500 and 549 and identity_id not in (500, 504) order by id.identity_name",SQLMode.Query,null);
                customerMasterDataResponse.interimKyc = DapperHelper.GetRecords<GeneralKYCIDTypesProperties>("select identity_id ,identity_name from identity id where identity_id not in(0,1,14,3,2,4,16,15,5,17,18)  and identity_id <100 order by id.identity_name", SQLMode.Query, null);


                customerMasterDataResponse.status.code = APIStatus.success;
                customerMasterDataResponse.status.message = "Success";
                customerMasterDataResponse.status.flag = ProcessStatus.success;
            }
            catch (Exception ex)
            {
                customerMasterDataResponse.status.flag = ProcessStatus.failed;
                customerMasterDataResponse.status.code = APIStatus.exception;
                customerMasterDataResponse.status.message = ex.Message;

            }
            //"11160010122230", 0, 3038, 1



            return customerMasterDataResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get Customer Search Data</summary>
        /// <param name="SearchCustomerRequest"><see cref="SearchCustomerRequest"/></param>
        /// <returns><see cref="SearchCustomerResponse"/></returns>
        public SearchCustomerResponse searchCustomer(SearchCustomerRequest searchCustomerRequest)
        {
            SearchCustomerResponse searchCustomerResponse = new SearchCustomerResponse();
            List<SearchCustomerProperties> searchCustomerList = new List<SearchCustomerProperties>();
            //DBAccessHelper helper = new DBAccessHelper();
            string logdat = "";
            DataSet dt = new DataSet();
            //------------------------------------------------------------------------------------------------------
            // done by Sreerekha 100006 //MAFIL Application Security Test 487 //application does not perform validation for the user input.
            //1-Oct-2019
            string sql = "select count(*)  from employee_master t where  t.emp_code=" + searchCustomerRequest.userId + " and t.status_id =1 and t.branch_id=" + searchCustomerRequest.branchId + "";
            DataTable dt1 = helper.ExecuteDataSet(sql).Tables[0];
            if (Convert.ToInt16(dt1.Rows[0][0]) ==0)
            {
                searchCustomerResponse.status.code = APIStatus.validationFailed ;
                searchCustomerResponse.status.message = "Mismatch in login branch ID & employee's station branch ID";
                searchCustomerResponse.status.flag = ProcessStatus.success;
                return searchCustomerResponse;
            }
                //-------------------------------------------------------------------------------------------------------
                // done by Sreerekha 100006 //MAFIL Application Security Test 487 //application does not perform validation for the user input.
                //12-jun-2019
                if (validateSearchCustomerRequest(searchCustomerRequest))
            {
                if (searchCustomerRequest.moduleId == "7") //  search customer for goldLoan
                {
                    try
                    {
                        //nvl(detail.cust_status,0) = 0
                        if (searchCustomerRequest.type == 1)// CUSTOMER ID
                        {
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no, nvl(c.isactive,1) as statusid,case when nvl( c.isactive,1) =1 then 'Active' else 'Inactive' end as status ,nvl(d.pep_id,0) as pep_id from customer c, post_master p,customer_detail d  where c.cust_id = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id = :branchId and d.cust_id=c.cust_id and  nvl(d.cust_status,0) = 0  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no ,c.isactive statusid,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p ,customer_detail d where c.cust_id = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = :searchValue  and m.branch_id = :branchId  and s.status_id <> 0)  and d.cust_id=c.cust_id and  nvl(d.cust_status,0) = 0 union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no ,c.isactive statusid,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p,customer_detail d  where c.cust_id = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3,5,8)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = :searchValue  and m.branch_id = :branchId  and s.status_id  <> 0) and d.cust_id=c.cust_id and  nvl(d.cust_status,0) = 0";
                        }
                        else if (searchCustomerRequest.type == 2)//CUSTOMER NAME
                        {
                            // logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl(c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p ,customer_detail d where  upper(c.name) like :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id = :branchId and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p,customer_detail d  where upper(c.name) like :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id <> 0) and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0 ";
                            // query modified as per mail from Manikandan... Resolve slowness issue in MAFIL--wef 8-Aug-2020-- done by SReerekha K 100006
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl(c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id  from customer c, post_master p ,customer_detail d where  upper(c.name) like :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id = :branchId and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0 ";
                        }
                        else if (searchCustomerRequest.type == 3)//PAN NUMBER
                        {
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1) =1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p, customer_detail d  where d.cust_id = c.cust_id  and upper(d.pan) = upper(:searchValue)  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.pin_serial = p.sr_number  and c.branch_id = :branchId and nvl(d.cust_status,0) = 0 union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p, customer_detail d  where upper(d.pan) = upper(:searchValue)  and d.cust_id = c.cust_id  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id <> 0) and nvl(d.cust_status,0) = 0 union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,    substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status ,nvl(d.pep_id,0) as pep_id from customer c, post_master p, customer_detail d  where upper(d.pan) = upper(:searchValue)  and d.cust_id = c.cust_id  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3,5,8)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId and s.status_id <> 0) and nvl(d.cust_status,0) = 0";
                        }
                        else if (searchCustomerRequest.type == 4)//PHONE NUMBER
                        {
                            //logdat = "select c.cust_id,c.name,c.fat_hus,c.house_name, c.locality, p.post_office,to_char(p.pin_code ) as pin_code,c.Phone1, c.Phone2, c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id  from customer c, post_master p, customer_detail d where d.cust_id = c.cust_id  and (((c.phone1 is not null) and c.phone1 = :searchValue) or  ((c.phone2 is not null) and c.phone2 = :searchValue))  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.pin_serial = p.sr_number  and c.branch_id = :branchId and nvl(d.cust_status,0) = 0 union select c.cust_id,  c.name, c.fat_hus,  c.house_name, c.locality, p.post_office, to_char(p.pin_code ) as pin_code,c.Phone1, c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p,customer_detail d where (((c.phone1 is not null) and c.phone1 = (:searchValue)) or ((c.phone2 is not null) and c.phone2 = (:searchValue)))  and c.pin_serial = p.sr_number   and nvl(c.isactive, 0) not in (2, 3,5,8)   and c.branch_id <> :branchId   and (c.cust_id) in (select m.cust_id   from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no       and m.cust_id = c.cust_id                and m.branch_id = :branchId                  and s.status_id <> 0) and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0  union select c.cust_id,   c.name,   c.fat_hus,      c.house_name,       c.locality,       p.post_office,      to_char(p.pin_code ) as pin_code,       substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,       substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2, c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p ,customer_detail d where (((c.phone1 is not null) and c.phone1 = (:searchValue)) or       ((c.phone2 is not null) and c.phone2 = (:searchValue)))   and c.pin_serial = p.sr_number   and nvl(c.isactive, 0) not in (2, 3,5,8)   and c.branch_id <> :branchId   and (c.cust_id) not in       (select m.cust_id          from pledge_master_igl m, pledge_status_igl s, customer t         where t.cust_id = m.cust_id           and nvl(t.isactive, 0) not in (2, 3,5,8)           and t.branch_id <> :branchId           and m.pledge_no = s.pledge_no           and m.cust_id = c.cust_id           and m.branch_id = :branchId           and s.status_id <> 0) and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0";

                            //New code added for query optimization--suggested by Orace team-on 15-july-2023     100654/Rahul---------------------------
                            logdat= "select c.cust_id, c.cust_name as name, c.fat_hus, c.house_name, c.locality, p.post_office, to_char(p.pin_code) as pin_code, c.Phone2 as phone1, c.Phone1 as phone2, '' as share_no, nvl(c.isactive, 1) as statusId, case when nvl(c.isactive, 1) = 1 then 'Active' else 'Inactive' end as status, nvl(d.pep_id, 0) as pep_id from aml_customer.tbl_customer c, aml_users.tbl_post_master p, .customer_detail d where d.cust_id = c.cust_id and(((c.phone1 is not null) and c.phone1 = :searchValue) or((c.phone2 is not null) and c.phone2 = :searchValue)) and nvl(c.isactive, 0) not in (2, 3, 5, 8) and c.pin_serial = p.sr_number and c.branch_id = :branchId and nvl(d.cust_status, 0) = 0 union select c.cust_id, c.cust_name as name, c.fat_hus, c.house_name, c.locality, p.post_office, to_char(p.pin_code) as pin_code, c.Phone2 as phone1, c.Phone1 as phone2, '' as share_no, c.isactive statusId, case when c.isactive = 1 then 'Active' else 'Inactive' end as status, nvl(d.pep_id, 0) as pep_id from aml_customer.tbl_customer c, aml_users.tbl_post_master p, .customer_detail d where(((c.phone1 is not null) and c.phone1 = (: searchValue)) or((c.phone2 is not null) and c.phone2 = (: searchValue))) and c.pin_serial = p.sr_number and nvl(c.isactive, 0) not in (2, 3, 5, 8) and c.branch_id<> :branchId and exists(select m.cust_id from pledge_master_igl m, pledge_status_igl s where m.cust_id = c.cust_id and m.pledge_no = s.pledge_no and m.cust_id = c.cust_id and m.branch_id = :branchId and s.status_id <> 0) and d.cust_id = c.cust_id and nvl(d.cust_status, 0) = 0 union select c.cust_id, c.cust_name as name, c.fat_hus, c.house_name, c.locality, p.post_office, to_char(p.pin_code) as pin_code, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone2, '' as share_no, c.isactive statusId, case when c.isactive = 1 then 'Active' else 'Inactive' end as status, nvl(d.pep_id, 0) as pep_id from aml_customer.tbl_customer c, aml_users.tbl_post_master p, .customer_detail d where(((c.phone1 is not null) and c.phone1 = (: searchValue)) or((c.phone2 is not null) and c.phone2 = (: searchValue))) and c.pin_serial = p.sr_number and nvl(c.isactive, 0) not in (2, 3, 5, 8) and c.branch_id<> :branchId and not exists(select m.cust_id from pledge_master_igl m, pledge_status_igl s, customer t where m.cust_id = c.cust_id and t.cust_id = m.cust_id and nvl(t.isactive, 0) not in (2, 3, 5, 8) and t.branch_id<> :branchId and m.pledge_no = s.pledge_no and m.cust_id = c.cust_id and m.branch_id = :branchId and s.status_id <> 0) and d.cust_id = c.cust_id and nvl(d.cust_status, 0) = 0";
                        }
                        else if (searchCustomerRequest.type == 5)//ID NUMBER
                        {
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id  from customer c, post_master p, identity_dtl d, customer_detail de where c.cust_id = d.cust_id  and d.id_number is not null  and upper(d.id_number) = upper(:searchValue)  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.pin_serial = p.sr_number  and c.branch_id = :branchId and de.cust_id=c.cust_id and nvl(de.cust_status,0) = 0 union   select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p, identity_dtl d,customer_detail de  where c.cust_id = d.cust_id  and upper(d.id_number) = upper(:searchValue)  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id <> 0) and de.cust_id=c.cust_id and nvl(de.cust_status,0) = 0  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,   substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2, c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id from customer c, post_master p, identity_dtl d ,customer_detail de where c.cust_id = d.cust_id  and upper(d.id_number) = upper(:searchValue)  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3,5,8)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id  <> 0) and de.cust_id=c.cust_id and nvl(de.cust_status,0) = 0";
                        }
                        else if (searchCustomerRequest.type == 6)//CARD NUMBER
                        {
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1, c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status,nvl(d.pep_id,0) as pep_id  from customer c, post_master p,customer_detail d  where c.card_no = :searchValue  and c.pin_serial = p.sr_number  and c.branch_id = :branchId and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status ,nvl(d.pep_id,0) as pep_id from customer c, post_master p,customer_detail d  where c.card_no = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id <> 0) and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0 union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,   substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status ,nvl(d.pep_id,0) as pep_id from customer c, post_master p,customer_detail d  where c.card_no = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3,5,8)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3,5,8)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id  <> 0) and d.cust_id=c.cust_id and nvl(d.cust_status,0) = 0";
                        }

                        DynamicParameters dynamicParameters = new DynamicParameters();

                        dynamicParameters.Add("branchId", searchCustomerRequest.branchId );
                        if (searchCustomerRequest.type == 2)
                        {
                            // dynamicParameters.Add("searchValue", "%" + searchCustomerRequest.searchValue.ToUpper() + "%");
                            //modified as per VAPT suggestions . done by Sreerekha K 100006 , on 1-oct-2019
                            dynamicParameters.Add("searchValue", searchCustomerRequest.searchValue.ToUpper() + "%");
                        }
                        else
                        {
                            dynamicParameters.Add("searchValue", searchCustomerRequest.searchValue);
                        }
                        searchCustomerList = DapperHelper.GetRecords<SearchCustomerProperties>(logdat,SQLMode.Query, dynamicParameters);
                        if (searchCustomerList.Count > 0)
                        {
                            searchCustomerResponse.searchCustomerList = searchCustomerList.OrderBy(a => a.NAME).ToList();
                            searchCustomerResponse.status.code = APIStatus.success;
                            searchCustomerResponse.status.message = "Success";
                            searchCustomerResponse.status.flag = ProcessStatus.success;
                            customerSearchLog(searchCustomerRequest);
                        }
                        else
                        {
                            searchCustomerResponse.status.code = APIStatus.no_Data_Found;
                            searchCustomerResponse.status.message = "No Data Found";
                            searchCustomerResponse.status.flag = ProcessStatus.success;
                        }


                    }
                    catch (Exception ex)
                    {
                        searchCustomerResponse.status.flag = ProcessStatus.failed;
                        searchCustomerResponse.status.code = APIStatus.exception;
                        searchCustomerResponse.status.message = ex.Message;
                    }

                }
                else // Normal search customer 
                {

                    try
                    {

                        if (searchCustomerRequest.type == 1)//CUSTOMER ID
                        {
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no, nvl( c.isactive,1) as statusid,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p  where c.cust_id = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id = " + searchCustomerRequest.branchId + "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no, c.isactive statusid,case when c.isactive=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p  where c.cust_id = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = :searchValue  and m.branch_id =  :branchId and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no, c.isactive statusid,case when c.isactive=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p  where c.cust_id = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <>  :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = :searchValue  and m.branch_id =  :branchId  and s.status_id  <> 0)";
                        }
                        else if (searchCustomerRequest.type == 2)//CUSTOMER NAME
                        {
                            //logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p  where  upper(c.name) like :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id = " + searchCustomerRequest.branchId + "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status   from customer c, post_master p  where upper(c.name) like :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id =  :branchId  and s.status_id <> 0)";
                            // query modified as per mail from Manikandan... Resolve slowness issue in MAFIL--wef 8-Aug-2020-- done by SReerekha K 100006
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p  where  upper(c.name) like :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id = " + searchCustomerRequest.branchId + "";
                        }
                        else if (searchCustomerRequest.type == 3)//PAN NUMBER
                        {
                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status from customer c, post_master p, customer_detail d  where d.cust_id = c.cust_id  and upper(d.pan) = upper(:searchValue)  and nvl(c.isactive, 0) not in (2, 3)  and c.pin_serial = p.sr_number  and c.branch_id = " + searchCustomerRequest.branchId + "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p, customer_detail d  where upper(d.pan) = upper(:searchValue)  and d.cust_id = c.cust_id  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId  and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id =  :branchId  and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,    substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p, customer_detail d  where upper(d.pan) = upper(:searchValue)  and d.cust_id = c.cust_id  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId and s.status_id <> 0)";
                        }
                        else if (searchCustomerRequest.type == 4)//PHONE NUMBER
                        {
                            logdat = "select c.cust_id,c.name,c.fat_hus,c.house_name, c.locality, p.post_office,to_char(p.pin_code ) as pin_code,c.Phone1, c.Phone2, c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p, customer_detail d where d.cust_id = c.cust_id  and (((c.phone1 is not null) and c.phone1 = :searchValue) or  ((c.phone2 is not null) and c.phone2 = :searchValue))  and nvl(c.isactive, 0) not in (2, 3)  and c.pin_serial = p.sr_number  and c.branch_id = :branchId union select c.cust_id,  c.name, c.fat_hus,  c.house_name, c.locality, p.post_office, to_char(p.pin_code ) as pin_code,c.Phone1, c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status from customer c, post_master p where (((c.phone1 is not null) and c.phone1 = (:searchValue)) or ((c.phone2 is not null) and c.phone2 = (:searchValue)))  and c.pin_serial = p.sr_number   and nvl(c.isactive, 0) not in (2, 3)   and c.branch_id <> :branchId   and (c.cust_id) in (select m.cust_id   from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no       and m.cust_id = c.cust_id                and m.branch_id = :branchId  and s.status_id <> 0) union select c.cust_id,   c.name,   c.fat_hus,      c.house_name,       c.locality,       p.post_office,      to_char(p.pin_code ) as pin_code,       substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,       substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,       c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p where (((c.phone1 is not null) and c.phone1 = (:searchValue)) or       ((c.phone2 is not null) and c.phone2 = (:searchValue)))   and c.pin_serial = p.sr_number   and nvl(c.isactive, 0) not in (2, 3)   and c.branch_id <>  :branchId   and (c.cust_id) not in       (select m.cust_id          from pledge_master_igl m, pledge_status_igl s, customer t         where t.cust_id = m.cust_id           and nvl(t.isactive, 0) not in (2, 3)           and t.branch_id <>  :branchId  and m.pledge_no = s.pledge_no           and m.cust_id = c.cust_id   and m.branch_id =  :branchId  and s.status_id <> 0) ";
                        }
                        else if (searchCustomerRequest.type == 5)//ID NUMBER
                        {

                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case when nvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status from customer c, post_master p, identity_dtl d  where c.cust_id = d.cust_id  and d.id_number is not null  and upper(d.id_number) = upper(:searchValue)  and nvl(c.isactive, 0) not in (2, 3)  and c.pin_serial = p.sr_number  and c.branch_id = :branchId  union   select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status from customer c, post_master p, identity_dtl d  where c.cust_id = d.cust_id  and upper(d.id_number) = upper(:searchValue)  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId  and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,   substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2, c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status from customer c, post_master p, identity_dtl d  where c.cust_id = d.cust_id  and upper(d.id_number) = upper(:searchValue)  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id =  :branchId  and s.status_id  <> 0)";
                        }
                        else if (searchCustomerRequest.type == 6)//CARD NUMBER
                        {

                            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1, c.Phone2,  c.share_no  , nvl( c.isactive,1) as statusId,case whennvl( c.isactive,1)=1 then 'Active' else 'Inactive' end as status  from customer c, post_master p  where c.card_no = :searchValue  and c.pin_serial = p.sr_number  and c.branch_id = " + searchCustomerRequest.branchId + "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,  c.Phone1,  c.Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status from customer c, post_master p  where c.card_no = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId and (c.cust_id) in (select m.cust_id  from pledge_master_igl m, pledge_status_igl s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id =  :branchId and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office, to_char(p.pin_code ) as pin_code,   substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' || substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1, substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||       substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  , c.isactive statusId,case when c.isactive=1 then 'Active' else 'Inactive' end as status from customer c, post_master p  where c.card_no = :searchValue  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> :branchId  and (c.cust_id) not in  (select m.cust_id  from pledge_master_igl m, pledge_status_igl s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <>  :branchId  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = :branchId and s.status_id  <> 0)";
                        }


                        DynamicParameters dynamicParameters = new DynamicParameters();

                        dynamicParameters.Add("branchId", searchCustomerRequest.branchId);
                        if (searchCustomerRequest.type == 2)
                        {
                            //dynamicParameters.Add("searchValue", "%" + searchCustomerRequest.searchValue.ToUpper() + "%");
                            //modified as per VAPT suggestions . done by Sreerekha K 100006 , on 1-oct-2019
                            dynamicParameters.Add("searchValue", searchCustomerRequest.searchValue.ToUpper() + "%");
                        }
                        else
                        {
                            dynamicParameters.Add("searchValue", searchCustomerRequest.searchValue);
                        }
                        searchCustomerList = DapperHelper.GetRecords<SearchCustomerProperties>(logdat,SQLMode.Query, dynamicParameters);
                        if (searchCustomerList.Count > 0)
                        {
                            searchCustomerResponse.searchCustomerList = searchCustomerList.OrderBy(a=>a.NAME).ToList();
                            searchCustomerResponse.status.code = APIStatus.success;
                            searchCustomerResponse.status.message = "Success";
                            searchCustomerResponse.status.flag = ProcessStatus.success;
                            customerSearchLog(searchCustomerRequest);
                        }
                        else
                        {
                            searchCustomerResponse.status.code = APIStatus.no_Data_Found;
                            searchCustomerResponse.status.message = "No Data Found";
                            searchCustomerResponse.status.flag = ProcessStatus.success;
                        }


                    }
                    catch (Exception ex)
                    {
                        searchCustomerResponse.status.flag = ProcessStatus.failed;
                        searchCustomerResponse.status.code = APIStatus.exception;
                        searchCustomerResponse.status.message = ex.Message;
                    }
                }
            }
            else
            {
                searchCustomerResponse.status.flag = ProcessStatus.failed;
                searchCustomerResponse.status.code = APIStatus.badRequest ;
                searchCustomerResponse.status.message = "Invalid Request";
            }

            return searchCustomerResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Post Customer Search Data Log</summary>
        /// <param name="SearchCustomerRequest"><see cref="SearchCustomerRequest"/></param>
        public void customerSearchLog(SearchCustomerRequest searchCustomerRequest)
        {
            try
            {
               //DBAccessHelper helper = new DBAccessHelper();
                OracleParameter[] parm_coll = new OracleParameter[4];
                parm_coll[0] = new OracleParameter("BrId", OracleDbType.Int64);
                parm_coll[0].Value = searchCustomerRequest.branchId;
                parm_coll[0].Direction = ParameterDirection.Input;
                parm_coll[1] = new OracleParameter("UserId", OracleDbType.Int64);
                parm_coll[1].Value = searchCustomerRequest.userId;
                parm_coll[1].Direction = ParameterDirection.Input;
                parm_coll[2] = new OracleParameter("Typ", OracleDbType.Int64);
                parm_coll[2].Value = searchCustomerRequest.type;
                parm_coll[2].Direction = ParameterDirection.Input;
                parm_coll[3] = new OracleParameter("SerVal", OracleDbType.Varchar2, 150);
                parm_coll[3].Value = searchCustomerRequest.searchValue;
                parm_coll[3].Direction = ParameterDirection.Input;
                helper.ExecuteNonQuery("proc_cust_search_log", parm_coll);

            }
            catch (Exception ex)
            {

            }

        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get DsaBa Details</summary>
        /// <param name="DsaBaRequest"><see cref="DsaBaRequest"/></param>
        /// <returns><see cref="DsaBaResponse"/></returns>

        public DsaBaResponse getDsaBa(DsaBaRequest dsaBaRequest)
        {
            DsaBaResponse dsaBaResponse = new DsaBaResponse();
            try
            {
               //DBAccessHelper helper = new DBAccessHelper();
                string sqlDSA = "";
                DataTable dt = new DataTable();

                if (dsaBaRequest.mediaId == 14)
                {

                    sqlDSA = "select TO_CHAR(t.brid) as BRID,BM.BRANCH_NAME, TO_CHAR(t.ba_code) as BA_CODE, t.ba_name as BA_NAME, TO_CHAR(t.address) as ADDRESS, TO_CHAR(t.phone) as PHONE, '' as EMAIL from TBL_ADD_BUSINESSAGENT t inner join Branch_master BM on T.BRID=Bm.Branch_Id where t.ba_code =" + dsaBaRequest.code + " and t.status = 1 ";
                    DsaBaResponse dsaBaRe = DapperHelper.GetRecord<DsaBaResponse>(sqlDSA,SQLMode.Query,null);
                    if (dsaBaRe != null)
                    {
                        dsaBaResponse = dsaBaRe;
                        dsaBaResponse.status.code = APIStatus.success;
                        dsaBaResponse.status.message = "Success";
                        dsaBaResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        dsaBaResponse.status.code = APIStatus.no_Data_Found;
                        dsaBaResponse.status.message = "No Data Found";
                        dsaBaResponse.status.flag = ProcessStatus.success;
                    }

                }
                else if (dsaBaRequest.mediaId == 29)
                {

                    sqlDSA = "select TO_CHAR(t.brid) as BRID,BM.BRANCH_NAME,TO_CHAR(t.dsa_code) as BA_CODE,t.dsa_name as BA_NAME,t.address as ADDRESS,t.mobile as PHONE,t.email as EMAIL from TBL_DSA_MASTER t inner join Branch_master BM on T.BRID=Bm.Branch_Id Where t.dsa_code = " + dsaBaRequest.code + " and t.status=1";
                    DsaBaResponse dsaBaRe = DapperHelper.GetRecord<DsaBaResponse>(sqlDSA,SQLMode.Query,null);
                    if (dsaBaRe != null)
                    {
                        dsaBaResponse = globalMethods.ConvertClass<DsaBaResponse>(helper.ExecuteDataSet(sqlDSA).Tables[0]);
                        dsaBaResponse.status.code = APIStatus.success;
                        dsaBaResponse.status.message = "Success";
                        dsaBaResponse.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        dsaBaResponse.status.code = APIStatus.no_Data_Found;
                        dsaBaResponse.status.message = "No Data Found";
                        dsaBaResponse.status.flag = ProcessStatus.success;
                    }

                }
                else
                {

                    dsaBaResponse.status.code = APIStatus.no_Data_Found;
                    dsaBaResponse.status.message = "No Data Found";
                    dsaBaResponse.status.flag = ProcessStatus.success;

                }


            }
            catch (Exception ex)
            {

                dsaBaResponse.status.flag = ProcessStatus.failed;
                dsaBaResponse.status.code = APIStatus.exception;
                dsaBaResponse.status.message = ex.Message;
            }

            return dsaBaResponse;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get Customer Details</summary>
        /// <param name="CustomerDataRequest"><see cref="CustomerDataRequest"/></param>
        /// <returns><see cref="CustomerDataResponse"/></returns>
        public CustomerDataResponse getCustomer(CustomerDataRequest customerDataRequest)
        {
            CustomerDataResponse customerDataResponse = new CustomerDataResponse();
            try
            {
                string query;
               //DBAccessHelper helper = new DBAccessHelper();
                query = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  TO_CHAR(p.pin_code) as pin_code,  " ;
                query += " c.Phone2 as Phone2, c.Phone1 as Phone1,  d.district_name,  s.state_name,  ";
                query += "  cd.country_name,  TO_CHAR(im.identity_id) as identity_id,  im.identity_name,  TO_CHAR(id.id_number) as id_number,  c.share_no,  ";
                query += "  TO_CHAR(id.address_proof) as address_proof,  TO_CHAR(c.branch_id) as branch_id,  TO_CHAR(c.firm_id) as firm_id,  TO_CHAR(c.isactive) as isactive,";
                query += "  ct.mother_name,   p.sr_number,  d.district_id,  s.state_id,  c.name_pre,  c.card_no,  c.Pref_lang,  ct.occupation_id,  ct.date_of_birth, ";
                query += "  ct.cust_type cust_status,  ct.email_id,  ct.gender,  ct.citizen,  c.country_id,  ct.reg_date,  ct.pan,  ct.emp_code,  ct.land_dtls,  ct.religion, ";
                query += "  ct.caste,  ct.purposeofloan,  ct.cust_status cust_category, ct.cust_category cust_type,  ct.pep_id,    ct.citizenship,  ct.nationality,  ct.resident,";
                query += "  ct.marital_stat,  ct.FATHUS_PRE,  ct.addr_flg,  ct.EDU_QUAL,  ct.NEED_FOR_LOAN,  ct.INCOME,  ct.FIRST_GL,   to_char(id.issue_dt,'mm/dd/yyyy') as issue_dt,  to_char(id.exp_dt,'mm/dd/yyyy') as exp_dt,  id.issue_plce,";
                query += "  (select (case when t.identity_id in (select identity_id  from identity id  where identity_id between 500 and 549   and identity_id not in (500, 504) ) then  '3'   when t.identity_id in (select identity_id  from identity id   where identity_id not in   (0, 1, 14, 3, 2, 4, 16, 15, 5, 17, 18)   and identity_id < 100 ) then  '2'  else '1'  end) kycidtype  from identity_dtl t WHERE T.CUST_ID = '" + customerDataRequest.custId +"') as kycidtype ,";
                query += "  id.exservice_status,  id.pension_order,  id.kycof,md.media_id , md.type_id mediaType, c.alt_house_name,c.alt_locality,to_char( c.alt_pin_serial ) as alt_pin_serial,ct.facebook_id as facebookID,to_char(tv.bill_date,'mm/dd/yyyy')  as addressProofBilldate ,nvl(tv.addr_typ,0) addressProofType ,nvl(tb.category_id,0) businessCategory ,nvl(ts.sub_category_id,0) businessSubCategory ,   nvl(decode(tr.risk_type, 1, 'High', 2, 'Medium', 3, 'Low'), 'Low') as riskCategory ,nvl(tcm.ckyc_id,0) as CKYCNumber,to_char(tml.mrg_dt, 'mm/dd/yyyy') as weddingDate ,0 as PeriodicUpdation from customer        c,  post_master     p,  district_master d,  state_master    s,  ";
                query += "  country_dtl     cd,  identity im,  identity_dtl    id,  customer_detail ct,brand_awareness md,tbl_cust_addr_verify tv  ,tbl_occupation_category tb ,tbl_occupation_sub_category ts,TBL_CUST_RISK_CLASSIFICATION tr ,tbl_customer_master tcm ,TBL_MULTIWAY_LEAD tml where c.cust_id =  '" + customerDataRequest.custId + "' and c.cust_id=ct.cust_id(+) ";
                query += "  and c.pin_serial = p.sr_number  and p.district_id = d.district_id  and d.state_id = s.state_id  and cd.country_id = s.country_id ";
                query += "  and c.cust_id = id.cust_id(+)  and im.identity_id(+) = id.identity_id and  c.cust_id = md.customer_id(+) and  c.cust_id = tv.cust_id(+)  and ct.business_category =tb.category_id(+) and ct.business_sub_category=ts.sub_category_id(+) and  c.cust_id = tr.cust_id(+) and  c.cust_id = tcm.cust_id(+) and c.cust_id = tml.cust_id(+)";
                // var query = "select c.cust_id,c.name,c.fat_hus,c.house_name,c.locality,p.post_office,TO_CHAR(p.pin_code) as pin_code,substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,d.district_name,s.state_name,cd.country_name,TO_CHAR(im.identity_id) as identity_id,im.identity_name,TO_CHAR(id.id_number) as id_number ,c.share_no,TO_CHAR(id.address_proof) as address_proof,TO_CHAR(c.branch_id) as branch_id,TO_CHAR(c.firm_id) as firm_id,TO_CHAR(c.isactive) as isactive from customer c,post_master p,district_master d,state_master s,country_dtl cd,identity im,identity_dtl id where c.cust_id = '" + customerDataRequest.custId + "' and c.pin_serial = p.sr_number and p.district_id=d.district_id and d.state_id = s.state_id and cd.country_id = s.country_id and  c.cust_id = id.cust_id(+) and im.identity_id(+) = id.identity_id";
                // query += " substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2, substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,  d.district_name,  s.state_name,  ";
                // ct.cust_status cust_type,  ct.cust_category
                // ct.cust_status cust_category, ct.cust_category cust_type,


                string refquery = "select RELTYPE,REFNAME,MOB from TBL_CUST_REF_DTL where CUST_ID='" + customerDataRequest .custId + "'";
                var referenceList = DapperHelper.GetRecords<CustomerReferenceProperties>(refquery, SQLMode.Query, null);

                CustomerDataResponse customer = DapperHelper.GetRecord<CustomerDataResponse>(query,SQLMode.Query,null);
                if (customer != null && customer.CUST_ID != null)
                {
                    customerDataResponse = customer;
                    if(referenceList!=null)
                    {
                        customerDataResponse.customerReferences = referenceList;
                    }
                    customerDataResponse.status.code = APIStatus.success;
                    customerDataResponse.status.message = "Success";
                    customerDataResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    customerDataResponse.status.code = APIStatus.no_Data_Found;
                    customerDataResponse.status.message = "No Data Found";
                    customerDataResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception ex)
            {

                customerDataResponse.status.flag = ProcessStatus.failed;
                customerDataResponse.status.code = APIStatus.exception;
                customerDataResponse.status.message = ex.Message;
            }

            return customerDataResponse;
        }


        /// <Created>Uneesh - 100156</Created>
        /// <summary>Check is Duplicate Customer</summary> 
        public DuplicateSearchCustomerResponse isDuplicateCustomer(DuplicateSearchCustomerRequest request)
        {
            DuplicateSearchCustomerResponse response = new DuplicateSearchCustomerResponse();
           //DBAccessHelper helper = new DBAccessHelper();
            Int32 duplicateVal;
            try
            {
                DataSet DsKycInfo = new DataSet();
                OracleParameter[] arParms = new OracleParameter[6];
                arParms[0] = new OracleParameter("CustID", OracleDbType.Varchar2, 1500);
                arParms[0].Value = request.custID;
                arParms[1] = new OracleParameter("KycNum", OracleDbType.Varchar2, 1500);
                arParms[1].Value = request.kycNumber;
                arParms[2] = new OracleParameter("KycType", OracleDbType.Int64);
                arParms[2].Value = request.kycType;
                arParms[3] = new OracleParameter("PinSer", OracleDbType.Int64);
                arParms[3].Value = request.pinSerial;
                arParms[4] = new OracleParameter("Custtype", OracleDbType.Int64);
                arParms[4].Value = request.custype;
                arParms[5] = new OracleParameter("as_outresult", OracleDbType.RefCursor);
                arParms[5].Direction = ParameterDirection.Output;
                DsKycInfo = helper.ExecuteDataSet("sp_checkforduplicatekyc", arParms);

                if (DsKycInfo.Tables.Count > 0 && DsKycInfo.Tables[0].Rows.Count > 0)
                {
                    duplicateVal = Convert.ToInt32(DsKycInfo.Tables[0].Rows[0]["IsDuplicateID"]);
                    if (duplicateVal == 1)
                        response.isDuplicate = false;
                    else
                        response.isDuplicate = true;
                }
                else
                { response.isDuplicate = false; }
            }
            catch (Exception ex)
            {
                response.isDuplicate = false;
            }

            return response;
        }


        #region AddCustometReferenceDetails


        /// <Created>Vidhya - 10010</Created>
        /// <summary>Add Customer Reference Details</summary> 

        public CustomerReferenceDetailsResponse AddCustometReferenceDetails(CustomerReferenceDetailsRequest request)
        {
            CustomerReferenceDetailsResponse response = new CustomerReferenceDetailsResponse();

            try
            {

                string strDetails = string.Empty;
                string strRefDetails = string.Empty;
                string strMessage = string.Empty;
                if (request.refDetails != null && request.refDetails.Count > 0)
                {
                    int i = 0;
                    for (i = 0; i <= request.refDetails.Count - 1; i++)
                    {
                        strDetails = request.refDetails[i].relationType + "*" + request.refDetails[i].referenceName + "*" + request.refDetails[i].referenceMobile;
                        if (strDetails.Length > 0)
                        {
                            if (strRefDetails.Length == 0)
                                strRefDetails = strDetails + "~";
                            else
                                strRefDetails = strRefDetails + strDetails + "~";
                        }
                    }
                    strMessage = SaveRefDetails("REFDETAILS", request.custId, request.empCode, "", "", strRefDetails.TrimEnd('~'));
                    if (strMessage.Length > 0)
                    {

                        response.status.code = APIStatus.success;
                        response.status.message = "COMPLETED";
                        response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        response.status.code = APIStatus.failed;
                        response.status.message = "Failed";
                        response.status.flag = ProcessStatus.success;
                    }
                }

                if (request.strMobile != "0" && request.strOTP!="0")
                {
                    strMessage = SaveRefDetails("MOBDETAILS", request.custId, request.empCode, request.strMobile, request.strOTP, "");
                    if (strMessage.Length > 0)
                    {

                        response.status.code = APIStatus.success;
                        response.status.message = "Success";
                        response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        response.status.code = APIStatus.failed;
                        response.status.message = "Failed";
                        response.status.flag = ProcessStatus.success;
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


        /// <Created>Vidhya - 10010</Created>
        /// <summary>Save Ref Details</summary> 
        private string SaveRefDetails(String StrFlag , String custID , String empcode , String strMob  , String strOTP , String RefDetails )
        {
            string result = string.Empty;
           //DBAccessHelper helper = new DBAccessHelper();
            OracleParameter[] parameters = new OracleParameter[7];
            DynamicParameters parm_coll = new DynamicParameters();

            parm_coll.Add("Flag", StrFlag, DbType.String, ParameterDirection.Input);
            parm_coll.Add("Customer_Id", custID, DbType.String, ParameterDirection.Input);
            parm_coll.Add("EmpCode", empcode, DbType.String, ParameterDirection.Input);
            parm_coll.Add("Mobile", strMob, DbType.String, ParameterDirection.Input);
            parm_coll.Add("OTPVAL", strOTP, DbType.String, ParameterDirection.Input);
            parm_coll.Add("Ref_Details", RefDetails, DbType.String, ParameterDirection.Input);
            parm_coll.Add("OutMessage", null, DbType.String, ParameterDirection.Output,1000);
            DapperHelper.ExecuteNonQuery("SP_SaveCustRefDetails", SQLMode.StoredProcedure, parm_coll);
            if (parm_coll.Get<string>("OutMessage") != null)
            {
                result = parm_coll.Get<string>("OutMessage");
            }

                return result;
        }


        #endregion


        #region MatchingCustomerList

        /// <Created>Vidhya - 10010</Created>
        /// <summary>Matching Customer List</summary> 
        public matchingCustomerResponse MatchingCustomerList(matchingCustomerRequest request)
        {
            matchingCustomerResponse response = new matchingCustomerResponse();
          
        
            try
            {
                DataSet DsKycInfo = new DataSet();
                OracleParameter[] arParms = new OracleParameter[12];
                arParms[0] = new OracleParameter("key", OracleDbType.Varchar2);
                arParms[0].Value = "Group1";
                arParms[1] = new OracleParameter("c_name", OracleDbType.Varchar2);
                arParms[1].Value = request.custName;
                arParms[2] = new OracleParameter("c_phone2", OracleDbType.Varchar2);
                arParms[2].Value = request.mobileNo;
                arParms[3] = new OracleParameter("c_fat_hus", OracleDbType.Varchar2);
                arParms[3].Value = request.fatherPrename + request.fatherName;
                arParms[4] = new OracleParameter("c_house_name", OracleDbType.Varchar2);
                arParms[4].Value = request.houseName;
                arParms[5] = new OracleParameter("c_locality", OracleDbType.Varchar2);
                arParms[5].Value = request.location;
                arParms[6] = new OracleParameter("c_pin_serial", OracleDbType.Varchar2);
                arParms[6].Value = request.pinSerial;
                arParms[7] = new OracleParameter("c_street", OracleDbType.Varchar2);
                arParms[7].Value = request.street;
                arParms[8] = new OracleParameter("c_date_of_birth", OracleDbType.Date);
                arParms[8].Value = Convert.ToDateTime(request.dob).ToString("MM/dd/yyyy");
                arParms[9] = new OracleParameter("c_kycid", OracleDbType.Long);
                arParms[9].Value = request.kycIdNo;
                arParms[10] = new OracleParameter("as_outresult", OracleDbType.RefCursor);
                arParms[10].Direction = ParameterDirection.Output;
                arParms[11] = new OracleParameter("c_kyctyp", OracleDbType.Long);
                arParms[11].Value = request.kycIdType;

                DsKycInfo = helper.ExecuteDataSet("plp_customer_filter", arParms);
                if (isValidDataset(DsKycInfo))
                {
                    DataTable dt = DsKycInfo.Tables[0];
                    matchingCustomerProperties matchingCustomerProperties;
                    List<matchingCustomerProperties> matchingCustomers = new List<matchingCustomerProperties>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        matchingCustomerProperties = new matchingCustomerProperties();
                        matchingCustomerProperties.group = dr[0].ToString();
                        matchingCustomerProperties.custId = dr[1].ToString();
                        matchingCustomerProperties.name = dr[2].ToString();
                        matchingCustomerProperties.phone2 = dr[3].ToString();
                        matchingCustomerProperties.address = dr[4].ToString();
                        matchingCustomerProperties.dateOfBirth = dr[5].ToString();
                        matchingCustomerProperties.idNumber = dr[6].ToString();
                        matchingCustomers.Add(matchingCustomerProperties);
                        //users.Add(dr[0].ToString());
                    }
                    response.matchingCustomers = matchingCustomers;
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;

                    
                }
                else
                {
                    response.status.code = APIStatus.failed;
                    response.status.message = "failed";
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

        #endregion


        private bool validateSearchCustomerRequest(SearchCustomerRequest searchCustomerRequest)
        {
            bool retVal = true;
            if (searchCustomerRequest.type == 3)//PAN NUMBER
            {
                Regex regex = new Regex(Constants.Strings.panNoFormat);
                if (!regex.IsMatch(searchCustomerRequest.searchValue))
                { retVal = false; }
                else
                { retVal = true; }
            }

            if (searchCustomerRequest.type == 4)//PHONE NUMBER
            {
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(searchCustomerRequest.searchValue))
                { retVal = false; }
                else
                { retVal = true; }
                if (searchCustomerRequest.searchValue.Length<5)
                { retVal = false; }
                else
                { retVal = true; }
            }

            if (searchCustomerRequest.type == 2)//CUSTOMER NAME
            {
                Regex regex = new Regex("^[a-zA-Z ]*$");
                if (!regex.IsMatch(searchCustomerRequest.searchValue))
                { retVal = false; }
                else
                { retVal = true; }
                if (searchCustomerRequest.searchValue.Length < 3)
                { retVal = false; }
                else
                { retVal = true; }
            }

            return retVal;

        }

        //-- send greetings mail while adding customer --- done by sreerekha k 100006 -- 24-feb-2020
        public EmailResponse Sendmail(string emailID,string customerurl )
        {

            EmailResponse response = new EmailResponse();

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(emailID, "Tomail"));
            msg.From = new MailAddress("mailcommunication@manappuram.com", "FromMail");
            msg.Subject = "Greetings from Manappuram";
            String htmlString = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border-bottom: 1px solid #f2eeed;} .subhead {font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font-size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font-size: 13.5px; line-height: 24px;} .button {text-align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font-family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo] .hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/p {color: #153643; font-family: sans-serif; font-size: 15px;} a:link { background-color: yellow; color:red; } a:visited { background-color: yellow; color: red; } .myButton { background-color: #ffea00; border-radius: 28px; border: 1px solid #fabc00; display: inline-block; cursor: pointer; color: #ff0000; font-family: Arial; font-size: 17px; padding: 16px 31px; text-decoration: none; text-shadow: 0px 1px 0px #fff200; } .myButton:hover { background-color: #dbc115; } .myButton:active { position: relative; top: 1px; } </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header' colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> <td align='right'> Toll Free Number: 1800-420-22-33 (24x7) </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='h2'> Greeting from, <br /> <span style='background-color: #FFFF00'> 'Manappuram' </span>! </td> </tr> <tr> <td class='bodycopy'> <p> Dear Sir/Madam, <br /> We take this opportunity to first of all thank you for choosing Manappuram Finance for servicing gold loan and other products. Through this mail, we would like to extend our warm welcome to you as a new customer in our Manappuram family. You have chosen a India’s largest gold loan company also the largest wealth creator for the year of 2019 as listed in ET 500 companies with market capitalisation of over Rs 5000 Crores. </p> <h3> List of features: </h3> <ul> <li> You can avail our online gold loan 24x7 anywhere anytime. </li> <li> Day wise Interest calculation. </li> <li> Part payment and partially settlement facility </li> <li> Lowest interest rate and maximum loan disbursement </li> <li> Complete onetime registration by providing your bank details in our branch for getting online gold loan. </li> <li> Weekly twice renew your loan and you can enjoy Rs 1% interest rate slab. </li> <li> Door-step gold loan service available. </li> <li> Avail gold loan in Manappuram and get free gold locker facility. </li> <li> <a href='https://play.google.com/store/apps/details?id=com.manappuram.b2c' target='_blank'> Download </a> Manappuram OGL application in your android smartphone and start transaction. </li> <li> You can pay your loan payment through Net banking, Debit card, Paytm, Google Pay, PhonePe etc… <a href='https://online.manappuram.com/' target='_blank'> Click here </a> </li> <li> Also you can enjoy our other services like Money transfer, DMTS, Vehicle loan, Home loan, Personal loan, business loan etc… </li> <li> Join as Business Associates and earn monthly income. <a href='https://play.google.com/store/apps/details?id=com.manappuram.goldloan' target='_blank'> Click here </a> </li> </ul> <p> We would like to thank you once again for choosing Manappuram Finance and we assure you of excellent service. If you have any kind of queries please visit <a href='https://www.manappuram.com/' target='_blank'> www.manappuram.com. </a> </p> <p>Considering your overall experience with us, how likely are you to choose Manappuram Finance for your next financial emergency, or to recommend us to your friends and family.Please rate us on a scale of 0 to 6 by clicking <a href='" + customerurl+ "' target='_blank'> " + customerurl + " </a> </p> <p> Thanking you.<br / Sincerely,<br /> Ratheesh C M <br /> DGM & National Head Sales </p> </td> </tr> <tr> <td> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> &nbsp; </td> <td align='right'> &nbsp; </td> </tr> </table> </td> </tr> <tr> <td colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a href='https://www.manappuram.com/' target='_blank' class='myButton'> Call Back </a> </td> <td align='center'> <a href='https://play.google.com/store/apps/details?id=com.mgc.ogl' target='_blank' class='myButton'> Mob app </a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center; line-height:20px;'> <b> Manappuram House </b> <br /> Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567, 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'><img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /></a> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </a> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";

            msg.IsBodyHtml = true;
            msg.Body = htmlString;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("mailcommunication@manappuram.com", "SH@123ho");
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.Send(msg);
                response.status.code = APIStatus.success;
                response.status.message = "Message Send Successfully";
            }
            catch (Exception ex)
            {
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }
            return response;

        }

        //mail send throgh Blue Mail
        protected EmailResponse SendEMAil(string emailID, string customerurl,string CustID,string CustName)
        {
            EmailResponse response = new EmailResponse();

            try
            {
                EmailUrl.ServiceSoapClient.EndpointConfiguration endpoint = new EmailUrl.ServiceSoapClient.EndpointConfiguration();
                EmailUrl.ServiceSoapClient myService = new EmailUrl.ServiceSoapClient(endpoint, "https://online.manappuram.com/SendMail/Service.asmx");
                              
                object messages = "";
                string fromMail = "mailcommunication@manappuram.com";
                string Subject = "Greetings from Manappuram";
                //old  string mailbody = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border-bottom: 1px solid #f2eeed;} .subhead {font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font-size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font-size: 13.5px; line-height: 24px;} .button {text-align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font-family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo] .hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/p {color: #153643; font-family: sans-serif; font-size: 15px;} a:link { background-color: yellow; color:red; } a:visited { background-color: yellow; color: red; } .myButton { background-color: #ffea00; border-radius: 28px; border: 1px solid #fabc00; display: inline-block; cursor: pointer; color: #ff0000; font-family: Arial; font-size: 17px; padding: 16px 31px; text-decoration: none; text-shadow: 0px 1px 0px #fff200; } .myButton:hover { background-color: #dbc115; } .myButton:active { position: relative; top: 1px; } </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header' colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> <td align='right'> Toll Free Number: 1800-420-22-33 (24x7) </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='h2'> Greeting from, <br /> <span style='background-color: #FFFF00'> 'Manappuram' </span>! </td> </tr> <tr> <td class='bodycopy'> <p> Dear Sir/Madam, <br /> We take this opportunity to first of all thank you for choosing Manappuram Finance for servicing gold loan and other products. Through this mail, we would like to extend our warm welcome to you as a new customer in our Manappuram family. You have chosen a India’s largest gold loan company also the largest wealth creator for the year of 2019 as listed in ET 500 companies with market capitalisation of over Rs 5000 Crores. </p> <h3> List of features: </h3> <ul> <li> You can avail our online gold loan 24x7 anywhere anytime. </li> <li> Day wise Interest calculation. </li> <li> Part payment and partially settlement facility </li> <li> Lowest interest rate and maximum loan disbursement </li> <li> Complete onetime registration by providing your bank details in our branch for getting online gold loan. </li> <li> Weekly twice renew your loan and you can enjoy Rs 1% interest rate slab. </li> <li> Door-step gold loan service available. </li> <li> Avail gold loan in Manappuram and get free gold locker facility. </li> <li> <a href='https://play.google.com/store/apps/details?id=com.manappuram.b2c' target='_blank'> Download </a> Manappuram OGL application in your android smartphone and start transaction. </li> <li> You can pay your loan payment through Net banking, Debit card, Paytm, Google Pay, PhonePe etc… <a href='https://online.manappuram.com/' target='_blank'> Click here </a> </li> <li> Also you can enjoy our other services like Money transfer, DMTS, Vehicle loan, Home loan, Personal loan, business loan etc… </li> <li> Join as Business Associates and earn monthly income. <a href='https://play.google.com/store/apps/details?id=com.manappuram.goldloan' target='_blank'> Click here </a> </li> </ul> <p> We would like to thank you once again for choosing Manappuram Finance and we assure you of excellent service. If you have any kind of queries please visit <a href='https://www.manappuram.com/' target='_blank'> www.manappuram.com. </a> </p> <p>Considering your overall experience with us, how likely are you to choose Manappuram Finance for your next financial emergency, or to recommend us to your friends and family.Please rate us on a scale of 0 to 6 by clicking <a href='" + customerurl + "' target='_blank'> " + customerurl + " </a> </p> <p> Thanking you.<br / Sincerely,<br /> Ratheesh C M <br /> DGM & National Head Sales </p> </td> </tr> <tr> <td> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> &nbsp; </td> <td align='right'> &nbsp; </td> </tr> </table> </td> </tr> <tr> <td colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a href='https://www.manappuram.com/' target='_blank' class='myButton'> Call Back </a> </td> <td align='center'> <a href='https://play.google.com/store/apps/details?id=com.mgc.ogl' target='_blank' class='myButton'> Mob app </a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center; line-height:20px;'> <b> Manappuram House </b> <br /> Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567, 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'><img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /></a> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </a> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";
                string mailbody = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border-bottom: 1px solid #f2eeed;} .subhead {font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font-size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font-size: 13.5px; line-height: 24px;} .button {text-align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font-family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo] .hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/p {color: #153643; font-family: sans-serif; font-size: 15px;} a:link { background-color: yellow; color:red; } a:visited { background-color: yellow; color: red; } .myButton { background-color: #ffea00; border-radius: 28px; border: 1px solid #fabc00; display: inline-block; cursor: pointer; color: #ff0000; font-family: Arial; font-size: 17px; padding: 16px 31px; text-decoration: none; text-shadow: 0px 1px 0px #fff200; } .myButton:hover { background-color: #dbc115; } .myButton:active { position: relative; top: 1px; } </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header' colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> <td align='right'> Toll Free Number: 1800-420-22-33 (24x7) </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='bodycopy'> <p> <b> Dear " + CustName + ", </b> <br /> <h3> Welcome to Manappuram family , we're excited you're here! </h3> <br /> You have made the right choice with India's leading NBFC for servicing gold loan and other products. we are happy to serve you. </p> <h3> List of features: </h3> <ul> <li> You can avail our online gold loan 24x7 anywhere anytime. </li> <li> Day wise Interest calculation. </li> <li> Part payment and partially settlement facility </li> <li> Lowest interest rate and maximum loan disbursement </li> <li> Contact our branch with your bank account details to complete one-time registration for OGL.You can also register digitally through Online.manappuram.com or OGL Mobile App. </li> <li> Weekly twice renew your loan and you can enjoy Rs 1% interest rate slab. </li> <li> Door-step gold loan service available. </li> <li> Avail gold loan in Manappuram and get free gold locker facility. </li> <li> <a href='https://play.google.com/store/apps/details?id=com.manappuram.b2c' target='_blank'> Download </a> Manappuram OGL application in your android smartphone and start transaction. </li> <li> You can pay your loan payment through Net banking, Debit card, Paytm, Google Pay, PhonePe etc… <a href='https://online.manappuram.com/' target='_blank'> Click here </a> </li> <li> Also you can enjoy our other services like Money transfer, DMTS, Vehicle loan, Home loan, Personal loan, business loan etc… </li> <li> Join as Business Associates and earn monthly income.To Know more <a href='https://www.manappuram.com/' target='_blank'> Click here </a> </li> </ul> <p> Thank you  for choosing Manappuram Finance Ltd. Assuring you of our best services always, we look forward to enhancing our relationship with you in future.</a> </p>  <p> Thanking you.<br /> Manappuram Finance Ltd. <br /> </p> </td> </tr> <tr> <td> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> &nbsp; </td> <td align='right'> &nbsp; </td> </tr> </table> </td> </tr> <tr> <td colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a href='https://www.manappuram.com/' target='_blank' class='myButton'> Call Back </a> </td> <td align='center'> <a href='https://play.google.com/store/apps/details?id=com.mgc.ogl' target='_blank' class='myButton'> Mob app </a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center; line-height:20px;'> <b> Manappuram House </b> <br /> Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567, 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'><img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /></a> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </a> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";
                messages = myService.sendemailAsync(emailID, "MAFIL", fromMail, "MANAPPURAM", Subject, mailbody);
                string mailbody1 = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border-bottom: 1px solid #f2eeed;} .subhead {font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font-size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font-size: 13.5px; line-height: 24px;} .button {text-align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font-family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo] .hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/p {color: #153643; font-family: sans-serif; font-size: 15px;} a:link { background-color: yellow; color:red; } a:visited { background-color: yellow; color: red; } .myButton { background-color: #ffea00; border-radius: 28px; border: 1px solid #fabc00; display: inline-block; cursor: pointer; color: #ff0000; font-family: Arial; font-size: 17px; padding: 16px 31px; text-decoration: none; text-shadow: 0px 1px 0px #fff200; } .myButton:hover { background-color: #dbc115; } .myButton:active { position: relative; top: 1px; } </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header' colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> <td align='right'> Toll Free Number: 1800-420-22-33 (24x7) </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='bodycopy'> <p> <b> Dear Customer, </b> <br /> We at Manappuram Finance Ltd. would appreciate your feedback on our product/support/service, which is why we are now sending you a short survey. </p> <br />  The survey take just five minutes to complete and your feedback will be of great help to us in the work to improve our product/support/service. <br /> <p> Thank you for your help! </a> </p> <b> To the Survey <a href='" + customerurl + "' target='_blank'> " + customerurl + " </a> <b/> <br/> <p> Best Regards ,<br/> Manappuram Finance Ltd. <br /> </p> </td> </tr> <tr> <td> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> &nbsp; </td> <td align='right'> &nbsp; </td> </tr> </table> </td> </tr> <tr> <td colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a href='https://www.manappuram.com/' target='_blank' class='myButton'> Call Back </a> </td> <td align='center'> <a href='https://play.google.com/store/apps/details?id=com.mgc.ogl' target='_blank' class='myButton'> Mob app </a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center; line-height:20px;'> <b> Manappuram House </b> <br /> Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567, 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'><img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /></a> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </a> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";
                messages = myService.sendemailAsync(emailID, "MAFIL", fromMail, "MANAPPURAM", Subject, mailbody1);

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


        protected EmailResponse SendEMAilToBA(string emailID, string CustID, string CustName,string baCode,string baName)
        {
            EmailResponse response = new EmailResponse();

            try
            {
                EmailUrl.ServiceSoapClient.EndpointConfiguration endpoint = new EmailUrl.ServiceSoapClient.EndpointConfiguration();
                EmailUrl.ServiceSoapClient myService = new EmailUrl.ServiceSoapClient(endpoint, "https://online.manappuram.com/SendMail/Service.asmx");

                object messages = "";
                string customerurl = "https://online.manappuram.com/custsurvey/CustomerSurveyBA.aspx?cid="+ emailID + " "+"~"+" "+ baCode + "";
                string fromMail = "mailcommunication@manappuram.com";
                string Subject = "Greetings from Manappuram";
                string mailbody = "<html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title>A Simple Responsive HTML Email</title> <style type='text/css'> body {margin: 0; padding: 0; min-width: 100%!important;} img {height: auto;} .content {width: 100%; max-width: 600px;} .header {padding: 40px 30px 20px 30px;} .innerpadding {padding: 30px 30px 30px 30px;} .borderbottom {border-bottom: 1px solid #f2eeed;} .subhead {font-size: 15px; color: #ffffff; font-family: sans-serif; letter-spacing: 10px;} .h1, .h2, .bodycopy {color: #153643; font-family: sans-serif;} .h1 {font-size: 33px; line-height: 38px; font-weight: bold;} .h2 {padding: 0 0 15px 0; font-size: 24px; line-height: 28px; font-weight: bold;} .bodycopy {font-size: 13.5px; line-height: 24px;} .button {text-align: center; font-size: 18px; font-family: sans-serif; font-weight: bold; padding: 0 30px 0 30px;} .button a {color: #ffffff; text-decoration: none;} .footer {padding: 20px 30px 15px 30px;} .footercopy {font-family: sans-serif; font-size: 14px; color: #ffffff;} .footercopy a {color: #ffffff; text-decoration: underline;} @media only screen and (max-width: 550px), screen and (max-device-width: 550px) { body[yahoo] .hide {display: none!important;} body[yahoo] .buttonwrapper {background-color: transparent!important;} body[yahoo] .button {padding: 0px!important;} body[yahoo] .button a {background-color: #e05443; padding: 15px 15px 13px!important;} body[yahoo] .unsubscribe {display: block; margin-top: 20px; padding: 10px 50px; background: #2f3942; border-radius: 5px; text-decoration: none!important; font-weight: bold;} } /*@media only screen and (min-device-width: 601px) { .content {width: 600px !important;} .col425 {width: 425px!important;} .col380 {width: 380px!important;} }*/p {color: #153643; font-family: sans-serif; font-size: 15px;} a:link { background-color: yellow; color:red; } a:visited { background-color: yellow; color: red; } .myButton { background-color: #ffea00; border-radius: 28px; border: 1px solid #fabc00; display: inline-block; cursor: pointer; color: #ff0000; font-family: Arial; font-size: 17px; padding: 16px 31px; text-decoration: none; text-shadow: 0px 1px 0px #fff200; } .myButton:hover { background-color: #dbc115; } .myButton:active { position: relative; top: 1px; } </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr> <td> <!--[if (gte mso 9)|(IE)]> <table width='600' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td> <![endif]--> <table bgcolor='#ffffff' class='content' align='center' cellpadding='0' cellspacing='0' border='0'> <tr> <td bgcolor='#ffffff' class='header' colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> <img class='fix' src='https://online.manappuram.com/images/manappuram-finance.png' width='171' height='51' border='0' alt='' /> </td> <td align='right'> Toll Free Number: 1800-420-22-33 (24x7) </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td class='bodycopy'> <p> Dear "+ baName + ", <br /> Hearty congratulations for mobilizing a new customer! We hope the same good work will continue in the coming days too </p>  <p> Also, please share your valuable feedback by clicking on the below link: <a href='" + customerurl + "' target='_blank'> " + customerurl + " </a> </p> <p> Thanking you.<br / Sincerely,<br /> MANAPPURAM FINANCE LIMITED <br /> </td> </tr> <tr> <td> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='left'> &nbsp; </td> <td align='right'> &nbsp; </td> </tr> </table> </td> </tr> <tr> <td colspan='2'> <table width='600' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a href='https://www.manappuram.com/' target='_blank' class='myButton'> Call Back </a> </td> <td align='center'> <a href='https://play.google.com/store/apps/details?id=com.mgc.ogl' target='_blank' class='myButton'> Mob app </a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding borderbottom'> <table width='500' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td height='115' style='padding: 0 20px 20px 0;'> <p style='text-align:center; line-height:20px;'> <b> Manappuram House </b> <br /> Head Office: IV / 470 (old) W638A (New), Manappuram House,<br />Valapad, Thrissur, Kerala, India,<br />Pin code : 680567, 1800-420-22-33 (toll free)</p> </td> </tr> </table> <table width='150' align='center' border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center'> <a style='background-color:white;' href='https://www.facebook.com/ManappuramFinanceLimitedMAFIL/' target='_blank'><img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/fb.png' width='32' height='32' border='0' alt='' /></a> </td> <td align='center'> <a style='background-color:white;' href='https://twitter.com/ManappuramMAFIL' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/twitter.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.linkedin.com/company/manappuram-finance-limited' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/instagram.png' width='32' height='32' border='0' alt='' /> </a> </td> <td align='center'> <a style='background-color:white;' href='https://www.youtube.com/channel/UC61FNQkz-EYTASuQwLBcfFQ' target='_blank'> <img class='fix' src='https://unsecurepl.manappuram.com/cglemaillogo/youtube.png' width='32' height='32' border='0' alt='' /> </a> </td> </tr> </table> </td> </tr> <tr> <td class='innerpadding bodycopy'><hr/> <p style='text-align:justify; font-size:10px; line-height:normal;'> 'This e-mail and any attachments thereto may contain confidential information and/or information protected by intellectual property rights for the exclusive attention of the intended addressees named above. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Unauthorized use, copying or further full or partial distribution of this e-mail or its contents is prohibited. Although this e-mail and any attachments are believed to be free of any virus or other defect that may affect any computer system into which it is received and opened, it is the responsibility of the recipient to ensure that it is virus free. Manappuram Finance Limited (MAFIL) is not liable for any loss or damage arising in any way from the use of this e-mail or its attachments.' </p> </td> </tr> </table> <!--[if (gte mso 9)|(IE)]> </td> </tr> </table> <![endif]--> </td> </tr></table></body></html>";

                messages = myService.sendemailAsync(emailID, CustName, fromMail, "MANAPPURAM", Subject, mailbody);

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



        /// <Created>Sreerekha - 100006</Created>
        /// <summary>Modify PEP Details</summary> 

        public UpdatePEPDetailsResponse UpdatePEPDetails(UpdatePEPDetailsRequest request)
        {
            UpdatePEPDetailsResponse response = new UpdatePEPDetailsResponse();
            try
            {
                string query  = "update customer_detail t set t.pep_id= " + request.PEPId + "  where t.cust_id='" + request.custId  + "'";
                helper.ExecuteNonQuery(query);
                response.status.flag = ProcessStatus.success;
                response.status.code = APIStatus.success ;
                response.status.message = "Succes";
            }
            catch (Exception ex)
            {
                response.status.flag = ProcessStatus.failed;
                response.status.code = APIStatus.exception;
                response.status.message = ex.Message;
            }

            return response;
        }
    }
}
