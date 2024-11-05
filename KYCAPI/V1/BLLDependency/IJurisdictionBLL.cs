using KYCAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.BLLDependency
{
    public interface IJurisdictionBLL
    {
        JurisdictionResponse getJurisdictionDetails();
    }
   
}
