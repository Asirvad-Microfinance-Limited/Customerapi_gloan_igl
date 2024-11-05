using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using KYCAPI.V1.BLL;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace KYCAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class KycIdTypesController : BaseController
    {
        IConfiguration configuration;
        IEKYCBLL eKYCBLL;
        public KycIdTypesController(IConfiguration iConfig, IEKYCBLL _eKYCBLL)
        {
            configuration = iConfig;
            eKYCBLL = _eKYCBLL;
        }

        #region KycIdTypesGet

        [HttpGet]
        public ActionResult<KycIdTypesResponse> Get()
        {
            if (ModelState.IsValid)
            {
                string authHeader = string.Empty;

                if (Request.Headers.TryGetValue("Authorization", out StringValues authToken))
                {
                    authHeader = authToken.First();
                }
                else
                {
                    return Forbid();
                }
                //EKYCBLL eKYCBLL = new EKYCBLL(configuration);
                return Ok(eKYCBLL.KYCIdTypesGet(authHeader));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion KycIdTypesGet
    }
}