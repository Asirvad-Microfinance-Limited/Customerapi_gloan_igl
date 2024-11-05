using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.Models.Request
{
    public class userLogin
    {
        public string employeeID { get; set; }
        public string password { get; set; }
        public string formMode { get; set; }
        public string branchID { get; set; }
        public string siganture { get; set; }
    }
}
