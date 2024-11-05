using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/districts")]
    [ApiController]
   

    public class DistrictsController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public DistrictsController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get districts details </summary>
        /// <param name="DistrictRequest"><see cref="DistrictRequest"/></param>
        /// <returns><see cref="DistrictResponse"/></returns>
        [HttpGet]
       
        public ActionResult<DistrictResponse> Get([FromQuery]DistrictRequest districtRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                DistrictResponse districtResponse = new DistrictResponse();
                districtResponse = generalBLL.getDistricts(districtRequest);
                return Ok(districtResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}