using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class GeneralParameterRequest
    {

        #region Declarations

        [Required(ErrorMessage = Required.firmID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.firmID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.firmID)]
        public long firmId { get; set; }

        [Required(ErrorMessage = Required.parameterId)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.parameterId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.parameterId)]
        public long parameterId { get; set; }

        [Required(ErrorMessage = Required.moduleId)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.moduleId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.moduleId)]
        public long moduleId { get; set; }

        #endregion Declarations

    }
}
