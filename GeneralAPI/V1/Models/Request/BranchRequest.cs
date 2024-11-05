using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class BranchRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.branchID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.branchID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.branchID)]
        public long branchId { get; set; }

        #endregion Declarations

    }
}
