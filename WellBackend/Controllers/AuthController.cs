using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WellBackend.Auth;
using WellBackend.Helpers;
using WellBackend.Models;
using WellBackend.ViewModels;

namespace WellBackend.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]User credentials)
        {
            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity == null)
            {
                return Unauthorized();
            }

            return Json(new
            {
                statusCode = 200,
                data = new
                {
                    id = identity.Claims.Single(c => c.Type == "id").Value,
                    authToken = await _jwtFactory.GenerateEncodedToken(credentials.Email, identity),
                    expiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
                }
            });
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userEmail, string password)
        {
            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByEmailAsync(userEmail);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (userToVerify.Password == password)
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userEmail, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
