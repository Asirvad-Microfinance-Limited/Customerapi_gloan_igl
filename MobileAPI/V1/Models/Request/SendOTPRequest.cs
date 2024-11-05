using GlobalValues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace MobileAPI.V1.Models.Request
{
    public class SendOTPRequest
    {
        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.mobilenumberFormat, ErrorMessage = Invalid.mobileNumber)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobile { get; set; }


        [Required(ErrorMessage = Required.confirmMobileNo)]
        [RegularExpression(Constants.Strings.mobilenumberFormat, ErrorMessage = Invalid.mobileNumber)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.confirmMobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.confirmMobileNoLength)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.confirmMobileNoLength)]
        [CompareAttribute("mobile", ErrorMessage = OtpValidationMessages.numbersNotMatching)]
        public string confirmMobile { get; set; }


        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.type)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.type)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.type)]
        public int type { get; set; }



        //[Required(ErrorMessage = Required.empCode)]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.empCode)]
        //[StringLength(Constants.Ints.empCodeLength, ErrorMessage = Invalid.empCodelength)]
        //[MinLength(Constants.Ints.empCodeminLength, ErrorMessage = Invalid.empCodelength)]
        //[MaxLength(Constants.Ints.empCodeLength, ErrorMessage = Invalid.empCodelength)]
        public string employeeCode { get; set; }

        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.type)]    
        public FormModeMobile formMode { get; set; }

        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mobileNumber)]
        //[MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        //[MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        //[StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string oldMobile { get; set; }


    }
}
