using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;

namespace CustomerAPI.V1.Models.Request
{
    public class OGLCustomerRequest
    {
        [Required(ErrorMessage = Required.custID)]
        // [Range(1, int.MaxValue, ErrorMessage = Invalid.empCode)]
        public string custID { get; set; }
    }
}
