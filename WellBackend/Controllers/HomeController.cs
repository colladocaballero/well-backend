using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WellBackend.Contexts;
using WellBackend.Models;
using WellBackend.Services;

namespace WellBackend.Controllers
{
    [Authorize(Policy ="WellUser")]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly WellDbContext _appDbContext;
        private readonly IUsersService _usersService;

        public HomeController(WellDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IUsersService usersService)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
            _usersService = usersService;
        }

        // GET api/home/mail@mail.com
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(string Id)
        {
            var user = await _appDbContext.UsersWell.SingleAsync(c => c.Id == Id);

            return new OkObjectResult(new
            {
                statusCode = 200,
                data = new
                {
                    user.Id,
                    user.Name,
                    user.Surname,
                    user.Email,
                    user.ProfilePicture,
                    user.Gender,
                    user.Country,
                    user.City,
                    user.Birthday
                }
            });
        }

        // GET api/home/mail@mail.com/GetFriends
        [HttpGet("{id}/{userId}/GetFriends")]
        public IActionResult GetFriends(string id, string userId)
        {
            var friends = _usersService.GetFriends(id, userId);

            return new OkObjectResult(new
            {
                statudCode = 200,
                data = friends
            });
        }

        // GET: api/home/search/query/mail@mail.com - Search query and user's id for excluding him/her from search results
        [HttpGet("search/{query}/{id}")]
        public IActionResult SearchUsers(string query, string id)
        {
            var users = _usersService.SearchUsers(query, id);

            return new OkObjectResult(new
            {
                statusCode = 200,
                data = users
            });
        }

        // PUT: api/home/mail@mail.com/ChangeProfilePicture
        [HttpPut("{id}/ChangeProfilePicture")]
        public async Task<IActionResult> ChangeProfilePicture(string id, [FromBody]Photo photo)
        {
            await _usersService.ChangeProfilePicture(id, photo);

            return NoContent();
        }

        // DELETE: api/home/mail@mail.com/mail2@mail2.com
        [HttpDelete("{user1Id}/{user2Id}")]
        public async Task<IActionResult> RemoveFriend(string user1Id, string user2Id)
        {
            await _usersService.RemoveFriend(user1Id, user2Id);

            return NoContent();
        }
    }
}
