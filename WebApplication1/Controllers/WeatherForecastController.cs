using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFIdentityAuthBasic.Helpers;
using EFIdentityAuthBasic.Models;
using EFIdentityAuthBasic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private IUserService userService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         IUserService UserService)
        {
            _logger = logger;
            userService = UserService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AuthUserModel authUser)
        {
            User user = userService.Authenticate(authUser.Email, authUser.Password);
            if (user == null) return BadRequest(new { message = "Bad email or password" });

            string token = JWTGenerator.GetToken("cQfTjWnZr4u7x!z%", user.ID.ToString());

            return Ok(new
            {
                ID = user.ID,
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserModel authUser)
        {
            User user = new User();
            user.Email = authUser.Email;
            try
            {
                userService.Create(user, authUser.Password);
                return Ok(new { message = "User created" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
