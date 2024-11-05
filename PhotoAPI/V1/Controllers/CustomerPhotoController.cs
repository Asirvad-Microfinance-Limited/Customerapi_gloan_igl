using APIBaseClassLibrary.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhotoAPI.V1.BLL;
using PhotoAPI.V1.BLLDependency;
using PhotoAPI.V1.Models.Request;
using PhotoAPI.V1.Models.Response;

namespace PhotoAPI.V1.Controllers
{
    [Route("api/v1/customerphoto")]
    [ApiController]
    public class CustomerPhotoController : BaseController
    {
        IConfiguration configuration;
        IPhotoBLL photo;
        public CustomerPhotoController(IConfiguration iConfig, IPhotoBLL _photoBll)
        {
            photo = _photoBll;
            configuration = iConfig;
        }
        #region CustomerPhotoGet

        [HttpGet]
        public ActionResult<CustomerPhotoResponse>  Get([FromQuery]CustomerPhotoRequest customerPhotoRequest)
        {
            if (ModelState.IsValid)
            {
               // PhotoBLL photoBLL = new PhotoBLL();
                CustomerPhotoResponse customerPhotoResponse = new CustomerPhotoResponse();
                customerPhotoResponse = photo.getCustomerPhoto(customerPhotoRequest);
                return customerPhotoResponse;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion CustomerPhotoGet

        #region CustomerPhotoPost

        [HttpPost]
        public ActionResult<AddPhotoResponse> Post([FromBody]AddPhotoRequest addPhotoRequest)
        {
            if (ModelState.IsValid)
            {
              //  PhotoBLL photoBLL = new PhotoBLL();
                AddPhotoResponse addPhotoResponse = new AddPhotoResponse();
                addPhotoResponse = photo.addCustomerPhoto(addPhotoRequest);
                return Ok(addPhotoResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion CustomerPhotoPost


        #region CustomerPhotoPut

        [HttpPut]
        public ActionResult<UpdatePhotoResponse> Put([FromBody]UpdatePhotoRequest updatePhotoRequest)
        {
            if (ModelState.IsValid)
            {
                //  PhotoBLL photoBLL = new PhotoBLL();
                UpdatePhotoResponse updatePhotoResponse = new UpdatePhotoResponse();
                updatePhotoResponse = photo.updateCustomerPhoto(updatePhotoRequest);
                return Ok(updatePhotoResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion CustomerPhotoPost

    }
}