using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.Models.Request
{
    public class KYCRequest
    {

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }


        [Required(ErrorMessage = Required.kycPhoto)]
        public string kycPhoto { get; set; }

        [Required(ErrorMessage = Required.branchID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.branchID)]
        public long branchID { get; set; }

        [Required(ErrorMessage = Required.userID)]
         [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.userID)]
        public long userID { get; set; }

      
       // [Required(ErrorMessage = Required.kycType)]
      //  [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.kycType)]

        public string kycType { get; set; }

       // [Required(ErrorMessage = Required.kycID)]
        //[Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.kycIDType)]
        public string kycID { get; set; }

        public CheckStatus chkAddress { get; set; }

        public CheckStatus chkSameAsId { get; set; }
        public CheckStatus Addressboxview { get; set; }

        public long AddressProf { get; set; }

    }


    public class VisaRequest
    {

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }

        [Required(ErrorMessage ="Required Visa number")]
        public string visaNumber { get; set; }

        [Required(ErrorMessage = "Required Visa date")]
        [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string visaIssueDate { get; set; }

        [Required(ErrorMessage = "Required Visa expiry date")]
        [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string visaExpiryDate { get; set; }
                
        public string Country1 { get; set; }
        public string Country2 { get; set; }
        public string Country3 { get; set; }

        [Required(ErrorMessage = "Required Visa document")]
        public string visaDocument { get; set; }

        
    }

    public class visadetailsGetRequest
    {

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }

    }

    public class AadharConsentRequest
    {

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }

        [Required(ErrorMessage = "Required Consent")]
        public string aadharConsent { get; set; }

        public string userID { get; set; }


    }

    public class UploadAddressproofRequest
    {

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }

        public int addressProof_Type { get; set; }

        public string BillDate { get; set; }
        
        [Required(ErrorMessage = "Required Addressproof document")]
        public string addressProofDocument { get; set; }

        public string userID { get; set; }


    }

    public class KycSelfCertifyRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }
        public string empCode { get; set; }
        public string SelfCertifyImage { get; set; }
    }
}
