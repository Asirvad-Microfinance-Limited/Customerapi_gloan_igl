using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace MobileAPI.V1.Models.Request
{
    public class VerifyOTPRequest
    {
        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.mobilenumberFormat, ErrorMessage = Invalid.mobileNumber)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobile { get; set; }

        [Required(ErrorMessage = Required.otp)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.otp)]
        public string otp { get; set; }

        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.checkOtp)]
        public OTPFlags checkOTP { get; set; }

        [Required(ErrorMessage = Required.empCode)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.empCode)]
        //[StringLength(Constants.Ints.empCodeLength, ErrorMessage = Invalid.empCodelength)]
        //[MinLength(Constants.Ints.empCodeminLength, ErrorMessage = Invalid.empCodelength)]
        //[MaxLength(Constants.Ints.empCodeLength, ErrorMessage = Invalid.empCodelength)]
        public string empCode { get; set; } // validationKey = empcode for OTPFlags.EmployeeCode ; validationKey = LoginKey for OTPFlags.LoginOTP 

       
       
    }
}
