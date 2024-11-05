using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using KYCAPI.V1.BLL;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KYCAPI.V1.Controllers
{
    [Route("api/v1/aadhaar")]
    [ApiController]
    public class AadhaarController : BaseController
    {
        IConfiguration configuration;
        IEKYCBLL eKYCBLL;
        public AadhaarController(IConfiguration iConfig, IEKYCBLL _eKYCBLL)
        {
            eKYCBLL = _eKYCBLL;
            configuration = iConfig;
        }

        [HttpGet]
        public ActionResult<AadharDataResponse> Get([FromQuery] AadharDataRequest aadharDataRequest)
        {
            if (ModelState.IsValid)
            {
                //EKYCBLL eKYCBLL = new EKYCBLL(configuration);
                return Ok(eKYCBLL.customerKYCDisplaydata(aadharDataRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        public ActionResult<AddAadhaarResponse> Post(AddAadhaarRequest addAadhaarRequest)
        {
            if (ModelState.IsValid)
            {
                //EKYCBLL eKYCBLL = new EKYCBLL(configuration);
                return Ok(eKYCBLL.addAadhaar(addAadhaarRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}