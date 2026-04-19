using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.Models.Request
{
    public class CustomerProfileRequest
    {
        public string AgentID { get; set; }
        public string Mobile { get; set; }
        public string SessionID { get; set; }
        public string Amount { get; set; }
    }
}
