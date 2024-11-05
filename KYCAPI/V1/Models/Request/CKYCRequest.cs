using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace KYCAPI.V1.Models.Request
{
    public class CKYCRequest
    {
        [Required(ErrorMessage = Required.CustFName)]
        [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.CustFName)]
        public string CustFName { get; set; }

        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        public string custID { get; set; }

        //[Required(ErrorMessage = "CustMName is required")]
        public string CustMName { get; set; }

        //[Required(ErrorMessage = "CustLName is required")]
        public string CustLName { get; set; }


        [Required(ErrorMessage = Required.FatSpouse)]
     //   [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.FatSpouse)]
        public int FatSpouse { get; set; }

        [Required(ErrorMessage = Required.FatPfx)]
      //  [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.FatPfx)]
        public int FatPfx { get; set; }


        [Required(ErrorMessage = Required.FatFname)]
      //  [RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.FatFname)]
        public string FatFname { get; set; }

        //[Required(ErrorMessage = "FatMname is required")]
        public string FatMname { get; set; }

        //[Required(ErrorMessage = "FatMname is required")]
        public string FatLname { get; set; }
        [Required(ErrorMessage = Required.CustPfx)]
      //  [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.CustPfx)]
        public int CustPfx { get; set; }

        //[Required(ErrorMessage = Required.MotPfx)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.MotPfx)]
        public int MotPfx { get; set; }

        //[Required(ErrorMessage = Required.MotFname)]
        //[RegularExpression(Constants.Strings.nameFormat, ErrorMessage = Invalid.MotFname)]
        public string MotFname { get; set; }


       // [Required(ErrorMessage = "MotMname is required")]
        public string MotMname { get; set; }

       // [Required(ErrorMessage = "MotLname is required")]
        public string MotLname { get; set; }


        //[Required(ErrorMessage = Required.JurisRes)]
       // [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.JurisRes)]
        public int JurisRes { get; set; }

        //[Required(ErrorMessage = Required)]
        public string TaxId { get; set; }

       // [Required(ErrorMessage = Required.BirCtry)]
       // [Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.BirCtry)]

        public int BirCtry { get; set; }

        //[Required(ErrorMessage = Required.AddrTyp)]
        //[Range(Constants.Ints.rangeValidatorFrom_0, 10, ErrorMessage = Invalid.AddrTyp)]

        public int AddrTyp { get; set; }

        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.userID)]
       // [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.userID)]
        public long UserID { get; set; }

        [Required(ErrorMessage = Required.UserFlg)]
       // [Range(Constants.Ints.rangeValidatorFrom_0, Constants.Ints.rangeValidatorFrom_1, ErrorMessage = Invalid.UserFlg)]

        public int UserFlg { get; set; }
       

    }
}
