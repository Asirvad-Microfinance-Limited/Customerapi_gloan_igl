using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Request
{
    public class AadharDataRequest
    {
        [Required(ErrorMessage = "RRN is required")]
        public string rrN { get; set; }
    }
}
