using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellBackend.Contexts;
using WellBackend.Models;
using WellBackend.Services;

namespace WellBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        // GET: api/Comments/test@test.com
        [HttpGet("{id}", Name = "GetWallComments")]
        public IActionResult GetWallComments(string id)
        {
            var result = _commentsService.GetWallComments(id);

            //return Json(result);
            return Ok(new
            {
                statusCode = 200,
                data = Json(result)
            });
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Comment newComment)
        {
            await _commentsService.AddComment(newComment);

            return Created("NewComment", new
            {
                statusCode = 201,
                data = new
                {
                    message = "Comment created"
                }
            });
        }
        
        // PUT: api/Comments/5
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
