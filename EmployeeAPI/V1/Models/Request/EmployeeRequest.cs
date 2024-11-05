using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.V1.Models.Request
{
    public class EmployeeRequest
    {
        public EmployeeRequest() { }

        [Range(1, int.MaxValue, ErrorMessage = "please use valid employee id")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid employee id")]
        public int employeeID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "please use valid branch id")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid branch id")]
        public int branchID { get; set; }

    }

    public class EmployeeBranchRequest
    {


        [Required(ErrorMessage = "HostName is required")]
        public string hostName { get; set; }

        [Required(ErrorMessage = "MacAddress is required")]
        public string macAddress { get; set; }

    }
}
