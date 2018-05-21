using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Controllers
{
    public class AccountsController
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager, IMapper mapper, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpPost, Route("api/register")]
        public async Task<IActionResult> Register([FromBody]User newUser)
        {
            var userIdentity = _mapper.Map<User>(newUser);

            var result = await _userManager.CreateAsync(userIdentity, newUser.Password);

            await _appDbContext.UsersWell.AddAsync(new User
            {
                Id = newUser.Id,
                UserName = newUser.Name + newUser.Surname + newUser.Id,
                NormalizedUserName = newUser.Name + newUser.Surname + newUser.Id,
                Email = newUser.Email,
                NormalizedEmail = newUser.Email,
                Password = newUser.Password,
                Name = newUser.Name,
                Surname = newUser.Surname,
                Birthday = newUser.Birthday,
                Gender = newUser.Gender,
                JoinDate = newUser.JoinDate
            });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}
