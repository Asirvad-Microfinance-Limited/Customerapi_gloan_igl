using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace MobileAPI.V1.Models.Request
{
    public class MobileRequest
    {
        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.mobilenumberFormat, ErrorMessage = Invalid.mobileNumber)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobileNo { get; set; }
    }
}
