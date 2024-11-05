using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace EmployeeAPI.V1.Models.Request
{
    public class VerifyLoginOTPRequest
    {
        public VerifyLoginOTPRequest() { }

        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mobileNo)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string moblieNo { get; set; }

        [Required(ErrorMessage = Required.otp)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.otp)]
        public string otpno { get; set; }

        [Required(ErrorMessage = Required.loginKey)]
        public string loginkey { get; set; }

        [Required(ErrorMessage = Required.token)]
        public string token { get; set; }

        [Required(ErrorMessage = Required.signature)]
        public string signature { get; set; }

    }
}
