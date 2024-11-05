using System.ComponentModel.DataAnnotations;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.Models.Request
{
    public class CustomerUpdateRequest
    {
        public CustomerUpdateRequest() { }

        // production bug- in customer modification some fields madatory removed. (monthername, pep, marital sttaus, validation of mothername etc)

        //[Required(ErrorMessage = Required.firmID)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.firmID)]
        //public int firmID { get; set; }

        //[Required(ErrorMessage = Required.branchID)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.branchID)]
        //public int branchID { get; set; }

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custID)]
        public string custID { get; set; }

        [Required(ErrorMessage = Required.fatHusName)]
     //   [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.fatHusName)]
        public string fatHusName { get; set; }

        [Required(ErrorMessage = Required.custName)]
       // [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.custName)]
        public string custName { get; set; }

        [Required(ErrorMessage = Required.houseName)]
       // [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.houseName)]
        public string houseName { get; set; }
        [Required(ErrorMessage = Required.location)]
        public string location { get; set; }

        [Required(ErrorMessage = Required.pinsrl)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.pinsrl)]
        [StringLength(Constants.Ints.pinSerialLength, ErrorMessage = Invalid.pinsrl)]
        public string pinsrl { get; set; }

        [Required(ErrorMessage = Required.isActive)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.isActive)]
        public int cust_Type { get; set; } // cust type in design
        [Required(ErrorMessage = Required.occupation)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 100, ErrorMessage = Invalid.occupation)]
        public int occupation { get; set; }

        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.phoneno)]
        //[MinLength(Constants.Ints.landLineNumberMinLength, ErrorMessage = Invalid.landLineNoLength)]
        //[MaxLength(Constants.Ints.landLineNumberMaxLength, ErrorMessage = Invalid.landLineNoLength)]
        public string phoneno { get; set; }

        [Required(ErrorMessage = Required.mobileNo)]
        //[RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.mobileNo)]
       // [StringLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        //[MinLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        //[MaxLength(Constants.Ints.mobileNumberLength, ErrorMessage = Invalid.mobileNoLength)]
        public string mobileNo { get; set; }
        public string loyaltyCardNo { get; set; }

        [Required(ErrorMessage = Required.kycIDType)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.kycIDType)]
        public int kycIDType { get; set; }

        [Required(ErrorMessage = Required.kycIdNo)]
        public string kycIdNo { get; set; }

       // [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string kycIssueDate { get; set; }

       // [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string kycExpiryDate { get; set; }
        public string kycIssuePlace { get; set; }

        [Required(ErrorMessage = Required.dob)]
        [DisplayFormat(DataFormatString = Constants.Strings.dateFormat)]
        public string dob { get; set; }
        public string modificationDescription { get; set; }
        //[Required(ErrorMessage = Required.media)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.media)]
        public int mediaID { get; set; }

        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.moduleID)]
        public int moduleID { get; set; }

        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.exServiceStatus)]
        public int exServiceStatus { get; set; }
        public string exServiceNo { get; set; }

        //[Required(ErrorMessage = Required.empCode)]
        //[Range(1, int.MaxValue, ErrorMessage = Invalid.empCode)]
        public int empCode { get; set; }


        //[Required(ErrorMessage = Required.empName)]
        //[RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.empName)]
        public string empName { get; set; }

        [Required(ErrorMessage = Required.mediaType)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.mediaType)]
        public int mediaTypeID { get; set; }
        [Required(ErrorMessage = Required.houseName)]
        public string alt_Housename { get; set; }
        [Required(ErrorMessage = Required.location)]
        public string alt_Locality { get; set; }
        [Required(ErrorMessage = Required.pinsrl)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.pinsrl)]
        [StringLength(Constants.Ints.pinSerialLength, ErrorMessage = Invalid.pinsrl)]

        public string alt_Pin { get; set; }

        [Required(ErrorMessage = Required.empCode)]
        [Range(1, int.MaxValue, ErrorMessage = Invalid.empCode)]
        public int updating_empCode { get; set; }

        [Required(ErrorMessage = Required.kycOf)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 2, ErrorMessage = Invalid.custType)]
        public int kycOf { get; set; }

        [Required(ErrorMessage = Required.gender)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.gender)]
        public int gender { get; set; }

        //[Range(Constants.Ints.rangeValidatorFrom_1, 1000, ErrorMessage = Invalid.prefLang)]
        public int prefLang { get; set; }

      //  [Required(ErrorMessage = Required.pep)]
      //  [Range(Constants.Ints.rangeValidatorFrom_1, 100, ErrorMessage = Invalid.pep)]
        public int pep { get; set; }

      //  [Required(ErrorMessage = Required.MotFname)]
       // [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.MotFname)]

        public string mothername { get; set; }
     //   [Required(ErrorMessage = Required.marital)]
      //  [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.marital)]
        public int marital { get; set; }

     //   [Required(ErrorMessage = Required.citizen)]
       //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.citizen)]
        public int citizen { get; set; }

       // [Required(ErrorMessage = Required.nationality)]
      //  [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.nationality)]
        public int nationality { get; set; }

       // [Required(ErrorMessage = Required.residentialStatus)]
     //   [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.residentialStatus)]
        public int residentialStatus { get; set; }
       [Required(ErrorMessage = Required.preName)]
        [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.preName)]
        public int preName { get; set; }
        [Required(ErrorMessage = Required.relationIdentity)]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.relationIdentity)]
        public int relationIdentity { get; set; }


        //[EmailAddress(ErrorMessage = Invalid.email)]
        //[RegularExpression(Constants.Strings.emailFormat, ErrorMessage = Invalid.email)]
        public string email { get; set; }

               //[Required(ErrorMessage = Required.religion)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, 100, ErrorMessage = Invalid.religion)]
        public int religion { get; set; }

        //[Required(ErrorMessage = Required.caste)]
       //[Range(Constants.Ints.rangeValidatorFrom_0, 100, ErrorMessage = Invalid.caste)]
        public int caste { get; set; }

        //[Required(ErrorMessage = Required.qualification)]
       // [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.qualification)]
        public int qualification { get; set; }


        //[Required(ErrorMessage = Required.loanPurpose)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.loanPurpose)]
        public int loanPurpose { get; set; }


        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.income)]
        public int income { get; set; }


        //[Required(ErrorMessage = Required.firstGL)]
       // [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.firstGL)]
        public int firstGL { get; set; }

        //---------------------------








      

       

        

       

        
       

        [Required(ErrorMessage = Required.fatHus_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.fatHus_Flag)]
        public int fatHus_Flag { get; set; }

        [Required(ErrorMessage = Required.house_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.house_Flag)]
        public int house_Flag { get; set; }

        [Required(ErrorMessage = Required.locality_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.locality_Flag)]
        public int locality_Flag { get; set; }

        [Required(ErrorMessage = Required.pinSerial_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.pinSerial_Flag)]
        public int pinSerial_Flag { get; set; }

        [Required(ErrorMessage = Required.cust_Status_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.cust_Status_Flag)]
        public int cust_Status_Flag { get; set; }

        [Required(ErrorMessage = Required.occupation_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.occupation_Flag)]
        public int occupation_Flag { get; set; }

        [Required(ErrorMessage = Required.phone1_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.phone1_Flag)]
        public int phone1_Flag { get; set; }

        [Required(ErrorMessage = Required.phone2_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.phone2_Flag)]
        public int phone2_Flag { get; set; }

        [Required(ErrorMessage = Required.loyaltiCardno_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.loyaltiCardno_Flag)]
        public int loyaltiCardno_Flag { get; set; }

        [Required(ErrorMessage = Required.kyc_Type_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.kyc_Type_Flag)]
        public int kyc_Type_Flag { get; set; }

        [Required(ErrorMessage = Required.key_Id_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.key_Id_Flag)]
        public int key_Id_Flag { get; set; }

        [Required(ErrorMessage = Required.kyc_Issue_Place_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.kyc_Issue_Place_Flag)]
        public int kyc_Issue_Place_Flag { get; set; }

        [Required(ErrorMessage = Required.descr_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.descr_Flag)]
        public int descr_Flag { get; set; }

        [Required(ErrorMessage = Required.street_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.street_Flag)]
        public int street_Flag { get; set; }

        [Required(ErrorMessage = Required.cust_Name_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.cust_Name_Flag)]
        public int cust_Name_Flag { get; set; }

        [Required(ErrorMessage = Required.email_Flag)]
        [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.email_Flag)]
        public int email_Flag { get; set; }

        public string facebookID { get; set; }

        public long addressProof_Type { get; set; }

        public string addressProof_Billdate { get; set; }

        public long addressUpdation_Flag { get; set; }

        public long businessCategory { get; set; }
        public long businessSubCategory { get; set; }

        public string CKYCNumber { get; set; }
        public string weddingDate { get; set; }
        public string maritalStatus { get; set; }
        //[Required(ErrorMessage = Required.RRN)]
        //public string RRN { get; set; }

        //[Required(ErrorMessage = Required.UUID)]
        //public string UUID { get; set; }


        //[Required(ErrorMessage = Required.countryID)]
        //[Range(1, int.MaxValue, ErrorMessage = Invalid.countryID)]
        //public int country { get; set; }


        //[Required(ErrorMessage = Required.custType)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.custType)]
        //public int custStatus { get; set; } // cust status in design


        //[Required(ErrorMessage = Required.kycType)]
        //[Range(Constants.Ints.rangeValidatorFrom_1, 10, ErrorMessage = Invalid.kycType)]
        //public int kycType { get; set; }

        //[Required(ErrorMessage = Required.mediaType)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.mediaType)]
        //public int mediaType { get; set; }

        //[Required(ErrorMessage = Required.address_Flg)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.address_Flg)]
        //public int address_Flg { get; set; }

        //[Required(ErrorMessage = Required.customerCategory)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.customerCategory)]
        //public int customerCategory { get; set; }

        //public string neftData { get; set; }
        //public int servType { get; set; }
        //public string reason { get; set; }
        //public string share { get; set; }
        //public string panNo { get; set; }
        //public string landHldtl { get; set; }
        //public string baCode { get; set; }
        //public string kycRemark { get; set; }
        //public string photoRemark { get; set; }
        ////public string modi_key { get; set; }



        //// unused variables
        ////public int state { get; set; }
        ////public int district { get; set; }
        ////public string post { get; set; }
        ////public int custStatus { get; set; }
        ////public string phcode { get; set; }
        ////public string custStatusNew { get; set; }
        ////public int loan { get; set; }

    }
}
