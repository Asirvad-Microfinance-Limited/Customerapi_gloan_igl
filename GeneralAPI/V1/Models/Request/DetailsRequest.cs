using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class DetailsRequest
    {

        #region Declarations

        [Required(ErrorMessage = Required.pinCode)]
        [Range(Constants.Ints.pinSerialLength, int.MaxValue, ErrorMessage = Invalid.pinCode)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.pinCode)]

        public long pinCode { get; set; }

        #endregion Declarations


    }
}
