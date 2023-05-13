using Microsoft.AspNetCore.Mvc;
using Pokerino.Server.Services;
using Pokerino.Shared.Models;
using Pokerino.Shared.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Pokerino.Server.Controllers
{


    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public IActionResult Create(UserCreateRequest model)
        {
            _userService.Create(model);
            return Ok(new { message = "User created" });
        }
    }
}

