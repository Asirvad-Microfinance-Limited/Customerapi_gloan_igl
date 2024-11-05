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

namespace KYCAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JurisdictionController : BaseController
    {
        // GET: api/CKYC
        IConfiguration configuration;
        IJurisdictionBLL ckyc;
        public JurisdictionController(IConfiguration iConfig, IJurisdictionBLL _ckyc)
        {
            configuration = iConfig;
            ckyc = _ckyc;
        }
        [HttpGet]
        public ActionResult<JurisdictionResponse> Get()
        {
            if (ModelState.IsValid)
            {
                //JurisdictionBLL ckyc = new JurisdictionBLL();
                return Ok(ckyc.getJurisdictionDetails());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
