using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class PinCodeRequest
    {

        #region Declarations

        [Required(ErrorMessage = Required.postOfficeId)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.postOfficeId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.postOfficeId)]
   
        public Int32 postOfficeId { get; set; }

        #endregion Declarations
    }
}
