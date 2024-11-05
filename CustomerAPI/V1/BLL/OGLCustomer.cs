using APIBaseClassLibrary.V1.BLL;
using CustomerAPI.V1.Models.Request;
using CustomerAPI.V1.Models.Response;
using DataAccessLibrary;
using GlobalValues;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.BLL
{
    public class OGLCustomer: APIBaseBLL, IOGLCustomer
    {
        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public OGLCustomer(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }

        public OGLCustomerResponse searchOGLCustomer(OGLCustomerRequest request)
        {
            OGLCustomerResponse searchCustomerResponse = new OGLCustomerResponse();
            try
            {
            //DBAccessHelper helper = new DBAccessHelper();
                var query = "select REBATE_STATUS,VER_ID,BRANCH_ID,CUST_ID,CUST_NAME,STATUS_ID,INVENTORY,SCHEME from TBL_ONLINE_GL_CUST_MST t where  t.status_id not in (2,3) and CUST_ID='" + request.custID + "'";
                OGLCustomerResponse customer = DapperHelper.GetRecord<OGLCustomerResponse>(query,SQLMode.Query,null);
                if (customer != null && customer.CUST_ID != null)
                {
                    searchCustomerResponse = customer;
                    searchCustomerResponse.status.code = APIStatus.success;
                    searchCustomerResponse.status.message = "Success";
                    searchCustomerResponse.status.flag = ProcessStatus.success;
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
            
            return searchCustomerResponse;
        }

    }
}
