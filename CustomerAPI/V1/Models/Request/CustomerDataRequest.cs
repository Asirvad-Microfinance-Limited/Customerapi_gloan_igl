using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.Models.Request
{
    public class CustomerDataRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        public string custId { get; set; }
    }
}
