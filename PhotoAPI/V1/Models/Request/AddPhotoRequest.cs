using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace PhotoAPI.V1.Models.Request
{
    public class AddPhotoRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]

        public string custId { get; set; }

        [Required(ErrorMessage = Required.status)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.status)]
        public int status { get; set; }

        [Required(ErrorMessage = Required.custPhoto)]
        public string custPhoto { get; set; }
    }


    public class UpdatePhotoRequest
    {
        [Required(ErrorMessage = Required.custID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.custID)]
        //[StringLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MinLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]
        //[MaxLength(Constants.Ints.customerIDLength, ErrorMessage = Invalid.custIdlength)]

        public string custId { get; set; }

        [Required(ErrorMessage = Required.status)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.status)]
        public int branchId { get; set; }

        [Required(ErrorMessage = Required.custPhoto)]
        public string custPhoto { get; set; }

        [Required(ErrorMessage = Required.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.status)]
        public string userId { get; set; }

    }
}
