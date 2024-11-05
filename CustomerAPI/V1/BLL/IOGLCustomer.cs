using CustomerAPI.V1.Models.Request;
using CustomerAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.BLL
{
    public interface IOGLCustomer
    {
        OGLCustomerResponse searchOGLCustomer(OGLCustomerRequest request);
    }
}
