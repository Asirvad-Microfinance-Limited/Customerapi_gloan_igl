using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.Models.Request
{
    public class EKYCRequest
    {

        #region Declaration       

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }

       
        [Required(ErrorMessage = Required.UUID)]
        public string uuId { get; set; }
       
        [Required(ErrorMessage = Required.RRN)]
        public string rrn { get; set; }    


        #endregion Declaration

    }


    public class EKYCGetRequest
    {

        #region Declaration


        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        [StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        [MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custId { get; set; }    
      

        public string uuId { get; set; } 



        #endregion Declaration

    }
}
