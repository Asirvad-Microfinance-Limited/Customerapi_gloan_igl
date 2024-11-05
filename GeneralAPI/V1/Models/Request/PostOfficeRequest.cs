
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class PostOfficeRequest
    {

        //[Required(ErrorMessage = Required.districtId)]
        //[Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.districtId)]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.districtId)]

        public long districtId { get; set; }
    }
}
