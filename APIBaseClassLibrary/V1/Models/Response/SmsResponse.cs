using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.Models.Response
{
    public class SmsResponse : BaseResponse
    {

        #region SmsRecived


        public object Outmessage { get; set; }

        #endregion SmsRecived

        public class SentStatus
        {
            public string message { get; set; }
            public string code { get; set; }
            public string flag { get; set; }
        }


    }
}
