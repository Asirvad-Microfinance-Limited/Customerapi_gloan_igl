using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static GlobalValues.GlobalVariables;
using static GlobalValues.CustomerValidationMessages;

namespace KYCAPI.V1.Models.Request
{
    public class ConsentRequest
    {
      

        [Required(ErrorMessage = Required.kycPhoto)]
        public string kycphoto { get; set; }

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

    }
    public class ConsentRequestlang
    {
        [Required(ErrorMessage = Required.language)]
        public int language { get; set; }
    }
}
