using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Request
{
    public class ConfigInsertRequest
    {
        [Required]
        public string configName { get; set; }

    }
    public class ConfigUpdateRequest
    {
        [Required]
        [Range(Constants.Ints.rangeValidatorFrom_1, int.MaxValue, ErrorMessage = Invalid.configId)]
        public long configId { get; set; }
        [Required]
        public string configName { get; set; }
        [Required]
        [Range(Constants.Ints.rangeValidatorFrom_0, int.MaxValue, ErrorMessage = Invalid.configStatus)]
        public long configStatus { get; set; }

    }

  
}
