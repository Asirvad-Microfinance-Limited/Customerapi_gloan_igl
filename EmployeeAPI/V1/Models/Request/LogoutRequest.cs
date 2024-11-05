using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.V1.Models.Request
{
    public class LogoutRequest
    {
        [Required(ErrorMessage = Required.empCode)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.empCode)]
        public string employeeCode { get; set; }

        [Required(ErrorMessage = Required.token)]
        public string token { get; set; }
    }
}
