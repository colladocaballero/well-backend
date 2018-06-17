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
    [Route("api/FriendRequests")]
    public class FriendRequestsController : Controller
    {
        private readonly IFriendRequestsService _friendRequestsService;

        public FriendRequestsController(IFriendRequestsService friendRequestsService)
        {
            _friendRequestsService = friendRequestsService;
        }

        // GET: api/FriendRequests/mail@mail.com
        [HttpGet("{id}", Name = "GetUserFriendRequests")]
        public IActionResult GetUserFriendRequests(string id)
        {
            return new OkObjectResult(new
            {
                statusCode = 200,
                data = new
                {
                    requestsCount = _friendRequestsService.GetRequestsCount(id),
                    friendRequests = _friendRequestsService.GetUserFriendRequests(id)
                }
            });
        }
        
        // POST: api/FriendRequests
        [HttpPost]
        public async Task<IActionResult> SendFriendRequest([FromBody]FriendRequest friendRequest)
        {
            try
            {
                await _friendRequestsService.SendFriendRequest(friendRequest);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return Created("Request sent", new
            {
                statusCode = 201,
                data = new
                {
                    message = "Friend request sent"
                }
            });
        }
        
        // PUT: api/FriendRequests/5
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
