using APIBaseClassLibrary.V1.Models.Response;
using BankAPI.V1.Models.Properties;
using System.Collections.Generic;

namespace EmployeeAPI.V1.Models.Response
{
    public class BankFillResponse : BaseResponse
    {
        public BankFillResponse() { }

        public int count { get; set; }
        public List<BankFillProperties> bankList { get; set; }
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
}
