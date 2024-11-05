using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.Models.Request
{
    public class PawnBrokerRequest
    {

        #region Declarations

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

        [Required(ErrorMessage = Required.identityId)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.identityId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.identityId)]
        public int identityId { get; set; }

        #endregion Declarations
    }
}
