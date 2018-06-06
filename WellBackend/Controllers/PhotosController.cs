using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellBackend.Models;
using WellBackend.Services;

namespace WellBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Photos")]
    public class PhotosController : Controller
    {
        private readonly IPhotosService _photosService;

        public PhotosController(IPhotosService photosService)
        {
            _photosService = photosService;
        }

        // GET: api/Photos/mail@mail.com
        [HttpGet("{userId}", Name = "GetByUserId")]
        public IActionResult GetByUserId(string userId)
        {
            return new OkObjectResult(new
            {
                statusCode = 200,
                data = _photosService.GetByUserId(userId)
            });
        }
        
        // POST: api/Photos
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile image, [FromForm]string userId)
        {
            return Created("ImageUploaded", new
            {
                statusCode = 201,
                data = await _photosService.UploadImage(image, userId)
            });
        }
        
        // PUT: api/Photos/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
