using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.Models.Request
{
    public class GeneralStatusRequest
    {
        [Required(ErrorMessage = Required.moduleId)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.moduleId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.moduleId)]
        public long moduleId { get; set; }
        [Required(ErrorMessage = Required.optionId)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.optionId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.optionId)]
        public long optionId { get; set; }
    }
}
