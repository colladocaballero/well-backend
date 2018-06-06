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

namespace WellBackend.Controllers
{
    [Authorize(Policy ="WellUser")]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly WellDbContext _appDbContext;

        public HomeController(UserManager<User> userManager, WellDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
        }

        // GET api/home/home
        [HttpGet("{id}")]
        public async Task<IActionResult> Home(string Id)
        {
            // retrieve the user info
            //HttpContext.User
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
    }
}
