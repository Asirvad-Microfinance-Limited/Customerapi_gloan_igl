using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class DistrictRequest
    {
        #region Declaration

        [Required(ErrorMessage = Required.stateId)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.stateId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.stateId)]
        public long stateId { get; set; }

        #endregion Declaration
    }
}
