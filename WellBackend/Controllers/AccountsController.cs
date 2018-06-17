using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WellBackend.Auth;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Controllers
{
    public class AccountsController : Controller
    {
        private readonly WellDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AccountsController(UserManager<User> userManager, IMapper mapper, WellDbContext appDbContext, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost, Route("api/register")]
        public async Task<IActionResult> Register([FromBody]User newUser)
        {
            var userCheck = await _appDbContext.UsersWell.SingleOrDefaultAsync(User => User.Id == newUser.Email);

            if (userCheck == null)
            {
                var userIdentity = _mapper.Map<User>(newUser);

                var result = await _userManager.CreateAsync(userIdentity, newUser.Password);

                await _appDbContext.UsersWell.AddAsync(new User
                {
                    Id = newUser.Email,
                    UserName = newUser.Name + newUser.Surname + newUser.Email,
                    NormalizedUserName = newUser.Name + newUser.Surname + newUser.Email,
                    Email = newUser.Email,
                    NormalizedEmail = newUser.Email,
                    Password = newUser.Password,
                    Name = newUser.Name,
                    Surname = newUser.Surname,
                    Birthday = newUser.Birthday,
                    Gender = newUser.Gender,
                    JoinDate = DateTime.Now,
                    ProfilePicture = "default/" + (newUser.Gender == "Masculino" ? "male_default.jpg" : "female_default.jpg")
                });

                var identity = await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(newUser.Email, newUser.Id));

                await _appDbContext.SaveChangesAsync();

                return Created("NewUser", new
                {
                    statusCode = 201,
                    data = new
                    {
                        id = newUser.Email,
                        authToken = await _jwtFactory.GenerateEncodedToken(newUser.Email, identity),
                        expiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
                    }
                });
            }
            else
            {
                return StatusCode(409, new
                {
                    statusCode = 409,
                    data = new { }
                });
            }
        }
    }
}
