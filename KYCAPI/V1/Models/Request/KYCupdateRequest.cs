using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.Models.Request
{
    public class KYCupdateRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

        [Required(ErrorMessage = Required.optFlag)]
        [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.optFlag)]

        public string as_flag { get; set; }

        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.userID)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.userID)]
        public string UserID { get; set; }


        public  string addproofID { get; set; }
        public  string param { get; set; }
    }
}
