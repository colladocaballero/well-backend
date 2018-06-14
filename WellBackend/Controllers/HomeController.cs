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
        [HttpGet("{id}/GetFriends")]
        public IActionResult GetFriends(string id)
        {
            var friends = _usersService.GetFriends(id);

            return new OkObjectResult(new
            {
                statudCode = 200,
                data = friends
            });
        }
    }
}
