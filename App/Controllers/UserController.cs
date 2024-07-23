using App.Dtos.CreateDtos;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       
        [HttpPost("register")]
        public ActionResult RegisterUser(RegisterUserDto dto)
        {
            _userService.RegisterNewUser(dto);

            return Created("", null);
        }






    }
}
