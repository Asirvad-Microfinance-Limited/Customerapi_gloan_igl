using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Request
{
    public class AddAadhaarRequest
    {
        [Required(ErrorMessage = "Branch id is required")]
        public string branchId { get; set; }
        public string aadhaarId { get; set; }

        public string vMode { get; set; }
        public string eMode { get; set; }
        public string eCode { get; set; }
        public string eTxn { get; set; }
        public string eTs { get; set; }
        public string retval { get; set; }
        public string photo { get; set; }
        public string eStatus { get; set; }
        public string userId { get; set; }
        public string custDtl { get; set; }
        [Required(ErrorMessage = "rrn_n is required")]
        public string rrnN { get; set; }
    }
}
