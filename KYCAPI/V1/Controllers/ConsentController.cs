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
    [Route("api/[controller]")]
    [ApiController]
    public class ConsentController : BaseController
    {
        IConfiguration configuration;
        IConsentBLL consent;
        public ConsentController(IConfiguration iConfig, IConsentBLL _consent)
        {
            configuration = iConfig;
            consent = _consent;
        }
        // GET: api/Consent
        [HttpGet]
        public ActionResult<ConsentResponse> Get([FromQuery] ConsentRequestlang request)
        {
            if (ModelState.IsValid)
            {
               
                return Ok(consent.getConsent(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Consent
        [HttpPost]
        public ActionResult<ConsentResponse> Post([FromBody] ConsentRequest request)
        {
            if (ModelState.IsValid)
            {
                
                return Ok(consent.addConsent(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
