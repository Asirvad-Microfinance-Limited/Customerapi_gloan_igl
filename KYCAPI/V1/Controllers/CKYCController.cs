using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KYCAPI.V1.Models.Response;
using KYCAPI.V1.Models.Request;
using KYCAPI.V1.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIBaseClassLibrary.V1.Controllers;
using Microsoft.Extensions.Configuration;
using KYCAPI.V1.BLLDependency;

namespace KYCAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CKYCController : BaseController
    {
        IConfiguration configuration;
        ICKYCBLL ckyc;
        public CKYCController(IConfiguration iConfig, ICKYCBLL _ckyc)
        {
            configuration = iConfig;
            ckyc = _ckyc;
        }
        // GET: api/CKYC
        [HttpPost]
        public ActionResult<CKYCResponse> Post([FromBody] CKYCRequest request)
        {
            if (ModelState.IsValid)
            {
               // CKYCBLL ckyc = new CKYCBLL();
                return Ok(ckyc.addCKYCDetails(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/CKYC
        [HttpGet]
        public ActionResult<CKYCMasterDataResponse> Get([FromQuery] CKYCMasterDataRequest cKYCRequest)
        {
            if (ModelState.IsValid)
            {
                //CKYCBLL ckyc= new CKYCBLL();
                return Ok(ckyc.getCKYCDetails(cKYCRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

}

}
