using System;
using Pokerino.Shared.Entities;
using System.Security.Claims;

namespace Pokerino.Server.Services
{
    public interface IUserResolverService
    {
        User? GetUser(ClaimsPrincipal principal);
    }

    public class UserResolverService : IUserResolverService
    {
        private readonly IUserService _userService;

        public UserResolverService(IUserService userService)
        {
            _userService = userService;
        }

        public User? GetUser(ClaimsPrincipal principal)
        {
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null || userIdClaim.Value == null)
            {
                return null;
            }

            return _userService.GetById(int.Parse(userIdClaim.Value));
        }
    }
}

