using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using APIBaseClassLibrary.V1.BLL;
using BankAPI.V1.Models.Properties;
using BankAPI.V1.Models.Request;
using BankAPI.V1.Models.Response;
using DataAccessLibrary;
using EmployeeAPI.V1.Models.Response;
using GlobalValues;
using Oracle.ManagedDataAccess.Client;
using static GlobalValues.GlobalVariables;

namespace BankAPI.V1.BLL
{
    public class BankBLL : APIBaseBLL, IBankBLL
    {

        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public BankBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }
        #region BankGet

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>GetvBankvDetailsvByvIFSC</summary> 
        public BankResponse getBankDetailsByIFSC(BankRequest request)
        {
            BankResponse response = new BankResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string query = "select t.state_id stateID,t.state stateName,t.dist_id districtID,t.district districtName,t.IFSC_code ifsccode,t.bank_id bankID,abbr || ',' || branch as bankname";
                query += "  from NEFT_BANK_MST t where t.ifsc_code = '" + request.IFSC_Code + "' and t.STATUS_ID = 1 ";
                BankResponse consent = DapperHelper.GetRecord<BankResponse>(query, SQLMode.Query, null);
                if (consent!=null && consent.BANKID>0)
                {
                    response = consent;
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.status.code = APIStatus.failed;
                    response.status.message = "This IFSCCode Not In Our Database,Please contact Marketing Team";
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
        /// <summary>Get BankList</summary> 
        public BankFillResponse getBankList(BankFillRequest request)
        {
            BankFillResponse response = new BankFillResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                //string query = "select trim(ifsc_code) IFSCCODE,t.bank_id BANKID,trim(branch) as BANKNAME from neft_bank_mst t";
                //query += " where  t.dist_id=" + request.districtId + " and STATUS_ID = 1 order by bankname||','||branch";


                string query = "select trim(ifsc_code) IFSCCODE,t.bank_id BANKID,abbr || ',' || branch as BANKNAME from neft_bank_mst t";
                query += " where  t.dist_id=" + request.districtId + " and STATUS_ID = 1 order by bankname||','||branch";
                List <BankFillProperties> bankList = DapperHelper.GetRecords<BankFillProperties>(query,SQLMode.Query,null);
                if (bankList != null && bankList.Count > 0)
                {
                    response.bankList = bankList;
                    response.count = bankList.Count;
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

        #endregion BankGet

        #region BankPost
        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Check is Valid Neft Flag</summary> 
        private bool isValidNeftFlag(string custID)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            string query = "select t.ifsc_code,(select s.bankname from neft_bank_mst s where s.ifsc_code=t.ifsc_code) beneficiary_bank,";
            query += " t.beneficiary_branch,t.beneficiary_account,c.account_name,t.bank_id,t.cust_id ,t.serialno,decode(t.verify_status,";
            query += " 'T','Approved','F','Pending for Approval') status,c.acc_type from neft_customer t,neft_current_account c where t.acc_type=c.acc_type and cust_id='" + custID + "'";
            DataSet ds = helper.ExecuteDataSet(query);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Check is Valid IFSC</summary> 
        private bool isValidIFSC(string IFSCCode)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            string query = "select t.state_id,t.dist_id,t.IFSC_code from NEFT_BANK_MST t where t.ifsc_code='" + IFSCCode + "' and t.STATUS_ID = 1";
            DataSet ds = helper.ExecuteDataSet(query);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Add Bank Details</summary> 
        public AddBankResponse addBankRequest(AddBankRequest request)
        {
            AddBankResponse response = new AddBankResponse();
            try
            {
                if (isValidNeftFlag(request.custID))
                {
                    if (isValidIFSC(request.IFSCCode))
                    {
                        //DBAccessHelper helper = new DBAccessHelper();
                        OracleParameter[] pr = new OracleParameter[10];
                        pr[0] = new OracleParameter("brid", OracleDbType.Long, 10);
                        pr[0].Value = request.branchID;
                        pr[1] = new OracleParameter("fmid", OracleDbType.Long, 4);
                        pr[1].Value = request.firmID;
                        pr[2] = new OracleParameter("custid", OracleDbType.Varchar2, 20);
                        pr[2].Value = request.custID;
                        pr[3] = new OracleParameter("ifsc", OracleDbType.Varchar2, 50);
                        pr[3].Value = request.IFSCCode;
                        pr[4] = new OracleParameter("accno", OracleDbType.Varchar2, 50);
                        pr[4].Value = request.accountNo;
                        pr[5] = new OracleParameter("rec_acc_typ", OracleDbType.Varchar2, 100);
                        pr[5].Value = request.accountType;
                        pr[6] = new OracleParameter("custname", OracleDbType.Varchar2, 100);
                        pr[6].Value = request.custName;
                        pr[7] = new OracleParameter("branch_name", OracleDbType.Varchar2, 100);
                        pr[7].Value = request.branchName;
                        pr[8] = new OracleParameter("mobile_no", OracleDbType.Long, 20);
                        pr[8].Value = request.mobileNo;
                        pr[9] = new OracleParameter("er_no", OracleDbType.Decimal,100);
                        pr[9].Direction = ParameterDirection.InputOutput;


                        helper.ExecuteNonQuery("proc_neft_add_details", pr);
                       // response.er_no = pr[9].Value;
                        if (pr[9].Value.ToString() != null)
                        {


                            if (pr[9].Value.ToString() == "0")
                            {

                                byte[] imageBytes = Convert.FromBase64String(request.neftPhoto);
                                //System.Drawing.Image img = Base64ToImage(request.neftPhoto);
                                //byte[] imageBytes = ImageToByteArray(img);

                                OracleParameter[] parm = new OracleParameter[1];
                                parm[0] = new OracleParameter("image", OracleDbType.Blob);
                                parm[0].Direction = ParameterDirection.Input;
                                parm[0].Value = imageBytes;
                                var query = "update neft_customer set id_proof=:image where cust_id='" + request.custID + "'";
                                helper.ExecuteNonQuery(query, parm);

                                OracleParameter[] parmAdd = new OracleParameter[1];
                                parmAdd[0] = new OracleParameter("imageAdd", OracleDbType.Blob);
                                parmAdd[0].Direction = ParameterDirection.Input;
                                parmAdd[0].Value = imageBytes;
                                var queryAdd = "update TBL_ADD_NEFT_CUSTOMER set id_proof=:imageAdd where cust_id='" + request.custID + "' and STATUS=0 ";
                                helper.ExecuteNonQuery(queryAdd, parmAdd);

                                response.status.code = APIStatus.success;
                                response.status.message = "NEFT details updated successfully";
                                response.status.flag = ProcessStatus.success;
                            }
                            else if (pr[9].Value.ToString() == "1")
                            {
                                response.status.code = APIStatus.alreadyExist;
                                response.status.message = "This account details already exist in another customer id";
                                response.status.flag = ProcessStatus.success;
                            }
                            else if (pr[9].Value.ToString() == "2")
                            {
                                response.status.code = APIStatus.alreadyExist;
                                response.status.message = "This account details already rejected, please verify";
                                response.status.flag = ProcessStatus.success;
                            }
                            else if (pr[9].Value.ToString() == "3")// new change added on 10-sep-2020---done by SReerekha K 100006
                            {
                                response.status.code = APIStatus.alreadyExist;
                                response.status.message = "Bank account details updation of employees on behalf of customer is blocked";
                                response.status.flag = ProcessStatus.success;
                            }
                            else
                            {

                                byte[] imageBytes = Convert.FromBase64String(request.neftPhoto);
                                OracleParameter[] parm = new OracleParameter[1];
                                parm[0] = new OracleParameter("image", OracleDbType.Blob);
                                parm[0].Direction = ParameterDirection.Input;
                                parm[0].Value = imageBytes;
                                var query = "update neft_customer set id_proof=:image where cust_id='" + request.custID + "'";
                                helper.ExecuteNonQuery(query, parm);

                                OracleParameter[] parmAdd = new OracleParameter[1];
                                parmAdd[0] = new OracleParameter("imageAdd", OracleDbType.Blob);
                                parmAdd[0].Direction = ParameterDirection.Input;
                                parmAdd[0].Value = imageBytes;
                                var queryAdd = "update TBL_ADD_NEFT_CUSTOMER set id_proof=:imageAdd where cust_id='" + request.custID + "' and STATUS=0 ";
                                helper.ExecuteNonQuery(queryAdd, parmAdd);

                                response.status.code = APIStatus.success;
                                response.status.message = "NEFT details updated successfully";
                                response.status.flag = ProcessStatus.success;
                            }
                        }
                        else
                        {
                            response.status.flag = ProcessStatus.success;
                            response.status.code = APIStatus.failed;
                            response.status.message = "Something happens in server, please contact HO";
                        }
                    }
                    else
                    {
                        response.status.flag = ProcessStatus.success;
                        response.status.code = APIStatus.invalidIFSCCode;
                        response.status.message = "Invalid IFSC code or no data found for the IFSC code " + request.IFSCCode;
                    }
                }
                else
                {
                    response.status.flag = ProcessStatus.success;
                    response.status.code = APIStatus.neftModificationRequired;
                    response.status.message = "Please use application available in Dotnet Lite to MODIFY customer NEFT details. Path : Loan -> Others -> Customer Change -> Offline Customer NEFT Modification";
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

        #endregion BankPost

        #region BankcustGet

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Bank Details</summary> 
        public getBankResponse getBankDetails(BankRequestCust request)
        {
            getBankResponse response = new getBankResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string query;
                query = "select t.ifsc_code ifscCode,  s.bankname bankName,  t.beneficiary_branch bankBranch,  t.beneficiary_account bankAccount,  c.account_name accountName,  t.bank_id bankID,  t.cust_id custID,  t.serialno serialNO,";
                query += " decode(t.verify_status, 'T', 'Approved', 'F', 'Pending for Approval') accStatus, t.CUST_NAME CustName, s.state_id stateID,  s.state stateName,  s.dist_id districtID,  s.district districtName, c.acc_type accountType  from neft_customer t,";
                query += " neft_current_account c,NEFT_BANK_MST s  where t.acc_type = c.acc_type and s.ifsc_code = t.ifsc_code  ";
                query +=" and cust_id='" + request.custID + "'";
                getBankResponse consent = DapperHelper.GetRecord<getBankResponse>(query,SQLMode.Query,null);
                if (consent!=null && consent.BANKID>0)
                {
                    response = consent;
                    response.status.code = APIStatus.success;
                    response.status.message = "Success";
                    response.status.flag = ProcessStatus.success;
                }
                else
                {
                    response.status.flag = ProcessStatus.success;
                    response.status.code = APIStatus.invalidIFSCCode;
                    response.status.message = "Invalid IFSC code or no data found for the Customer ID " + request.custID;
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



        #endregion BankcustGet

        #region AccountTypesGet

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Account Types</summary> 
        public AccountTypeResponse getAccountTypes()
        {
            AccountTypeResponse accountTypeResponse = new AccountTypeResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string query = "select acc_type,account_name from neft_current_account order by acc_type";
                var accountTypeList = DapperHelper.GetRecords<AccountTypeProperties>(query,SQLMode.Query,null);
                if (accountTypeList.Count > 0)
                {
                    accountTypeResponse.accountTypeList = accountTypeList;
                    accountTypeResponse.status.code = APIStatus.success;
                    accountTypeResponse.status.message = "Success";
                    accountTypeResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    accountTypeResponse.status.code = APIStatus.no_Data_Found;
                    accountTypeResponse.status.message = "No data found for your search";
                    accountTypeResponse.status.flag = ProcessStatus.success;
                }
            }
            catch (Exception e)
            {
                accountTypeResponse.status.flag = ProcessStatus.failed;
                accountTypeResponse.status.code = APIStatus.exception;
                accountTypeResponse.status.message = e.Message;
            }

            return accountTypeResponse;
        }

        #endregion AccountTypesGet

        #region CustomerBankPhotoGet

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Customer Bank Photo</summary> 
        public CustomerBankPhotoResponse CustomerBankPhotoGet(CustomerBankPhotoRequest customerBankPhotoRequest)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            CustomerBankPhotoResponse customerBankPhotoResponse = new CustomerBankPhotoResponse();
            try
            {
                var query = "select t.id_proof from NEFT_CUSTOMER t where t.cust_id='"+ customerBankPhotoRequest.custID  + "' and t.id_proof is not null";
                          

                DataSet ds = helper.ExecuteDataSet(query);
                DataTable dt = ds.Tables[0];
                string base64String = null;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                      
                        DataRow dRow = dt.Rows[i];
                        if (dRow["id_proof"] != null)
                        {

                            byte[] imagebyte = (byte[])(dRow["id_proof"] == DBNull.Value ? ImageToByte() : (dRow["id_proof"]));
                            try
                            {
                                customerBankPhotoResponse.tiffIdProof = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
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
                            catch(Exception ex)
                            {
                                imagebyte = (byte[])(dRow["id_proof"] == DBNull.Value ? ImageToByte() : (dRow["id_proof"]));

                            }

                            base64String = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                        }
                 
                    }
                }


                if (base64String !=null)
                {
                    customerBankPhotoResponse.idProof = base64String;
                    customerBankPhotoResponse.status.code = APIStatus.success;
                    customerBankPhotoResponse.status.message = "Success";
                    customerBankPhotoResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    customerBankPhotoResponse.status.code = APIStatus.no_Data_Found;
                    customerBankPhotoResponse.status.message = "Bank passbook not uploaded yet";
                    customerBankPhotoResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                customerBankPhotoResponse.status.code = APIStatus.exception;
                customerBankPhotoResponse.status.message = ex.Message;
                customerBankPhotoResponse.status.flag = ProcessStatus.failed;
            }

            return customerBankPhotoResponse;
        }

        /// <Created>Aravind - 100231</Created>
        /// <summary>Get Image type</summary> 
        public static string GeMimeTypeFromImageByteArray(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            using (Image image = Image.FromStream(stream))
            {
                return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == image.RawFormat.Guid).MimeType;
            }
        }

        /// <Created>Aravind - 100231</Created>
        /// <summary>Convert Image to Byte</summary> 
        public static byte[] ImageToByte()
        {
            MemoryStream imgStream = new MemoryStream();
            byte[] byteArray = imgStream.ToArray();
            return byteArray;
        }


        #endregion CustomerBankPhotoGet

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

    }
}
