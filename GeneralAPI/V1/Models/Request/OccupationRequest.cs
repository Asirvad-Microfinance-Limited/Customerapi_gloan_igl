using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class OccupationSubCategoryRequest

    {
        #region Declaration

        //[Required(ErrorMessage = "Required Occupation ID")]
        //[Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = "Required Occupation ID")]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = "Required Occupation ID")]
        public long OccupationId { get; set; }

        public long CategoryId { get; set; }

        #endregion Declaration
    }
}
