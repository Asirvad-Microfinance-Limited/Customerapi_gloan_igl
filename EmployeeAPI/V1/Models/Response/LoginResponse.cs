using APIBaseClassLibrary.V1.Models.Response;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.Models
{
    public class LoginResponse : BaseResponse
    {

        public LoginResponse()
        {
           
        }

        public LoginStatus loginStatus { get; set; }
        public int firmID { get; set; }
        public string firmName { get; set; }
        public int empCode { get; set; }
        public string employeeName { get; set; }
        public int branchID { get; set; }
        public string branchName { get; set; }
        public int accessID { get; set; }
        public int roleID { get; set; }
        public int postID { get; set; }
        public string token { get; set; }
        public string mobileNumber { get; set; }
        public string loginKey { get; set; }

    }
}
