using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CatDataBank.Helper;
using CatDataBank.Model;
using CatDataBank.Model.Dto;
using CatDataBank.Service;

namespace CatDataBank.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private IUserService _userService;
        private IAutoMapperProfile _mapper;

        private readonly AppSettings _appSettings;
        AppDbContext _applicationDbContext = new AppDbContext();

        public AuthController(
            IUserService userService,
            IAutoMapperProfile mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_applicationDbContext.Users);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            try
            {
                var token = _userService.Authenticate(userDto.Email, userDto.Password);
                if (token == null)
                    return BadRequest();
                return Ok(new
                {
                    Username = userDto.Email,
                    Token = token
                });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            var user = _mapper.GetMapper().Map<User>(userDto);

            try
            {
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
