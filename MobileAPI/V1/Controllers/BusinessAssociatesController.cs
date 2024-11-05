using APIBaseClassLibrary.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobileAPI.V1.BLLDependency;
using MobileAPI.V1.Models.Request;
using MobileAPI.V1.Models.Response;
using MoblieAPI.V1.BLL;

namespace MoblieAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
  
    public class BusinessAssociatesController : BaseController
    {
        IConfiguration configuration;
        IMobileBLL mobile;
        public BusinessAssociatesController(IConfiguration iConfig, IMobileBLL _mobile)
        {
            mobile = _mobile;
            configuration = iConfig;
        }

        [HttpGet]
        public ActionResult<MobileResponse> Get([FromQuery]MobileRequest request)
        {
            if (ModelState.IsValid)
            {
                //MobileBLL mobile = new MobileBLL();
                return Ok(mobile.getbaMobile(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}