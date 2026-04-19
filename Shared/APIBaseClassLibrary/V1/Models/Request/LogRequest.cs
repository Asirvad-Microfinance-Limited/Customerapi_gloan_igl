using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.Models.Request
{
    public class GlAuditLogRequest
    {
        public string LOG_ID { get; set; }
        public string BRANCH_ID { get; set; }
        public string EMP_CODE { get; set; }
        public string URL { get; set; }
        public string METHOD { get; set; }
        public string CONTROLLER_NAME { get; set; }
        public string ACTION_NAME { get; set; }
        public string REQUEST { get; set; }
        //public string RESPONSE { get; set; }
        public string REQUEST_TIME { get; set; }
        //public string RESPONSE_TIME { get; set; }
    }
    public class GlAuditLogUpdateRequest
    {
        public string LOG_ID { get; set; }
        public string RESPONSE { get; set; }
        public string RESPONSE_TIME { get; set; }
        public string RESPONSE_STATUS { get; set; }
    }
    public class GlErrorLogRequest
    {
        public string LOG_ID { get; set; }
        public string BRANCH_ID { get; set; }
        public string EMP_CODE { get; set; }
        public string URL { get; set; }
        public string METHOD { get; set; }
        public string CONTROLLER_NAME { get; set; }
        public string ACTION_NAME { get; set; }
        public string EXCEPTION { get; set; }
        public string INNER_EXCEPTION { get; set; }
        public string STACKTRACE { get; set; }
    }
}
