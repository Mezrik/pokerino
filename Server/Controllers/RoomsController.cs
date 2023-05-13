using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokerino.Shared.Models.Rooms;
using Pokerino.Server.Services;

namespace Pokerino.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomService _roomService;
        private IUserResolverService _userResolverService;
        private IMapper _mapper;

        public RoomsController(IRoomService roomService, IUserResolverService userResolverService, IMapper mapper)
        {
            _roomService = roomService;
            _userResolverService = userResolverService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public IActionResult Create(RoomCreateRequest model)
        {
            var user = _userResolverService.GetUser(HttpContext.User);

            var room = _roomService.CreateRoom(model, user);

            return Ok(room);
        }
    }
}

