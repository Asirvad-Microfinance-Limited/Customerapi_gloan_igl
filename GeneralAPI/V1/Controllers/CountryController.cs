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
    [Route("api/v1/country")]
    [ApiController]
    public class CountryController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public CountryController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get country details </summary>
        /// <returns><see cref="BranchResponse"/></returns>
        [HttpGet]
        public ActionResult<CountriesResponse>  Get()
        {
            if (ModelState.IsValid)
            {
               // GeneralBLL generalBLL = new GeneralBLL();
                CountriesResponse countriesResponse = new CountriesResponse();
                countriesResponse = generalBLL.getCountries();
            return Ok(countriesResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
