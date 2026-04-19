using System;
using System.Collections.Generic;
using System.Text;
using static GlobalLibrary.GlobalVariables;

namespace APIBaseClassLibrary.V1.Models.Response
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            status = new ResponseStatus();
            status.flag = ProcessStatus.failed;
            status.code = APIStatus.failed;
            status.message = "Failed";
            status.timeStamp = DateTime.Now.ToString(Constants.Strings.dateTimeFormat);
        }
        public ResponseStatus status { get; set; }
    }
}
