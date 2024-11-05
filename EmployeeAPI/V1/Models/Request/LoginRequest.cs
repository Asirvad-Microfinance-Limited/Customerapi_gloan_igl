using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.Models.Request
{
    public class LoginRequest
    {
        public LoginRequest()
        {
             bio_flag = 0;
        }

        [Required(ErrorMessage = "Employee id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "please use valid employee id")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid employee id")]
        public int employeeID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        [Required(ErrorMessage = "Form Mode is required")]
        [Range(1, (double)LoginFormMode.MaxValue, ErrorMessage = "please use valid employee id")]
        public LoginFormMode formMode { get; set; }

        [Required(ErrorMessage = Required.branchID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.branchID)]
        public int branchID { get; set; }

        [Required(ErrorMessage = Required.siganture)]
        public string siganture { get; set; }
                 
         public int bio_flag { get; set; }//biometric  CRF 70009507

    }
}
