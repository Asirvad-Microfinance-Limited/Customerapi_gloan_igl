using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.Models.Request
{
    public class DsaBaRequest
    {

        [Required(ErrorMessage = Required.code)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.code)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.code)]
        public long code { get; set; }

        [Required(ErrorMessage = Required.mediaId)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.mediaId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mediaId)]
        public long mediaId { get; set; }
    }
}
