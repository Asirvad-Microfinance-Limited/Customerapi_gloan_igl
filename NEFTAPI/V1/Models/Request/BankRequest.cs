using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace BankAPI.V1.Models.Request
{
    public class BankRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.IFSCCode)]
        [RegularExpression(Constants.Strings.IfscCodeFormat, ErrorMessage = Invalid.IFSCCode)]
        public string IFSC_Code { get; set; }

        #endregion Declarations

    }

    public class BankRequestCust
    {
        #region Declarations

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        public string custID { get; set; }

        #endregion Declarations
    }

    public class BankFillRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.districtId)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.districtId)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.districtId)]

        public long districtId { get; set; }

        #endregion Declarations

    }

    public class CustomerBankPhotoRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.custID)]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        public string custID { get; set; }

        #endregion Declarations
    }

}
