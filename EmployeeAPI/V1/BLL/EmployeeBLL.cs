using APIBaseClassLibrary.V1.BLL;
using DataAccessLibrary;
using EmployeeAPI.Models.Request;
using EmployeeAPI.V1.Models.Properties;
using EmployeeAPI.V1.Models.Request;
using EmployeeAPI.V1.Models.Response;
using GlobalValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.V1.BLL
{
    public class EmployeeBLL: APIBaseBLL,IEmployeeBLL
    {

        public IDBAccessHelper helper;
        public IDBAccessDapperHelper DapperHelper;
        public EmployeeBLL(IDBAccessHelper _helper, IDBAccessDapperHelper _DapperHelper)
        {
            helper = _helper;
            DapperHelper = _DapperHelper;
        }


        /// <Created>Uneesh - 100156</Created>
        /// <summary>Get Employee Details</summary> 
        public EmployeeResponse getEmployeeDetails(EmployeeRequest request)
        {
            EmployeeResponse response = new EmployeeResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string query="";
                query = "select a.firm_id firmID,b.firm_name firmName,a.emp_code empCode,a.emp_name employeeName,c.branch_id branchID,";
                query += " c.branch_name branchName,a.post_id,a.access_id accessID,a.designation_id roleID,a.post_id postID from employee_master a";
                query += " inner join firm_master b on a.firm_id=b.firm_id ";
                query += " inner join branch_master c on a.branch_id=c.branch_id and a.firm_id=c.firm_id where 1=1 ";
              
                if (request.employeeID > 0)
                {
                    query += "and a.status_id in (1,10) and a.emp_code = " + request.employeeID;
                }
                if (request.branchID > 0)
                {
                    query += " and  a.branch_id = " + request.branchID;
                }


               List<EmployeeProperties> employee = DapperHelper.GetRecords<EmployeeProperties>(query,SQLMode.Query,null);
                
                if (employee!=null && employee.Count>0)
                {
                    response.employeeList = employee;
                    response.count = employee.Count;
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


        /// <Created>Uneesh - 100156</Created>
        /// <summary>Get Employee Branch</summary> 
        public EmployeeBranchResponse getEmployeeBranch(EmployeeBranchRequest request)
        {
            EmployeeBranchResponse response = new EmployeeBranchResponse();
            try
            {
                //DBAccessHelper helper = new DBAccessHelper();
                string query = "select t.branch_id as branchID from branch_sys_info t where upper(t.host_name) = '" + request.hostName.ToUpper()  + "' and upper(t.mac_address)='" + request.macAddress.ToUpper() +"' and t.status_id=1";

                object branchid = helper.ExecuteScalar<object>(query);
                if (branchid != null)
                {
                    response.branchID = branchid.ToString();                 
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

    }
}
