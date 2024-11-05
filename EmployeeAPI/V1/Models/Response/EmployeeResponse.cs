using APIBaseClassLibrary.V1.Models.Response;
using EmployeeAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.V1.Models.Response
{
    public class EmployeeResponse : BaseResponse
    {
        public EmployeeResponse() { }

        public int count { get; set; }
        public List<EmployeeProperties> employeeList { get; set; }
        //public int firmID { get; set; }
        //public string firmName { get; set; }
        //public int employeeID { get; set; }
        //public string employeeName { get; set; }
        //public int branchID { get; set; }
        //public string branchName { get; set; }
        //public int accessID { get; set; }
        //public int roleID { get; set; }
        //public int postID { get; set; }
    }

    public class EmployeeBranchResponse : BaseResponse
    {
     

        public string  branchID { get; set; }
       
    }
}
