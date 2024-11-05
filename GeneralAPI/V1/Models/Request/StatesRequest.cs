using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class StatesRequest
    {

        [Required(ErrorMessage = Required.countryID)]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.countryID)]
        [RegularExpression(Constants.Strings.numberFormat, ErrorMessage = Invalid.countryID)]
      
        public long countryId { get; set; }
    }
}
