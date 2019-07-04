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
    public class AuthController : ApiControllerBase
    {
        private IUserService _userService;
        private IAutoMapperProfile _mapper;

        public AuthController(
            IUserService userService,
            IAutoMapperProfile mapper
            )
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            try
            {
                var token = _userService.Authenticate(userDto.Email, userDto.Password);
                if (token == null)
                    return Error(new {Message = "Les informations fournies sont incorrects"});
                return Success(new
                {
                    Username = userDto.Email,
                    Token = token
                });
            }
            catch
            {
                return Error(new {Message = "Les informations fournies sont incorrects"});
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
                return Success(new { Message = "Opération réussi" });
            }
            catch
            {
                return Error(new { Message = "Impossible de créer cet identifiant" });
            }
        }
    }
}
