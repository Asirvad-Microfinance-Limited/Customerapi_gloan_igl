using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;
namespace CustomerAPI.V1.Models.Request
{
    public class SearchCustomerRequest
    {
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.searchValue)]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Enter valid search value")]
        public string searchValue { get; set; }
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.userID)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 9999999, ErrorMessage = Invalid.userID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.userID)]
        public long userId { get; set; }
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.branchID)]
        [Range(Constants.Ints.rangeValidatorFrom_0, 50000, ErrorMessage = Invalid.branchID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.branchID)]
        public long branchId { get; set; }
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.type)]
        [Range(Constants.Ints.rangeValidatorFrom_1, 7, ErrorMessage = Invalid.type)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.type)]
        public long type { get; set; }

        public string moduleId  { get; set; }
    }
}
