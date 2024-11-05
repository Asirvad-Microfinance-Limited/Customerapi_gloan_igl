using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;
using static GlobalValues.GlobalVariables.Constants;

namespace APIBaseClassLibrary.V1.Models.Request
{
   public class SmsRequest
    {
        public string ApiDetail { get; set; }

        [Required(ErrorMessage = Required.Message)]
        // [MinLength(Constants.Ints.messageLength, ErrorMessage = Invalid.messageLength)]
         [MaxLength(Constants.Ints.smsmessageLength, ErrorMessage = Invalid.MessageLength)]
        public string  Message { get; set; }

        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mobileNo)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobileNo { get; set; }
       

        [Required(ErrorMessage = Required.smsaccountType)]
        [Range(Constants.Ints.rangeValidatorFrom_1, Constants.Ints.zeroOneMaxValue, ErrorMessage = Invalid.smsaccountType)]
        public SMSAccMode accountType { get; set; }

        //[Required(ErrorMessage = Required.firmID)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.firmID)]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.firmID)]
        //public int firmID { get; set; }



    }
}
