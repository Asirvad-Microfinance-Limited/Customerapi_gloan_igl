using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.Models.Request
{
    public class AgentLoginRequest
    {
        public string agentID { get; set; }
        public string password { get; set; }
    }
}
