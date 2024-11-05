using APIBaseClassLibrary.V1.BLL;
using APIBaseClassLibrary.V1.Controllers;
using DataAccessLibrary;
using GlobalValues;
using Oracle.ManagedDataAccess.Client;
using PANAPI.V1.BLLDependency;
using PANAPI.V1.Models.Properties;
using PANAPI.V1.Models.Request;
using PANAPI.V1.Models.Response;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;
using static System.Net.Mime.MediaTypeNames;
namespace PANAPI.V1.BLL
{
    public class PanBLL: APIBaseBLL , IPanBLL
    {
        public IDBAccessHelper helper;
        public PanBLL(IDBAccessHelper _helper)
        {
            helper = _helper;
        }
        #region Pan

        #region PanGet
        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Pan Details</summary> 
        public PanDetailsResponse getPanDetails(PanDetailsRequest panDetailsRequest)
        {
            //DBAccessHelper helper = new DBAccessHelper();
            PanDetailsResponse panDetailsResponse = new PanDetailsResponse();
            try
            {

                var query = "select t.cust_id,t.pan,t.pan_copy from  DEPOSIT_PAN_DETAIL t,  customer c  where c.cust_id = t.cust_id  and c.isactive in (1, null, 0, 5)     and c.cust_id = '" + panDetailsRequest.custId + "'";

                DataSet ds = helper.ExecuteDataSet(query);
                DataTable dt = ds.Tables[0];
                List<PanProperties> panList = new List<PanProperties>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string base64String = null;
                        DataRow dRow = dt.Rows[i];
                        if (dRow["pan_copy"] != null)
                        {
                            byte[] imagebyte = (byte[])(dRow["pan_copy"] == DBNull.Value ? ImageToByte() : (dRow["pan_copy"]));
                            base64String = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                        }
                        panList.Add(new PanProperties
                        {
                            PAN = Convert.ToString(dRow["pan"]),
                            CUST_ID = Convert.ToString(dRow["cust_id"]),
                            PAN_COPY = base64String,

                        });
                    }
                }           


                if (panList.Count > 0)
                {
                    panDetailsResponse.panList = panList;
                    panDetailsResponse.status.code = APIStatus.success;
                    panDetailsResponse.status.message = "Success";
                    panDetailsResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    panDetailsResponse.status.code = APIStatus.no_Data_Found;
                    panDetailsResponse.status.message = "No data found for your search";
                    panDetailsResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                panDetailsResponse.status.code = APIStatus.exception;
                panDetailsResponse.status.message = ex.Message;
                panDetailsResponse.status.flag = ProcessStatus.failed;
            }

            return panDetailsResponse;
        }

        public static byte[] ImageToByte()
        {          
            MemoryStream imgStream = new MemoryStream();        
            byte[] byteArray = imgStream.ToArray();           
            return byteArray;
        }

        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Get Pan Count</summary> 
        public PanCountResponse getPanCount(GetPanRequest getPanRequest)
        {
           // DBAccessHelper helper = new DBAccessHelper();
            PanCountResponse panCountResponse = new PanCountResponse();
            try
            {
                var query = "select count(*) aa from  DEPOSIT_PAN_DETAIL t,  customer c  where c.cust_id = t.cust_id  and c.isactive in (1, null, 0, 5)   and t.pan = '" + getPanRequest .panNo + "'   and c.cust_id <> '" + getPanRequest .custId + "'";

                decimal obj = helper.ExecuteScalar<decimal>(query);

                if (obj != -1)
                {
                    panCountResponse.panCount = Convert.ToInt64(obj);
                    panCountResponse.status.code = APIStatus.success;
                    panCountResponse.status.message = "Success";
                    panCountResponse.status.flag = ProcessStatus.success;
                }
                else
                {
                    panCountResponse.status.code = APIStatus.no_Data_Found;
                    panCountResponse.status.message = "No data found for your search";
                    panCountResponse.status.flag = ProcessStatus.success;
                }

            }
            catch (Exception ex)
            {
                panCountResponse.status.code = APIStatus.exception;
                panCountResponse.status.message = ex.Message;
                panCountResponse.status.flag = ProcessStatus.failed;
            }

            return panCountResponse;
        }


        #endregion PanGet

        #region PanPost
        /// <Created>Rizwan S I - 100011</Created>
        /// <summary>Update Pan</summary>                      
        public UpdatePanResponse updatePan(UpdatePanDetailsRequest updatePanDetailsRequest)
        {
          //  DBAccessHelper helper = new DBAccessHelper();
            UpdatePanResponse updatePanResponse = new UpdatePanResponse();
            try
            {
                try
                {
                    GetPanRequest getPanRequest = new GetPanRequest();
                    getPanRequest.custId = updatePanDetailsRequest.custId;
                    getPanRequest.panNo = updatePanDetailsRequest.newPan;
                    //4th alphabet must be either P or C or H or A or B or G or J or L or F or T ---21-oct-2020
                            string[] panChars = Constants.Strings.validPanChars.Split(",");
                            bool validPan = false;
                            for (int i = 0; i < panChars.Length; i++)
                            {
                                if (panChars[i] == updatePanDetailsRequest.newPan.Substring(3, 1))
                                {
                                    validPan = true;
                                    break;
                                }
                            }
                            if (!validPan)
                            {
                            updatePanResponse.status.code = APIStatus.failed;
                            updatePanResponse.message = "Pls enter valid alphabet.Pls Check 4th Alphabet";
                            updatePanResponse.status.flag = ProcessStatus.success;
                            return updatePanResponse;
                            }
                         

                   long panCount = getPanCount(getPanRequest).panCount;

                    if (panCount > 0)
                    {
                        updatePanResponse.message = "This PAN number already exist with another customer ID..!!";
                        updatePanResponse.status.flag = ProcessStatus.success;                       
                    }
                    else
                    {

                        OracleParameter[] parameters = new OracleParameter[5];

                        parameters[0] = new OracleParameter("CustID", OracleDbType.Varchar2, 500);
                        parameters[0].Value = updatePanDetailsRequest.custId;
                        parameters[0].Direction = ParameterDirection.Input;

                        parameters[1] = new OracleParameter("CustType", OracleDbType.Long, 5);
                        parameters[1].Value =Convert.ToInt64(updatePanDetailsRequest.custType);
                        parameters[1].Direction = ParameterDirection.Input;

                        parameters[2] = new OracleParameter("NewPAN", OracleDbType.Varchar2, 500);
                        parameters[2].Value = updatePanDetailsRequest.newPan;
                        parameters[2].Direction = ParameterDirection.Input;

                        parameters[3] = new OracleParameter("UserID", OracleDbType.Varchar2, 500);
                        parameters[3].Value = updatePanDetailsRequest.userId;
                        parameters[3].Direction = ParameterDirection.Input;

                        parameters[4] = new OracleParameter("ErrMsg", OracleDbType.Varchar2, 800);
                        parameters[4].Direction = ParameterDirection.Output;

                        helper.ExecuteNonQuery("PanCardModification", parameters);
                        if (parameters[4].Value != null)
                        {

                            byte[] imageBytes = Convert.FromBase64String(updatePanDetailsRequest.panPhoto);
                            OracleParameter[] parm = new OracleParameter[1];
                            parm[0] = new OracleParameter("image", OracleDbType.Blob);
                            parm[0].Direction = ParameterDirection.Input;
                            parm[0].Value = imageBytes;
                            var query = "update  DEPOSIT_PAN_DETAIL set PAN_COPY=:image where cust_id = '" + updatePanDetailsRequest.custId + "'";
                            helper.ExecuteNonQuery(query, parm);
                            updatePanResponse.message = parameters[4].Value.ToString();
                            updatePanResponse.status.code = APIStatus.success;
                            updatePanResponse.status.message = "Success";
                            updatePanResponse.status.flag = ProcessStatus.success;
                        }
                        else
                        {
                            updatePanResponse.message = parameters[4].Value.ToString();
                            updatePanResponse.status.flag = ProcessStatus.success;
                        }
                    }

                }
                catch (Exception ex)
                {
                    updatePanResponse.status.code = APIStatus.exception;
                    updatePanResponse.status.message = ex.Message;
                    updatePanResponse.status.flag = ProcessStatus.failed;

                }

            }
            catch (Exception ex)
            {
                updatePanResponse.status.code = APIStatus.exception;
                updatePanResponse.status.message = ex.Message;
                updatePanResponse.status.flag = ProcessStatus.failed;

            }

            return updatePanResponse;
        }


        #endregion PanPost

        #endregion Pan


        #region Form60

        public UpdateForm60Response updateForm60(UpdateForm60Request updateForm60Request)
        {
             UpdateForm60Response updateForm60Response = new UpdateForm60Response();
            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
            var query = "";
            var query1 = "";
            try
                {
                if (updateForm60Request.form60Image != null)
                {
                    ods = helper.ExecuteDataSet("select count(*) from  TBL_CUSTOMER_FORM60 where cust_id='" + updateForm60Request.custId + "'");
                    if (Convert.ToInt32(ods.Tables[0].Rows[0][0]) > 0)
                    {
                        query = "update  TBL_CUSTOMER_FORM60 set form_60 =:image where cust_id='" + updateForm60Request.custId + "'" ;
                        byte[] imageBytes = Convert.FromBase64String(updateForm60Request.form60Image);
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter("image", OracleDbType.Blob);
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(query, parm);
                        updateForm60Response.message = "Success";
                        updateForm60Response.status.code = APIStatus.success;
                        updateForm60Response.status.message = "Success";
                        updateForm60Response.status.flag = ProcessStatus.success;
                    }
                    else
                    {
                        query1 = "insert into  TBL_CUSTOMER_FORM60(CUST_ID,TRA_DT,USER_ID) values('"+ updateForm60Request .custId+ "', sysdate,"+ updateForm60Request .userId + " )";
                        helper.ExecuteNonQuery(query1);
                        query = "update  TBL_CUSTOMER_FORM60 set form_60 =:image where cust_id='" + updateForm60Request.custId + "'";
                        byte[] imageBytes = Convert.FromBase64String(updateForm60Request.form60Image);
                        OracleParameter[] parm = new OracleParameter[1];
                        parm[0] = new OracleParameter("image", OracleDbType.Blob);
                        parm[0].Direction = ParameterDirection.Input;
                        parm[0].Value = imageBytes;
                        helper.ExecuteNonQuery(query, parm);
                        updateForm60Response.message = "Success";
                        updateForm60Response.status.code = APIStatus.success;
                        updateForm60Response.status.message = "Success";
                        updateForm60Response.status.flag = ProcessStatus.success;
                    }

                   

                }
                else
                {
                    updateForm60Response.message = "Please Upload Form60";
                    updateForm60Response.status.flag = ProcessStatus.success;
                }
                
                }
                catch (Exception ex)
                {
                   updateForm60Response.status.code = APIStatus.exception;
                   updateForm60Response.status.message = ex.Message;
                   updateForm60Response.status.flag = ProcessStatus.failed;

                }

            return updateForm60Response;
     }
        public Form60DetailsResponse getForm60Details(Form60DeatilsRequest form60DetailsRequest)

        {
            Form60DetailsResponse form60DetailsResponse = new Form60DetailsResponse();
            DataTable dt = new DataTable();
            // dt = helper.ExecuteDataSet("select t.form_60 from  tbl_customer_form60 t where t.cust_id= '" + form60DetailsRequest.custId + "'").Tables[0];
            dt = helper.ExecuteDataSet("select f.amount,f.tran_mode,to_char(f.pan_applied_dt),f.pan_ack_no,f.agri_income,f.other_agri_income  from  tbl_customer_form60 f  where f.cust_id = '" + form60DetailsRequest.custId + "'").Tables[0];

            if ((dt != null) && (dt.Rows.Count > 0))
            {
              
                try
                {
                    form60DetailsResponse.transactionAmount =Convert.ToString(dt.Rows[0][0]);
                    form60DetailsResponse.transactionMode = Convert.ToString(dt.Rows[0][1]);
                    form60DetailsResponse.panApplyDate = Convert.ToString(dt.Rows[0][2]);
                    form60DetailsResponse.panAcknowledgeNo = Convert.ToString(dt.Rows[0][3]);
                    form60DetailsResponse.agriIncome = Convert.ToString(dt.Rows[0][4]);
                    form60DetailsResponse.otherAgriIncome = Convert.ToString(dt.Rows[0][5]);

                    dt = helper.ExecuteDataSet("select t.form_60 from  tbl_customer_form60 t where t.cust_id= '" + form60DetailsRequest.custId + "' and t.form_60 is not null").Tables[0];
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        byte[] imagebyte = (byte[])dt.Rows[0][0];
                        form60DetailsResponse.form60Image = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
                    }
                    else
                    { form60DetailsResponse.form60Image = ""; }
                    
                    form60DetailsResponse.status.code = APIStatus.success;
                    form60DetailsResponse.status.message = "Success";
                    form60DetailsResponse.status.flag = ProcessStatus.success;
                }

                catch (Exception ex)
                {
                    form60DetailsResponse.status.code = APIStatus.exception;
                    form60DetailsResponse.status.message = ex.Message;
                    form60DetailsResponse.status.flag = ProcessStatus.failed;
                    return form60DetailsResponse;
                }
            }

            dt = helper.ExecuteDataSet("select t.docid, b.id_number, t.issue_auth from tbl_form60_docid t, identity_dtl b where b.cust_id =  '" + form60DetailsRequest.custId + "'  and t.identity_id = b.identity_id").Tables[0];
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                form60DetailsResponse.documentCode = Convert.ToString(dt.Rows[0][0]);
                form60DetailsResponse.documentIdNumber = Convert.ToString(dt.Rows[0][1]);
                form60DetailsResponse.issuedBy = Convert.ToString(dt.Rows[0][2]);

            }
 

            return form60DetailsResponse;
        }


        public SaveForm60DetailsResponse SaveForm60Details(SaveForm60DetailsRequest saveForm60Request)
        {
            SaveForm60DetailsResponse saveForm60Response = new SaveForm60DetailsResponse();
            DataSet ods = new DataSet();
            DataTable dt = new DataTable();
         
            try
            {
                OracleParameter[] parameters = new OracleParameter[9];

                parameters[0] = new OracleParameter("(custid", OracleDbType.Varchar2, 20);
                parameters[0].Value = saveForm60Request.custId;
                parameters[0].Direction = ParameterDirection.Input;

                parameters[1] = new OracleParameter("amt", OracleDbType.Long, 15);
                parameters[1].Value = Convert.ToInt64(saveForm60Request.amountOfTransaction);
                parameters[1].Direction = ParameterDirection.Input;

                parameters[2] = new OracleParameter("tranmode", OracleDbType.Varchar2, 100);
                parameters[2].Value = saveForm60Request.transactionMode;
                parameters[2].Direction = ParameterDirection.Input;

                parameters[3] = new OracleParameter("panappdt", OracleDbType.Date, 50);
                if (!string.IsNullOrEmpty(saveForm60Request.panApplyDate))
                {
                    parameters[3].Value = saveForm60Request.panApplyDate;
                }
                else
                {
                    parameters[3].Value = DBNull.Value;
                }
                parameters[3].Direction = ParameterDirection.Input;

                parameters[4] = new OracleParameter("panackno", OracleDbType.Varchar2, 100);
                parameters[4].Value = saveForm60Request.panAcknowledgementNo;
                parameters[4].Direction = ParameterDirection.Input;

                parameters[5] = new OracleParameter("agri_inc", OracleDbType.Long, 20);
                parameters[5].Value = saveForm60Request.agriIncome;
                parameters[5].Direction = ParameterDirection.Input;

                parameters[6] = new OracleParameter("oth_agri_inc", OracleDbType.Long, 20);
                parameters[6].Value = saveForm60Request.OtherAgriIncome;
                parameters[6].Direction = ParameterDirection.Input;

                parameters[7] = new OracleParameter("usrid", OracleDbType.Long, 10);
                parameters[7].Value = saveForm60Request.userId;
                parameters[7].Direction = ParameterDirection.Input;

                parameters[8] = new OracleParameter("err_msg", OracleDbType.Varchar2, 800);
                parameters[8].Direction = ParameterDirection.Output;

                helper.ExecuteNonQuery("proc_customer_form60", parameters);

                if (parameters[8].Value != null)
                {
                    saveForm60Response.status.code = APIStatus.success;
                    saveForm60Response.status.message = Convert.ToString(parameters[8].Value);
                    saveForm60Response.status.flag = ProcessStatus.success;
                }

                }
            catch (Exception ex)
            {
                saveForm60Response.status.code = APIStatus.exception;
                saveForm60Response.status.message = ex.Message;
                saveForm60Response.status.flag = ProcessStatus.failed;

            }

            return saveForm60Response;
        }

        #endregion Form60

    }
}
