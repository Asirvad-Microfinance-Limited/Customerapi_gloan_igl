using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.Models.Request
{
    public class ResendLoginOTPRequest
    {
        public ResendLoginOTPRequest()
        {
        }

        [Required(ErrorMessage = "Employee id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "please use valid employee id")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid employee id")]
        public int empCode { get; set; }

        [Required(ErrorMessage = "login key is required")]
        public string loginKey { get; set; }

    }
}
