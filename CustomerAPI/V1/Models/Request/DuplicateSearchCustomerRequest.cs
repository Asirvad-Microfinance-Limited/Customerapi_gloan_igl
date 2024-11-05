using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomerAPI.V1.Models.Request
{
    public class DuplicateSearchCustomerRequest
    {
        public string custID { get; set; }               
        public string kycNumber { get; set; }
        public int kycType { get; set; }
        public int pinSerial { get; set; }
        public int custype { get; set; }
    }

    public class matchingCustomerRequest
    {
        public string custName { get; set; }
        public string mobileNo { get; set; }
        public string fatherPrename { get; set; }
        public string fatherName { get; set; }
        public string houseName { get; set; }
        public string street { get; set; }
        public string location { get; set; }
        public string pinSerial { get; set; }
        public string kycIdNo { get; set; }
        public string dob { get; set; }
        public long kycIdType { get; set; }
    }
}
