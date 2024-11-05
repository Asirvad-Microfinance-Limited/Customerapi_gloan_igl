using CustomerAPI.V1.Models.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.Models.Request
{
    public class CustomerAddRequest
    {
        public CustomerAddRequest() { }

        [Required(ErrorMessage = Required.firmID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.firmID)]
        public int firmID { get; set; }

        [Required(ErrorMessage = Required.branchID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.branchID)]
        public int branchID { get; set; }

        [Required(ErrorMessage = Required.empCode)]
      // [Range(1, int.MaxValue, ErrorMessage = Invalid.empCode)]
        public string empCode { get; set; }

        //[Required(ErrorMessage = Required.RRN)]
        public string RRN { get; set; }

        //[Required(ErrorMessage = Required.UUID)]
        public string UUID { get; set; }

        [Required(ErrorMessage = Required.preName)]
        [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.preName)]
        public int preName { get; set; }

        [Required(ErrorMessage = Required.custName)]
        [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.custName)]
        public string custName { get; set; }

        [Required(ErrorMessage = Required.gender)]
        [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.gender)]
        public int gender { get; set; }

        [Required(ErrorMessage = Required.relationIdentity)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.relationIdentity)]
        public int relationIdentity { get; set; }

        [Required(ErrorMessage = Required.fatHusName)]
        [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.fatHusName)]
        public string fatHusName { get; set; }

        [Required(ErrorMessage = Required.MotFname)]
        [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.MotFname)]
        public string mothername { get; set; }

        [Required(ErrorMessage = Required.houseName)]
        //[RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.houseName)]
        public string houseName { get; set; }

        [Required(ErrorMessage = Required.location)]
        public string location { get; set; }

        [Required(ErrorMessage = Required.countryID)]
        [Range(1, int.MaxValue, ErrorMessage = Invalid.countryID)]
        public int countryID { get; set; }

        [Required(ErrorMessage = Required.pinsrl)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.pinsrl)]
        [StringLength(Constants.Ints.pinSerialLength, ErrorMessage = Invalid.pinsrl)]
        public string pinsrl { get; set; }

        [Required(ErrorMessage = Required.citizen)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.citizen)]
        public int citizen { get; set; }

        [Required(ErrorMessage = Required.nationality)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.nationality)]
        public int nationality { get; set; }

        [Required(ErrorMessage = Required.residentialStatus)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.residentialStatus)]
        public int residentialStatus { get; set; }

        [Required(ErrorMessage = Required.dob)]
        [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string dob { get; set; }

        //[EmailAddress(ErrorMessage = Invalid.email)]
        //[RegularExpression(Constants.Strings.emailFormat, ErrorMessage = Invalid.email)]
        public string email { get; set; }

        [Required(ErrorMessage = Required.custStatus)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.custStatus)]
        public int custStatus { get; set; } // cust status in design

        //[Required(ErrorMessage = Required.qualification)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.qualification)]
        public int qualification { get; set; }

        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.phoneno)]
        //[MinLength(Constants.Ints.landLineNumberMinLength, ErrorMessage = Invalid.landLineNoLength)]
        //[MaxLength(Constants.Ints.landLineNumberMaxLength, ErrorMessage = Invalid.landLineNoLength)]
      //  [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.landLineNumberMaxLength, ErrorMessage = Invalid.landLineNoLength)]
        public string phoneno { get; set; }

        [Required(ErrorMessage = Required.mobileNo)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mobileNo)]
        [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        [MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobileNo { get; set; }



        [Required(ErrorMessage = Required.kycOf)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 2, ErrorMessage = Invalid.kycOf)]
        public int kycOf { get; set; }

       // [Range(Constants.Ints.rangeValidatorFrom_1, 1000, ErrorMessage = Invalid.prefLang)]
        public int prefLang { get; set; }

        [Required(ErrorMessage = Required.kycType)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 10, ErrorMessage = Invalid.kycType)]
        public int kycType { get; set; }

        [Required(ErrorMessage = Required.kycIDType)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.kycIDType)]
        public int kycIDType { get; set; }

        [Required(ErrorMessage = Required.kycIdNo)]
        public string kycIdNo { get; set; }

        [Required(ErrorMessage = Required.ckycId)]
        public string ckycId { get; set; }

        //[DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string kycIssueDate { get; set; }

       // [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string kycExpiryDate { get; set; }
        public string kycIssuePlace { get; set; }




        [Required(ErrorMessage = Required.mediaType)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.mediaType)]
        public int mediaTypeID { get; set; }

        //[Required(ErrorMessage = Required.media)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.media)]
        public int mediaID { get; set; }


        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.moduleID)]
        public int moduleID { get; set; }

        //[Required(ErrorMessage = Required.religion)]
      //  [Range(Constants.Ints.rangeValidatorFrom_1, 100, ErrorMessage = Invalid.religion)]
        public int religionID { get; set; }

        //[Required(ErrorMessage = Required.caste)]
      //  [Range(Constants.Ints.rangeValidatorFrom_1, 100, ErrorMessage = Invalid.caste)]
        public int casteID { get; set; }

        [Required(ErrorMessage = Required.occupation)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 100, ErrorMessage = Invalid.occupation)]
        public int occupationID { get; set; }

        [Required(ErrorMessage = Required.pep)]
        [Range(Constants.Ints.rangeValidatorFrom_0, 100, ErrorMessage = Invalid.pep)]
        public int pep { get; set; }

        public string loyaltyCardNo { get; set; }

        [Required(ErrorMessage = Required.custType)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.custStatus)]
        public int cust_Type { get; set; } // cust type in design

        [Required(ErrorMessage = Required.isActive)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.isActive)]
        public int isActive { get; set; }

        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.income)]
        public int income { get; set; }

        //[Required(ErrorMessage = Required.firstGL)]
       // [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.firstGL)]
        public int firstGL { get; set; }

        [Required(ErrorMessage = Required.marital)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.marital)]
        public int marital { get; set; }

        //[Required(ErrorMessage = Required.loanPurpose)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.loanPurpose)]
        public int loanPurpose { get; set; }


        [Required(ErrorMessage = Required.address_Flg)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.address_Flg)]
        public int address_Flg { get; set; }
        [Required(ErrorMessage = Required.houseName)]
        public string alt_Housename { get; set; }
        [Required(ErrorMessage = Required.location)]
        public string alt_Locality { get; set; }

        [Required(ErrorMessage = Required.pinsrl)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.pinsrl)]
        [StringLength(Constants.Ints.pinSerialLength, ErrorMessage = Invalid.pinsrl)]

        public string alt_Post { get; set; }

        
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.exServiceStatus)]
        public int exServiceStatus { get; set; }
        public string exServiceNo { get; set; }


        [Required(ErrorMessage = Required.customerCategory)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.customerCategory)]
        public int customerCategory { get; set; }


        public string neftData { get; set; }
        public int CustSource { get; set; }
        public int reason { get; set; }
        public string shareFlag { get; set; }

        [RegularExpression(Constants.Strings.panNoFormat, ErrorMessage = Invalid.panNo)]
        [StringLength(Constants.Ints.panNumberLength, ErrorMessage = Invalid.panNoLength)]
        //[MinLength(Constants.Ints.panNumberLength, ErrorMessage = Invalid.panNoLength)]
        [MaxLength(Constants.Ints.panNumberLength, ErrorMessage = Invalid.panNoLength)]
        public string panNo { get; set; }

        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.landDtl)]
        public int landDtlID { get; set; }

        public string baCode { get; set; }
        public string kycRemark { get; set; }
        public string photoRemark { get; set; }

        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.vehicleloan)]
        //[MaxLength(Constants.Ints.vehicleLoanNumberLength, ErrorMessage = Invalid.vehicleLoanNumberLength)]
        public string vehicleloan { get; set; }

        [Required(ErrorMessage = Required.customerImageFlag)]
        public bool customerImageFlag { get; set; } // cust type in design

        public string facebookID { get; set; }

        public long addressProof_Type { get; set; }

        public string addressProof_Billdate { get; set; }

        public long businessCategory { get; set; }
        public long businessSubCategory { get; set; }
        public string CKYCNumber { get; set; }
        public string weddingDate { get; set; }
        public string maritalStatus { get; set; }

        // unused variables
        //public int state { get; set; }
        //public int district { get; set; }
        //public string post { get; set; }
        //public int custStatus { get; set; }
        //public string phcode { get; set; }
        //public string custStatusNew { get; set; }
        //public int loan { get; set; }

    }


    public class CustomerReferenceDetailsRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        //[MaxLength(Constants.Ints.customerIDLengthD:\Asirvad-GL-TFS\CustomerAPISolution\CustomerAPI\V1\Models\Request\DuplicateSearchCustomerRequest.cs, ErrorMessage = Invalid.custID)]
        public string custId { get; set; }

        //[Required(ErrorMessage = Required.strFlag)]     
        //public string strFlag { get; set; }

        [Required(ErrorMessage = Required.empCode)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.empCode)]
        public string empCode { get; set; }

       // [Required(ErrorMessage = Required.strMobile)]
        public string strMobile { get; set; }
       // [Required(ErrorMessage = Required.strOTP)]
        public string strOTP { get; set; }

        public List<CustomerReferenceDetailsProperties> refDetails { get; set; }

     
    }

    public class UpdatePEPDetailsRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        [StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        [MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        [MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        public string custId { get; set; }
       
        [Required(ErrorMessage = "RequiredPEPId")]
        public long PEPId { get; set; }
        
    }
}
