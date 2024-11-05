using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.CustomerValidationMessages;
using static GlobalValues.GlobalVariables;

namespace MobileAPI.V1.Models.Request
{
    public class EMailRequest
    {
        [Required]
        public string toAddress { get; set; }
        [Required]
        public string subject { get; set; }
        [Required]
        public string message { get; set; }
    }

    

    public class SaveEmailOTPRequest
    {
        [Required]
        public string custID { get; set; }
        [Required]
        public string emailID { get; set; }
        [Required]
        public string otp { get; set; }

        [Required]
        public long  userID { get; set; }
    }

    public class SendEmailOTPRequest
    {
        public string emailID { get; set; }


    }
}
