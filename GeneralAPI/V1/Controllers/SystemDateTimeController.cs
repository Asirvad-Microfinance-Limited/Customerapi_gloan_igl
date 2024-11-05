using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/systemdatetime")]
    [ApiController]
    public class SystemDateTimeController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public SystemDateTimeController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get db DateAndTime </summary>
        /// <returns><see cref="DateAndTimeResponse"/></returns>

        [HttpGet]
        public ActionResult<DateAndTimeResponse> Get()
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                DateAndTimeResponse dateAndTimeResponse = new DateAndTimeResponse();
                dateAndTimeResponse = generalBLL.getDateAndTime();
                return Ok(dateAndTimeResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}