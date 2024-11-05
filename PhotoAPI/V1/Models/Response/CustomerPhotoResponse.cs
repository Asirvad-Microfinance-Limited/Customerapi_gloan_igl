using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAPI.V1.Models.Response
{
    public class CustomerPhotoResponse : BaseResponse
    {
        public string custPhoto { get; set; }
    }
}
