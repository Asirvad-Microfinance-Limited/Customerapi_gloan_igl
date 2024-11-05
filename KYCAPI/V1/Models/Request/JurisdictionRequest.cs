using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KYCAPI.V1.Models.Request
{
    public class JurisdictionRequest
    {
        [Required(ErrorMessage = "Branch id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "please use valid branch id")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid branch id")]
        public long branchId { get; set; }
    }
}
