using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace CustomerAPI.V1.Models.Properties
{
    public class CustomerReferenceDetailsProperties
    {
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.relationType)]
        public string relationType { get; set; }
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.referenceName)]
        public string referenceName { get; set; }
        [Required(ErrorMessage = GlobalValues.CustomerValidationMessages.Required.referenceMobile)]
        public string referenceMobile { get; set; }

        
    }
}
