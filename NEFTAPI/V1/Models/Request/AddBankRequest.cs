using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace BankAPI.V1.Models.Request
{
    public class AddBankRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.branchID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.branchID)]
        public int branchID { get; set; }

        [Required(ErrorMessage = Required.firmID)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.firmID)]
        public int firmID { get; set; }


        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }

        [Required(ErrorMessage = Required.IFSCCode)]
        [RegularExpression(Constants.Strings.IfscCodeFormat, ErrorMessage = Invalid.IFSCCode)]
        public string IFSCCode { get; set; }


        [Required(ErrorMessage = Required.accountNo)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.accountNo)]
        public string accountNo { get; set; }


        [Required(ErrorMessage = Required.accountTypeName)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.accountTypeName)]
        public string accountType { get; set; }

        [Required(ErrorMessage = Required.custName)]
       // [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.custName)]
        public string custName { get; set; }


        [Required(ErrorMessage = Required.branchName)]
      //  [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.branchName)]
        public string branchName { get; set; }


        [Required(ErrorMessage = Required.mobileNo)]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mobileNo)]
        //[MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        //[MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        //[StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobileNo { get; set; }

    

        [Required(ErrorMessage = Required.neftPhoto)]
        public string neftPhoto { get; set; }
  

        #endregion Declarations

    }
}
