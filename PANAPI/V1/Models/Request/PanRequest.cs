using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace PANAPI.V1.Models.Request
{
    public class GetPanRequest
    {

        #region Declarations

        [Required(ErrorMessage = Required.panNo)]
        [RegularExpression(Constants.Strings.panNoFormat, ErrorMessage = Invalid.panNo)]

        public string panNo { get; set; }

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]

        public string custId { get; set; }

        #endregion Declarations

    }

    public class PanDetailsRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]

        public string custId { get; set; }

        #endregion Declarations
    }
       
    public class UpdatePanDetailsRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

        [Required(ErrorMessage = Required.custType)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.custType)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custType)]
        public string custType { get; set; }

        [Required(ErrorMessage = Required.panNo)]
        [RegularExpression(Constants.Strings.panNoFormat, ErrorMessage = Invalid.panNo)]
        public string newPan { get; set; }

        [Required(ErrorMessage = Required.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.userID)]
        public string userId { get; set; }

        [Required(ErrorMessage = Required.panPhoto)]
        public string panPhoto { get; set; }

        #endregion Declarations

    }

        
     public class UpdateForm60Request
      {
        
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

        [Required(ErrorMessage = Required.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.userID)]
        public string userId { get; set; }

        [Required(ErrorMessage = "Required Form60 Image")]
        public string form60Image { get; set; }

       
    }

    public class Form60DeatilsRequest
    {
        #region Declarations

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]

        public string custId { get; set; }

        #endregion Declarations
    }

    
     public class SaveForm60DetailsRequest
    {

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

        [Required(ErrorMessage = "Required Amount of Transaction")]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = "Invalid Amount of Transaction")]
        public long amountOfTransaction { get; set; }
        
        [Required(ErrorMessage = "Required Transaction Mode")]
        public string transactionMode { get; set; }
        public string panApplyDate { get; set; }
        public string panAcknowledgementNo { get; set; }
        public long agriIncome { get; set; }
        public long OtherAgriIncome { get; set; }

        [Required(ErrorMessage = Required.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.userID)]
        public string userId { get; set; }
            

    }
}
